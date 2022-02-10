using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorld.Data
{
    public class GroupTeam
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
