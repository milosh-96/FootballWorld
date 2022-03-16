using FootballWorld.Data;
using FootballWorldWeb.Areas.UserPanel.Models;
using FootballWorldWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]


    public class CompetitionsController : Controller
    {

        private readonly FootballDbContext dbContext;

        private string uploadSubFolder = "competitions";

        public CompetitionsController(FootballDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Overview(int id)
        {
            Competition competition = dbContext.Competitions.Include(x => x.Seasons).Where(x => x.Id == id).FirstOrDefault();
            if(competition == null)
            {
                return new NotFoundResult();
            }
            CompetitionOverviewViewModel viewModel = new CompetitionOverviewViewModel();
            viewModel.Id = id;
            viewModel.Competition = competition;
            return View(viewModel);
        }

        public IActionResult Index() {
            ViewData["Title"] = "List of Competitions";
            List<Competition> competitions = dbContext.Competitions.Where(x=>x.Parent==null).Include(x=>x.Children).ToList();
            return View(competitions);
        }   
        public IActionResult Create() {
            ViewData["Title"] = "Create a Competition";
            CompetitionFormViewModel viewModel = new CompetitionFormViewModel();
            var selectList = new SelectList(dbContext.Competitions.ToList().Prepend(new Competition() { Id = 0, Name = "--No Parent--" }).ToList(), "Id", "Name");
            viewModel.ParentSelectList = selectList;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(CompetitionFormViewModel formData)
        {
            try
            {
                Competition competition = new Competition();
                competition.Name = formData.Name;
                competition.Slug = new Slugify.SlugHelper().GenerateSlug(formData.Name);
                if (formData.ReuploadLogo)
                {
                    if (formData.LogoUploadFile != null)
                    {
                        Services.UploadService.Upload(formData.LogoUploadFile,this.uploadSubFolder);
                        competition.CompetitionLogo = formData.LogoUploadFile.FileName;
                    }
                }
                if(formData.SelectedCompetitionId > 0)
                {
                    competition.Parent = dbContext.Competitions.Where(x=>x.Id==formData.SelectedCompetitionId).FirstOrDefault();
                }
                dbContext.Add(competition);
                if(dbContext.SaveChanges()>0)
                {
                    return RedirectToAction("Index");
                }
                return new BadRequestResult();
            }catch(Exception e)
            {
                return View(formData);
            }
        }
        public IActionResult Edit(int id) {
            ViewData["Title"] = "Edit Competition";
            Competition competition = dbContext.Competitions.Where(x => x.Id == id).FirstOrDefault();
            if (competition == null) { return new NotFoundResult(); }
            CompetitionFormViewModel viewModel = new CompetitionFormViewModel();
            viewModel.Id = competition.Id;
            viewModel.Name = competition.Name;
            viewModel.CompetitionLogo = competition.CompetitionLogo;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(int id,CompetitionFormViewModel formData)
        {
            Competition competition = dbContext.Competitions.Where(x => x.Id == id).FirstOrDefault();
            if(competition == null) { return new NotFoundResult(); }
            if(formData.ReuploadLogo)
            {
                if(formData.LogoUploadFile != null)
                {
                    Services.UploadService.Upload(formData.LogoUploadFile, this.uploadSubFolder);
                    competition.CompetitionLogo = formData.LogoUploadFile.FileName;
                }
            }
            competition.Name = formData.Name;
            competition.Slug = new Slugify.SlugHelper().GenerateSlug(formData.Name);
            if (dbContext.SaveChanges() > 0) {
                TempData["Message"] = "Updated";
            }
            return RedirectToAction("Edit", new { id = competition.Id });
        }
        [HttpPost]
        public IActionResult Delete(int id) {
            Competition competition = dbContext.Competitions.Where(x => x.Id == id)
                .Include(x => x.Children)
                .FirstOrDefault();
            if(competition == null) { return new NotFoundResult(); }
            dbContext.Competitions.Remove(competition);
            if(dbContext.SaveChanges()>0)
            {
                TempData["Message"] = "Deleted";
                return RedirectToAction("Index");
            }
            ViewData["Erorr"] = "Error";
            return RedirectToAction("Edit", new { id = competition.Id });
        }   
    }
}
