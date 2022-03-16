using FootballWorld.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Models.Competitions
{
    public class SeasonViewViewModel
    {
        public int Id { get; set; }

        public Season Season { get; set; }
        public List<Group> Groups { get; set; }
        public List<Group> Lists { get; set; }

        public Competition Competition { get; set; }

        public bool SingleGroup { get; set; } = false;
    }
}
