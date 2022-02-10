using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballWorld.Data
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        
        public string Comments { get; set; }

        public List<MatchResult> Results { get; set; } = new List<MatchResult>();
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public bool Finished { get; set; } = false;

        public MatchResult GetHomeResult()
        {
            return this.Results
       .Where(x => x.Type == FootballWorld.Data.MatchResultType.HomeTeam).FirstOrDefault();
           
        } 
        public MatchResult GetAwayResult()
        {
            return this.Results
       .Where(x => x.Type == FootballWorld.Data.MatchResultType.AwayTeam).FirstOrDefault();
           
        }

    }
}
