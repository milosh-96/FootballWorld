using FootballWorld.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Models.Groups
{
    public class GroupViewViewModel
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public Standings Standings { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();


    }
}
