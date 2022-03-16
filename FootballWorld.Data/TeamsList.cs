using System;
using System.Collections.Generic;
using System.Text;

namespace FootballWorld.Data
{
    public class TeamsList
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public List<Team> Items { get; set; } = new List<Team>();
    }
}
