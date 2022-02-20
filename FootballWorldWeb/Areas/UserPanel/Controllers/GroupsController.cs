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
    public class GroupsController : Controller
    {
        private readonly FootballDbContext dbContext;
        public GroupsController(FootballDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Create(int? seasonId)
        {
            if(seasonId == null) {
                TempData["Error"] = "You must provide season ID";
                return RedirectToAction("Index", "Home");
            }
            Season season = dbContext.Seasons.Where(x => x.Id == seasonId).FirstOrDefault();
            if(season==null)
            {
                return new NotFoundResult();
            }

            ViewData["Title"] = "Create Group";
            GroupFormViewModel viewModel = new GroupFormViewModel();
            viewModel.SeasonId = season.Id;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(GroupFormViewModel formData)
        {
            Group group = new Group();
            group.Name = formData.Name;
            group.GroupType = (GroupType) Enum.Parse(typeof(GroupType),formData.GroupTypeValue.ToString());
            group.SeasonId = formData.SeasonId;
            Season season = dbContext.Seasons.Where(x => x.Id == group.SeasonId).Include(x => x.Competition).FirstOrDefault();
            string slug = String.Format(
                "{0}-{1}",
                season.Slug,
                new Slugify.SlugHelper().GenerateSlug(formData.Name)
                );
            group.Slug = slug;
            dbContext.Groups.Add(group);
            if(dbContext.SaveChanges() > 0)
            {
                TempData["Message"] = "Successfully Added";
            }
            else
            {
                TempData["Error"] = "Error";
            }
            return RedirectToAction("Edit", new { id = group.Id });
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "You must provide season ID";
                return RedirectToAction("Index", "Home");
            }
            Group group = dbContext.Groups.Where(x => x.Id == id).FirstOrDefault();
            if (group == null)
            {
                return new NotFoundResult();
            }

            ViewData["Title"] = "Edit Group";
            GroupFormViewModel viewModel = new GroupFormViewModel();
            viewModel.SeasonId = group.SeasonId;
            viewModel.Name = group.Name;
            int groupValue = (int)Enum.Parse(typeof(GroupType), group.GroupType.ToString());
            viewModel.GroupTypeValue = groupValue;

            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach (int item in Enum.GetValues(typeof(GroupType)))
            {
                selectListItems.Add(
                    new SelectListItem()
                    {
                        Text = Enum.GetName(typeof(GroupType), item),
                        Value = item.ToString(),
                    }
                    );
            }

            viewModel.GroupTypeSelect =  new SelectList(selectListItems, "Value", "Text");
            viewModel.Id = group.Id;
            viewModel.Order = group.Order;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(GroupFormViewModel formData)
        {
            Group group = dbContext.Groups.Include(x=>x.Season).ThenInclude(x=>x.Competition).Where(x => x.Id == formData.Id).FirstOrDefault();
            if(group == null) { return new NotFoundResult(); } 
            group.Name = formData.Name;
            group.Order = formData.Order;
            group.GroupType = (GroupType)Enum.Parse(typeof(GroupType), formData.GroupTypeValue.ToString());
            string slug = String.Format(
                "{0}-{1}",
                group.Season.Slug,
                new Slugify.SlugHelper().GenerateSlug(formData.Name)
                );
            group.Slug = slug;
            if (dbContext.SaveChanges() > 0)
            {
                TempData["Message"] = "Successfully Saved";
            }
            else
            {
                TempData["Error"] = "Error";
            }
            return RedirectToAction("Edit", new { id = group.Id });
        }

        [HttpGet]
        public IActionResult ManageTeams(int? id)
        {
            if(id != null)
            {
                Group group = dbContext.Groups.Include(x=>x.Season).ThenInclude(x=>x.Competition).Where(x => x.Id == id).FirstOrDefault();
                if (group == null)
                {
                    return new NotFoundResult();
                }
                ViewData["Title"] = String.Format("Manage Teams of {0} of {1} ({2})",group.Name,group.Season.Competition.Name,group.Season.Name);
                GroupManageTeamsFormViewModel viewModel = new GroupManageTeamsFormViewModel();
                viewModel.GroupId = group.Id;
                viewModel.CurrentTeams = dbContext.GroupTeams.Include(x=>x.Team).Where(x => x.GroupId == id).Select(x => x.Team).ToList();
                viewModel.TeamsSelectList = new MultiSelectList(
                    dbContext.Teams.Where(x=>!viewModel.CurrentTeams.Contains(x)).ToList().OrderBy(x=>x.Name), "Id", "Name");
                viewModel.SeasonId = group.SeasonId;
                return View(viewModel);
            }
            return new NotFoundResult();

        }
        [HttpPost]
        public IActionResult ManageTeams(GroupManageTeamsFormViewModel formData)
        {
            if(formData.GroupId > 0 && formData.SelectedTeams.Count > 0)
            {
                foreach(var teamId in formData.SelectedTeams) 
                {
                    dbContext.Add(new GroupTeam() { GroupId = formData.GroupId, TeamId = teamId });
                }
                dbContext.SaveChanges();
            }
            return RedirectToAction("ManageTeams", new { id = formData.GroupId });
        }

        [HttpPost]
        public IActionResult RemoveTeam(int? id,int? teamId)
        {
            if(id == null || teamId==null)
            {
                TempData["Error"]="You must provide both group id and team id";
                return RedirectToAction("Index", "Home");

            }
            var matches = dbContext.MatchResults.Include(x => x.Match).Where(x => x.TeamId == teamId).Select(m => m.Match).ToList();
            if (matches.Where(x=>x.GroupId==id).Count() > 0)
            {
                TempData["Error"] ="You can't remove this team because it's linked to matches in this group.";
                return RedirectToAction("ManageTeams",new { id = id });
            }
            dbContext.GroupTeams.Remove(dbContext.GroupTeams.Where(x => x.TeamId == teamId).Where(x => x.GroupId == id).FirstOrDefault());
            if (dbContext.SaveChanges() > 0)
            {
                return RedirectToAction("ManageTeams", new { id = id });
            }
            return new BadRequestResult();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Group group = dbContext.Groups.Where(x => x.Id == id).FirstOrDefault();
            dbContext.Remove(group);
            dbContext.SaveChanges();
            return RedirectToAction("Overview", "Seasons", new { id = group.SeasonId });
        }
    }
}
