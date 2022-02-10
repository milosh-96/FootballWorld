using FootballWorld.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Models
{
    public class IndexViewModel
    {
        public List<Competition> Competitions { get; set; } = new List<Competition>();

    }
}
