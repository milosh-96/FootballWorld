using FootballWorld.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class TeamsIndexViewModel
    {
        public string FilterBy { get; set; } = null;

        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
