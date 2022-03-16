using FootballWorld.Data;
using FootballWorldWeb.Areas.UserPanel.Models;
using FootballWorldWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class SeasonsController : Controller
    {
        private readonly FootballDbContext dbContext;

        public SeasonsController(FootballDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IActionResult Overview(int? id)
        {
            if(id==null) {
                TempData["Error"] = "You must provide season ID";
                return RedirectToAction("Home", "Index");
            }
            Season season = dbContext.Seasons
                .Include(x => x.Competition)
                .Include(x => x.Groups)
                .ThenInclude(x => x.GroupTeams)
                .Where(x=>x.Id==id)
                .FirstOrDefault();
            if(season==null) { return new NotFoundResult(); }
            SeasonOverviewViewModel viewModel = new SeasonOverviewViewModel();
            viewModel.Id = (int) id;
            viewModel.Season = season;
            ViewData["Title"] = String.Format("Season Overview {0}", season.Name);
            return View(viewModel);
        }
        public IActionResult Index(int? competitionId)
        {
            if(competitionId == null)
            {
                TempData["Error"] = "You must provide competition ID";
                return RedirectToAction("Index", "Home");
            }
            Competition competition = dbContext.Competitions.Where(x => x.Id == competitionId).FirstOrDefault();
            if(competition == null)
            {
                return new NotFoundResult();
            }

            ViewData["Title"] = "List of Seasons";
            List<Season> seasons = dbContext.Seasons.Where(x=>x.CompetitionId==competitionId).Include(x=>x.Competition).ToList();
            return View(seasons);
        }
        public IActionResult Create(int competitionId)
        {
            Competition competition = dbContext.Competitions.Where(x => x.Id == competitionId).FirstOrDefault();
            if (competition == null)
            {
                return new NotFoundResult();
            }
            ViewData["Title"] = "Create a Season";
            SeasonFormViewModel viewModel = new SeasonFormViewModel();
            viewModel.CompetitionId = competition.Id;
            viewModel.Competition = competition;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(SeasonFormViewModel formData)
        {
            Season season = new Season();
            season.Name = formData.Name;
            season.StartYear = formData.StartYear;
            season.EndYear = formData.EndYear;
            season.CompetitionId = formData.CompetitionId;
            string slug = String.Format("{0}-{1}", dbContext.Competitions.Where(x => x.Id == formData.CompetitionId).FirstOrDefault().Name, formData.Name);
            season.Slug = new Slugify.SlugHelper().GenerateSlug(slug);

            dbContext.Seasons.Add(season);
            if(dbContext.SaveChanges()>0)
            {
                return RedirectToAction("Index", new { competitionId = formData.CompetitionId });
            }
            return new BadRequestResult();
        }
        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Edit Season";
            Season season = dbContext.Seasons.Where(x => x.Id == id).FirstOrDefault();
            if(season==null)
            {
                return new NotFoundResult();
            }
            SeasonFormViewModel viewModel = new SeasonFormViewModel();
            viewModel.Id = season.Id;
            viewModel.Name = season.Name;
            viewModel.StartYear = season.StartYear;
            viewModel.EndYear = season.EndYear;
            return View(viewModel);
                 
        }

        [HttpPost]
        public IActionResult Edit(SeasonFormViewModel formData) {
            Season season = dbContext.Seasons.Include(x => x.Competition).Where(x => x.Id == formData.Id).FirstOrDefault();
            if (season == null) { return new NotFoundResult(); }
            season.Name = formData.Name;
            season.Slug = new Slugify.SlugHelper().GenerateSlug(String.Format("{0}-{1}",season.Competition.Slug,season.Name));
            if (dbContext.SaveChanges() > 0)
            {
                TempData["Message"] = "Success";
                return RedirectToAction("Edit", new { id = season.Id });
            }
            TempData["Error"] = "Error happened.";
            return RedirectToAction("Edit", new { id = season.Id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Season season = dbContext.Seasons.Where(x => x.Id == id).FirstOrDefault();
            if(season==null) { return new NotFoundResult(); }
            dbContext.Seasons.Remove(season);
            dbContext.SaveChanges();
            return RedirectToAction("Overview", "Competitions",new { id = season.CompetitionId });
        }
    }
}
