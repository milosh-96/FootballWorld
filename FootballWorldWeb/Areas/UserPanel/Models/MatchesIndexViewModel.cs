using FootballWorld.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class MatchesIndexViewModel
    {
        public int GroupId { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
    }
}
