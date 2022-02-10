using FootballWorld.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Models.Competitions
{
    public class CompetitionOverviewViewModel
    {
        public List<Competition> Children { get; set; } = new List<Competition>();
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Logo { get; set; } = "";
        public string Slug { get; set; } = "";
        public List<Season> Seasons { get; set; } = new List<Season>();
    }
}
