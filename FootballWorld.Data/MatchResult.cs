using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorld.Data
{
    public class MatchResult
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }
        public int Score { get; set; }

        public MatchResultType Type { get; set; } = MatchResultType.HomeTeam;
    }

    public enum MatchResultType
    {
        HomeTeam,AwayTeam

    }

}
