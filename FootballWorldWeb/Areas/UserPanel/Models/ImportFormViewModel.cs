using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class ImportFormViewModel
    {
        public IFormFile UploadedFile { get; set; }
        public int RelationshipId { get; set; }
    }
}
