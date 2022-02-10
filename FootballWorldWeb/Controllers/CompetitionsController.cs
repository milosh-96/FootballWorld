using FootballWorld.Data;
using FootballWorldWeb.Data;
using FootballWorldWeb.Models.Competitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Controllers
{
    public class CompetitionsController : Controller
    {
        private readonly FootballDbContext dbContext;

        public CompetitionsController(FootballDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("/Competition/{slug}/{id}")]
        public IActionResult Overview(int id,string slug)
        {
            Competition competition = dbContext.Competitions.Include(x=>x.Seasons).Where(x => x.Id == id).FirstOrDefault();
            if(competition ==null) { return new NotFoundResult(); }
            ViewData["Title"] = String.Format("{0} - Overview", competition.Name);
            CompetitionOverviewViewModel viewModel = new CompetitionOverviewViewModel();
            viewModel.Name = competition.Name;
            viewModel.Id = competition.Id;
            viewModel.Logo = competition.CompetitionLogo;
            viewModel.Slug = competition.Slug;
            viewModel.Seasons = competition.Seasons;
            return View(viewModel);
        }
       

        public IActionResult GroupView() {
            return View();

        }
    }
}
