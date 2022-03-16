using CsvHelper;
using CsvHelper.Configuration;
using FootballWorld.Data;
using FootballWorldWeb.Areas.UserPanel.Models;
using FootballWorldWeb.Data;
using FootballWorldWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class MatchesController : Controller
    {
        private readonly FootballDbContext dbContext;
        private readonly StandingsCalculatorService standingsCalculatorService;
        public MatchesController(FootballDbContext dbContext, StandingsCalculatorService standingsCalculatorService)
        {
            this.dbContext = dbContext;
            this.standingsCalculatorService = standingsCalculatorService;
        }

        public IActionResult Index(int groupId = 0)
        {
            if (groupId < 1)
            {
                TempData["Error"] = "Please provide ID greater than 0";
                return RedirectToAction("Index", "Home");
            }
            MatchesIndexViewModel viewModel = new MatchesIndexViewModel();
            Group group = dbContext.Groups.Include(x => x.Season).ThenInclude(x => x.Competition).Where(x => x.Id == groupId).FirstOrDefault();
            if (group == null) { return new ContentResult() { StatusCode = 404, Content = "Group with ID #" + groupId + " wasn't found." }; }
            ViewData["Title"] = String.Format("List of Matches for {0} of {1} ({2})",
                group.Name,
                group.Season.Competition.Name,
                group.Season.Name);
            IQueryable<Match> query;

            query = dbContext.Matches.Where(x => x.GroupId == groupId).Include(x => x.Results).ThenInclude(x => x.Team).OrderBy(x => x.Start);

            List<Match> matches = query.ToList();
            viewModel.Matches = matches;
            viewModel.GroupId = groupId;
            viewModel.SeasonId = group.SeasonId;
            return View(viewModel);
        }

        public IActionResult Create(int groupId = 0)
        {
            ViewData["Title"] = "Create a Match";
            MatchFormViewModel viewModel = new MatchFormViewModel();

            Group group = dbContext.Groups.Where(x => x.Id == groupId).Include(x => x.Season).FirstOrDefault();
            if (group == null) { return new NotFoundResult(); }
            viewModel.HomeTeamSelect = new SelectList(
            dbContext.GroupTeams.Where(x => x.GroupId == groupId).Select(x => x.Team).OrderBy(x => x.Name).ToList(),
            "Id", "Name");
            viewModel.AwayTeamSelect = new SelectList(
                dbContext.GroupTeams.Where(x => x.GroupId == groupId).Select(x => x.Team).OrderBy(x => x.Name).ToList()
                , "Id", "Name");

            viewModel.GroupId = group.Id;
            viewModel.Start = new DateTime(group.Season.StartYear, 9, 1, 20, 00, 00);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(MatchFormViewModel formData)
        {
            if (formData.HomeResult.Team.Id == formData.AwayResult.Team.Id)
            {
                TempData["Error"] = "Home and Away team can't be the same.";
                return RedirectToAction("Create", new { groupId = formData.GroupId });
            }
            Match match = new Match()
            {
                Start = formData.Start,
                Finished = formData.Finished,
                GroupId = formData.GroupId,
            };
            if (formData.Comments != null)
            {
                match.Comments = formData.Comments.Trim();
            }
            dbContext.Add(match);
            dbContext.SaveChanges();

            MatchResult homeResult = new MatchResult()
            {
                Score = formData.HomeResult.Score,
                TeamId = formData.HomeResult.Team.Id,
                Type = MatchResultType.HomeTeam,
                MatchId = match.Id
            };
            MatchResult awayResult = new MatchResult()
            {
                Score = formData.AwayResult.Score,
                TeamId = formData.AwayResult.Team.Id,
                Type = MatchResultType.AwayTeam,
                MatchId = match.Id
            };
            List<MatchResult> results = new List<MatchResult>() { homeResult, awayResult };
            dbContext.MatchResults.AddRange(results);
            dbContext.SaveChanges();
            standingsCalculatorService.UpdateDbStandings(formData.GroupId);
            return RedirectToAction("Edit", new { id = match.Id, groupId = match.GroupId });
        }


        [HttpGet]
        public IActionResult Edit(int id, int? groupId)
        {
            if (id < 0 & groupId == null)
            {
                TempData["Error"] = "You must provide both match Id and group Id";
                return RedirectToAction("Index", "Home");
            }
            ViewData["Title"] = "Edit Match";

            Match match = dbContext.Matches
                .Include(x => x.Results)
                .ThenInclude(x => x.Team)
                .Where(x => x.Id == id)
                .Where(x => x.GroupId == groupId).FirstOrDefault();

            if (match == null) { return new NotFoundResult(); }
            MatchFormViewModel viewModel = new MatchFormViewModel();

            viewModel.Id = match.Id;
            viewModel.Start = match.Start;
            viewModel.Finished = match.Finished;
            viewModel.GroupId = match.GroupId;
            viewModel.Comments = match.Comments;
            viewModel.HomeResult = match.Results.Where(x => x.Type == MatchResultType.HomeTeam).FirstOrDefault();
            viewModel.AwayResult = match.Results.Where(x => x.Type == MatchResultType.AwayTeam).FirstOrDefault();

            viewModel.HomeTeamSelect = new SelectList(
                dbContext.GroupTeams.Include(x => x.Team).Where(x => x.GroupId == groupId).Select(x => x.Team).OrderBy(x => x.Name).ToList(),
                "Id", "Name", viewModel.HomeResult.Team.Id);
            viewModel.AwayTeamSelect = new SelectList(
                dbContext.GroupTeams.Include(x => x.Team).Where(x => x.GroupId == groupId).Select(x => x.Team).OrderBy(x => x.Name).ToList(), "Id", "Name", viewModel.AwayResult.Team.Id);

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(int id, MatchFormViewModel formData)
        {
            Match match = dbContext.Matches.Where(x => x.Id == id).FirstOrDefault();
            if (match == null)
            {
                return new NotFoundResult();
            }
            MatchResult homeResult = dbContext.MatchResults
                .Where(x => x.Match == match)
                .Where(x => x.Type == MatchResultType.HomeTeam).FirstOrDefault();
            MatchResult awayResult = dbContext.MatchResults
                .Where(x => x.Match == match)
                .Where(x => x.Type == MatchResultType.AwayTeam).FirstOrDefault();

            match.Finished = formData.Finished;
            match.Start = formData.Start;

            homeResult.Score = formData.HomeResult.Score;
            homeResult.TeamId = formData.HomeResult.Team.Id;

            awayResult.Score = formData.AwayResult.Score;
            awayResult.TeamId = formData.AwayResult.Team.Id;

            if (formData.Comments != null)
            {
                match.Comments = formData.Comments.Trim();
            }
            dbContext.SaveChanges();

            standingsCalculatorService.UpdateDbStandings(match.GroupId);

            return RedirectToAction("Edit", new { id = id, groupId = match.GroupId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id < 1) { return new BadRequestResult(); }
            Match match = dbContext.Matches.Where(x => x.Id == id).FirstOrDefault();
            if (match == null)
            {
                return new NotFoundResult();
            }

            dbContext.Matches.Remove(match);
            dbContext.SaveChanges();
            standingsCalculatorService.UpdateDbStandings(match.GroupId);
            return RedirectToAction("Index", new { groupId = match.GroupId });
        }


        public IActionResult Import(int groupId)
        {
            ViewData["Title"] = "Import Matches using a file";
            ImportFormViewModel viewModel = new ImportFormViewModel();
            viewModel.RelationshipId = groupId;
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult ImportPreview(ImportFormViewModel formData)
        {
            ViewData["Title"] = "Preview of imported file";
            ImportPreviewViewModel<Match> viewModel = new ImportPreviewViewModel<Match>();
            viewModel.RelationshipId = formData.RelationshipId;
            //parse file and pass it to viewmodel//
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, IgnoreBlankLines = true };
            using (var reader = new StreamReader(formData.UploadedFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                var records = csv.GetRecords<CsvMatch>();
                foreach (CsvMatch csvMatch in records)
                {
                    var filledRow = csvMatch.GetType().GetProperties()
                            .All(p => p.GetValue(csvMatch) != null);
                    
                    if(!filledRow) { continue; }
                    Team homeTeam = dbContext.Teams.Where(x => x.Name == csvMatch.HomeTeam).FirstOrDefault();
                    Team awayTeam = dbContext.Teams.Where(x => x.Name == csvMatch.AwayTeam).FirstOrDefault();
                    if (homeTeam == null || awayTeam == null) { continue; } // if either team wasn't found skip csv row
                    if(!dbContext.GroupTeams.Any(x=>x.TeamId==homeTeam.Id && x.GroupId==viewModel.RelationshipId)
                    || !dbContext.GroupTeams.Any(x => x.TeamId == awayTeam.Id && x.GroupId == viewModel.RelationshipId)) {
                        continue; } // check if teams are assigned to correct group
                    MatchResult homeResult = new MatchResult()
                    {
                        Type = MatchResultType.HomeTeam,
                        Score = csvMatch.HomeScore.Value,
                        TeamId = homeTeam.Id
                    };
                    MatchResult awayResult = new MatchResult()
                    {
                        Type = MatchResultType.AwayTeam,
                        Score = csvMatch.AwayScore.Value,
                        TeamId = awayTeam.Id
                    };
                    Match match = new Match()
                    {
                        Finished = csvMatch.Finished.Value,
                        Start = csvMatch.MatchStart.Value,
                        Results = new List<MatchResult>() { homeResult, awayResult }
                    };
                    if (viewModel.RelationshipId > 0)
                    {
                        if (dbContext.Groups.Any(x => x.Id == viewModel.RelationshipId))
                        {
                            match.GroupId = viewModel.RelationshipId;
                        }
                    }
                    viewModel.Items.Add(match);
                    dbContext.Matches.Add(match);
                    dbContext.SaveChanges();
                }

            }
            standingsCalculatorService.UpdateDbStandings(viewModel.RelationshipId);
            return View(viewModel);
        }
    }

}