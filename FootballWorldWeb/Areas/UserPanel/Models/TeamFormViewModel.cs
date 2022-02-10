using FootballWorld.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class TeamFormViewModel
    {
        public TeamFormViewModel()
        {
            List<SelectListItem> items = Enum.GetValues(typeof(TeamType)).Cast<TeamType>().Select(x => new SelectListItem()
            {
                Text = x.ToString(),
                Value = ((int)x).ToString(),
                Selected = (SelectedTeamType==x) ? true : false 
            }).ToList();
            this.TeamTypeSelectList = new SelectList(items, "Value", "Text");
           
        }

        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string TeamLogo { get; set; }

        public IFormFile LogoUploadFile { get; set; }

        public bool ReuploadLogo { get; set; } = false;

        public SelectList TeamTypeSelectList { get; set; }

        public TeamType SelectedTeamType { get; set; }
    }
}
