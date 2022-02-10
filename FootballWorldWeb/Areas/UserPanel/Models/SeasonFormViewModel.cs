using FootballWorld.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class SeasonFormViewModel
    {
        public SeasonFormViewModel()
        {
            StartYears = new SelectList(Enumerable.Range(1980,2030),this.StartYear);
            EndYears = new SelectList(Enumerable.Range(1980,2030),this.EndYear);
        }

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; } = new Competition();

        public int Id { get; set; }
        public string Name { get; set; }
        public SelectList StartYears { get; set; }
        public SelectList EndYears { get; set; }

        public int StartYear { get; set; } = DateTime.Now.Year;
        public int EndYear { get; set; } = (DateTime.Now.Year)+1;
    }
}
