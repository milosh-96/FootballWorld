using FootballWorld.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class CompetitionFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompetitionLogo { get; set; }
        public IFormFile LogoUploadFile { get; set; }
        public bool ReuploadLogo { get; set; } = false;

        public int SelectedCompetitionId { get; set; } = 0; 
        public SelectList ParentSelectList { get; set; } = new SelectList(new List<Competition>(),"Id","Name");
    }
}
