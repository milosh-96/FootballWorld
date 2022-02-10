using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorld.Data
{
    public class Competition
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }

         [Column(TypeName = "varchar(255)")]
        public string Slug { get; set; }


        [Column(TypeName = "varchar(255)")]
        public string CompetitionLogo { get; set; }

        
        public int? ParentId { get; set; }
        public Competition Parent { get; set; }
        public List<Competition> Children { get; set; }
        public List<Season> Seasons { get; set; }

    }
}
