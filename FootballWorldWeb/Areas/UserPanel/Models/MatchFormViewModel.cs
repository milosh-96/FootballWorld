using FootballWorld.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class MatchFormViewModel
    {
        public MatchFormViewModel()
        {
        }

        public int Id { get; set; }
        public DateTime Start { get; set; }
        public bool Finished { get; set; } = false;
        public SelectList HomeTeamSelect { get; set; } = new SelectList(new List<Team>());
        public SelectList AwayTeamSelect { get; set; } = new SelectList(new List<Team>());
        public MatchResult HomeResult { get; set; } = new MatchResult() { Type = MatchResultType.HomeTeam };
        public MatchResult AwayResult { get; set; } = new MatchResult() { Type = MatchResultType.AwayTeam };

        public string Comments { get; set; } = "";

        public int GroupId { get; set; }
    }
}
