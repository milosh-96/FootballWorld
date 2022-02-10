using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorld.Data
{
    public class Group
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Slug { get; set; }

        public GroupType GroupType { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }

        public List<GroupTeam> GroupTeams { get; set; }
        public List<Match> Matches { get; set; }
        public List<Standings> Standings { get; set; }
    }

    public enum GroupType
    {
        League, Knockout
    }
}
