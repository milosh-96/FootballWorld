using CsvHelper;
using CsvHelper.Configuration;
using FootballWorld.Data;
using FootballWorldWeb.Areas.UserPanel.Models;
using FootballWorldWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class TeamsController : Controller
    {
        private readonly FootballDbContext dbContext;

        public TeamsController(FootballDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "List of Teams";
            return View(dbContext.Teams.ToList());
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Create new Team";
            return View(new TeamFormViewModel()) ;
        }
        [HttpPost]
        public IActionResult Create(TeamFormViewModel formData)
        {
            try
            {
                Team team = new Team();
                team.Name = formData.Name.Trim();
                team.Slug = new Slugify.SlugHelper().GenerateSlug(formData.Name.Trim());
                team.TeamType = formData.SelectedTeamType;

                if (formData.LogoUploadFile != null)
                {
                    team.TeamLogo = formData.LogoUploadFile.FileName;
                    Services.UploadService.Upload(formData.LogoUploadFile);
                }
                dbContext.Teams.Add(team);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(formData);
            }
        }

        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Edit Team";
            TeamFormViewModel viewModel = new TeamFormViewModel();
            Team team = dbContext.Teams.Where(x => x.Id == id).FirstOrDefault();
            if(team == null) { return new NotFoundResult(); }
            viewModel.Id = team.Id;
            viewModel.Name = team.Name;
            viewModel.TeamLogo = team.TeamLogo;
            viewModel.SelectedTeamType = team.TeamType;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(int id,TeamFormViewModel formData)
        {
            try
            {
                Team team = dbContext.Teams.Where(x => x.Id == id).FirstOrDefault();
                if (team == null) { return new NotFoundResult(); }
                formData.TeamLogo = team.TeamLogo;
                team.Name = formData.Name.Trim();
                team.TeamType = formData.SelectedTeamType;
                team.Slug = new Slugify.SlugHelper().GenerateSlug(formData.Name.Trim());
                if (formData.ReuploadLogo)
                {
                    if (formData.LogoUploadFile != null) { 
                        team.TeamLogo = formData.LogoUploadFile.FileName;
                   Services.UploadService.Upload(formData.LogoUploadFile);
                    }
                }
                var result = dbContext.SaveChanges();
                if (result < 1) { throw new Exception("bad request"); }
                return RedirectToAction("Edit", new { id = id });
            }
            catch(Exception e)
            {
                return View(formData);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Team team = dbContext.Teams.Where(x => x.Id == id).FirstOrDefault();
            var results = dbContext.MatchResults.Where(x => x.Team == team).ToList();
            if(results.Count > 0)
            {
                TempData["Error"] = "You can't delete this team because there are matches that are associated with the team.";
                return RedirectToAction("Edit", new { id = id });
            }
            if(team == null)
            {
                return new NotFoundResult();
            }
            dbContext.Teams.Remove(team);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Import()
        {
            ViewData["Title"] = "Import Teams using a file";
            ImportFormViewModel viewModel = new ImportFormViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult ImportPreview(ImportFormViewModel formData)
        {
            ViewData["Title"] = "Preview of imported file";
            ImportPreviewViewModel<Team> viewModel = new ImportPreviewViewModel<Team>();
            //parse file and pass it to viewmodel//
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) {HasHeaderRecord=true,IgnoreBlankLines = true };
            using (var reader = new StreamReader(formData.UploadedFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                var records = csv.GetRecords<CsvTeam>();
                foreach(CsvTeam csvTeam in records)
                {
                    if (!dbContext.Teams.Any(x => x.Name == csvTeam.Name) && !String.IsNullOrEmpty(csvTeam.Name))
                    {
                        viewModel.Items.Add(new Team() { 
                            Name = csvTeam.Name.Trim(), 
                            TeamLogo = csvTeam.TeamLogo.Trim(), 
                            Slug = new Slugify.SlugHelper().GenerateSlug(csvTeam.Name),
                            TeamType= (TeamType)(int.Parse(csvTeam.TeamType))
                        });
                    }
                 }
                dbContext.Teams.AddRange(viewModel.Items);
                dbContext.SaveChanges();
            }
            return View(viewModel);
        }
       
    }
}
