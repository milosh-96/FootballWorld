using FootballWorld.Data;
using FootballWorldWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Services
{
    public class StandingsCalculatorService
    {
        private readonly FootballDbContext dbContext;

        public StandingsCalculatorService(FootballDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public bool UpdateDbStandings(int groupId)
        {
            Group group = dbContext.Groups.Where(x => x.Id == groupId).Include(x => x.Standings).FirstOrDefault();
            if (group == null) { return false; }
            if(group.GroupType == GroupType.Knockout ) { return false; }
            Standings standings = new Standings();

            if (group.Standings.Count() < 1)
            {
                standings.GroupId = groupId;
                dbContext.Standings.Add(standings);
                dbContext.SaveChanges();
            }
            else
            {
                standings = group.Standings.FirstOrDefault();
            }
            List<Team> teams = this.SelectTeams(groupId);
            dbContext.StandingRows.AddRange(this.Calculate(teams, standings.Id));
            if (dbContext.SaveChanges() > 0) { return true; }
            return false;

        }


        public Standings GetStandings(int groupId = 0)
        {
            Standings standings = new Standings();
            standings.Items = Calculate(SelectTeams(groupId));
            return standings;
        }
        public List<Team> SelectTeams(int groupId)
        {
                       return     dbContext.GroupTeams
                            .Where(x => x.GroupId == groupId)
                            .Include(x => x.Team)
                            .Select(x => x.Team)
                            .ToList();
        }

        public List<StandingsRow> Calculate(List<Team> teams,int standingsId = 0)
        {
            List<StandingsRow> standingsRows = new List<StandingsRow>();

            Standings standings = dbContext.Standings.Where(x => x.Id == standingsId).FirstOrDefault();
            foreach (Team team in teams)
            {
                bool update = true;
                var row = dbContext.StandingRows.Where(x => x.TeamId == team.Id && x.StandingsId == standingsId).FirstOrDefault();
                if (row == null)
                {
                    row = new StandingsRow() { Team = team };
                    update = false;
                }
                // get match result with associated match for current team in the loop
                var results = dbContext.MatchResults
                    .Include(x => x.Match)
                    .ThenInclude(x => x.Results)
                    .Where(x => x.Team == team).ToList();

                if(update == true)
                {
                    //reset stats so it doesn't increment old values
                    row.AwayGoals = 0;
                    row.AwayWins = 0;
                    row.Draws = 0;
                    row.GoalsConceded = 0;
                    row.GoalsScored = 0;
                    row.HomeGoals = 0;
                    row.HomeWins = 0;
                    row.Losses = 0;
                    row.Played = 0;
                    row.Points = 0;
                    row.Wins = 0;
                    

                }

                foreach (var result in results.Where(x => x.Match.Finished == true).Where(x=>x.Match.GroupId==standings.GroupId).ToList())
                {
                    // query each matchresult from match of current matchresult //
                    foreach (var resultMatch in result.Match.Results)
                    {
                        // ensure the queried matchresult isn't same as parent mathresult, so you don't compare values of
                        // the same item (basically, resultMatch is an opponent team's result)//
                        if (resultMatch != result)
                        {
                            row.Played += 1;
                            // if current team won //
                            if (result.Score > resultMatch.Score)
                            {
                                if (result.Type == MatchResultType.HomeTeam)
                                {
                                    row.HomeWins += 1;
                                    row.HomeGoals += result.Score;
                                }
                                else
                                {
                                    row.AwayWins += 1;
                                    row.AwayGoals += result.Score;
                                }
                                row.Points += 3;
                                row.Wins += 1;
                            }
                            // if current team drew //
                            else if (result.Score == resultMatch.Score)
                            {
                                row.Points += 1;
                                row.Draws += 1;
                            }
                            // if current team lost //
                            else if (result.Score < resultMatch.Score)
                            {
                                row.Losses += 1;
                            }
                        }
                        // if resultMatch's team isn't current, so it's an oponnent
                        if (resultMatch.Team != team)
                        {
                            row.GoalsConceded += resultMatch.Score;
                        }
                        else
                        {
                            row.GoalsScored += resultMatch.Score;
                        }
                    }
                }
                if (standingsId > 0)
                {
                    row.StandingsId = standingsId;
                }
                if (update)
                {
                    dbContext.SaveChanges();
                }
                else
                {
                    standingsRows.Add(row);
                }
            }
            return standingsRows;
        }

        
    }
}
