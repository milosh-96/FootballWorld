using FootballWorld.Data;
using FootballWorldWeb.Data;
using FootballWorldWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class StandingsController : Controller
    {
        private readonly FootballDbContext dbContext;
        private readonly StandingsCalculatorService standingsCalculatorService;

        public StandingsController(FootballDbContext dbContext, StandingsCalculatorService standingsCalculatorService)
        {
            this.dbContext = dbContext;
            this.standingsCalculatorService = standingsCalculatorService;
        }

        public IActionResult Update(int groupId)
        {
            if(groupId < 0) { return new BadRequestResult();}
            Group group = dbContext.Groups.Where(x => x.Id == groupId).Include(x => x.Standings).FirstOrDefault();
            if (group == null) { return new NotFoundResult(); }
            standingsCalculatorService.UpdateDbStandings(group.Id);
            return RedirectToAction("Overview","Seasons",new { id = group.SeasonId });
        }
    }
}
