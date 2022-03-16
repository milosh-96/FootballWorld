using FootballWorld.Data;
using FootballWorldWeb.Data;
using FootballWorldWeb.Models.Competitions;
using FootballWorldWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Controllers
{
    public class SeasonsController : Controller
    {
        private readonly ILogger<SeasonsController> _logger;
        private readonly FootballDbContext dbContext;
        private readonly StandingsCalculatorService standingsCalculator;

        public SeasonsController(ILogger<SeasonsController> logger,
            FootballDbContext dbContext, StandingsCalculatorService standingsCalculator)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.standingsCalculator = standingsCalculator;
        }

        public IActionResult Details(int id,string slug)
        {
            Season season = dbContext.Seasons
                .Include(c => c.Competition)
                .Include(g => g.Groups)
                .ThenInclude(g=>g.GroupTeams).ThenInclude(t=>t.Team)
                .Include(g=>g.Groups)
                .ThenInclude(g => g.Standings)
                .ThenInclude(s => s.Items).ThenInclude(t => t.Team)
                    .Include(g => g.Groups)
                    .ThenInclude(g => g.Matches).ThenInclude(t=>t.Results).ThenInclude(t=>t.Team)
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                if (season == null) { return new NotFoundResult(); }
                SeasonViewViewModel viewModel = new SeasonViewViewModel();
                viewModel.Season = season;
                viewModel.Competition = season.Competition;
                viewModel.Groups = season.Groups;
                viewModel.SingleGroup = season.Groups.Count > 1 ? false : true;
            viewModel.Lists = season.Groups.Where(x => x.GroupType == GroupType.TeamsList).ToList();
                ViewData["Title"] = String.Format("Season {0} of {1}", viewModel.Season.Name, viewModel.Competition.Name);

                return View(viewModel);
                //todo create view for seasonview//
            }
        }
    }
