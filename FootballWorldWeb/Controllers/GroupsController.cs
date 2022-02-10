using FootballWorld.Data;
using FootballWorldWeb.Data;
using FootballWorldWeb.Models.Groups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Controllers
{
    public class GroupsController : Controller
    {
        private readonly FootballDbContext dbContext;

        public GroupsController(FootballDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("{slug}/{id}")]
        public IActionResult Details(int id)
        {
            Group group = dbContext.Groups.Where(x => x.Id == id)
                .Include(x=>x.Season)
                .ThenInclude(x=>x.Competition)
                .Include(x=>x.Standings)
                .ThenInclude(x=>x.Items).ThenInclude(x=>x.Team)
                .Include(x=>x.Matches).ThenInclude(x=>x.Results).ThenInclude(x=>x.Team)
                .FirstOrDefault();
            if(group == null) { return new NotFoundResult(); }
            GroupViewViewModel viewModel = new GroupViewViewModel();
            viewModel.Id = id;
            viewModel.Standings = group.Standings.FirstOrDefault();
            viewModel.Matches = group.Matches;
            viewModel.Group = group;
            ViewData["Title"] = String.Format("{0} - {1} ({2})", group.Name, group.Season.Name,group.Season.Competition.Name);
            return View(viewModel);
        }
    }
}
