using FootballWorld.Data;
using FootballWorldWeb.Data;
using FootballWorldWeb.Models;
using FootballWorldWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FootballDbContext dbContext;
        private readonly StandingsCalculatorService standingsCalculator;

        public HomeController(ILogger<HomeController> logger,
            FootballDbContext dbContext, StandingsCalculatorService standingsCalculator)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.standingsCalculator = standingsCalculator;
        }

       
        public IActionResult Index(IndexViewModel formData)
        {
            ViewData["Title"] = "Centre Circle";

            IndexViewModel viewModel = new IndexViewModel();
            viewModel.Competitions = dbContext.Competitions.Where(x => x.ParentId == null).ToList();
            return View(viewModel);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
