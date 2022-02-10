using FootballWorld.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class CompetitionOverviewViewModel
    {
        public int Id { get; set; }
        public Competition Competition { get; set; }
    }
}
