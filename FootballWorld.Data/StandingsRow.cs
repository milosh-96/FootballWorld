using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorld.Data
{
    public class StandingsRow
    {
        public int Id { get; set; }
        public int StandingsId { get; set; }
        public Standings Standings { get; set; }
        public Team Team { get; set; }
        public int TeamId { get; set; }
        public int Points { get; set; } = 0;
        public int Played { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int GoalsScored { get; set; } = 0;
        public int GoalsConceded { get; set; } = 0;
        public int HomeWins { get; set; } = 0;
        public int HomeGoals { get; set; } = 0;
        public int AwayWins { get; set; } = 0;
        public int AwayGoals { get; set; } = 0;

    }
}
