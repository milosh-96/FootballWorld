using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballWorld.Data
{
    public class Team
    {
        public int Id { get; set; }
       [Column(TypeName="varchar(255)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Slug { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string TeamLogo { get; set; }

        public TeamType TeamType { get; set; } = TeamType.Club;

    }

    public enum TeamType
    {
        Club,NationalTeam
    }

    public class CsvTeam
    {
        public string Name { get; set; }
        public string TeamLogo { get; set; } = "";
        public string TeamType { get; set; } = "0";
    }
}
