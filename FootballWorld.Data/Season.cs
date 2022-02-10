using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorld.Data
{
    public class Season
    {
        public int Id { get; set; }
        [Column(TypeName="varchar(255)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Slug { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }

        public List<Group> Groups { get; set; }
    }
}
