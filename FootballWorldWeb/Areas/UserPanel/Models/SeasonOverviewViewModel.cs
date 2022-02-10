using FootballWorld.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class SeasonOverviewViewModel
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public Season Season { get; set; } = new Season();
        public List<Team> Teams { get; set; } = new List<Team>();
        public List<Match> Matches { get; set; } = new List<Match>();
    }
}
