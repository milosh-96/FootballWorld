using FootballWorld.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class GroupManageTeamsFormViewModel
    {
        public int GroupId { get; set; }
        public List<Team> CurrentTeams { get; set; } = new List<Team>();
        public int SeasonId { get; set; }

        public List<int> SelectedTeams { get; set; } = new List<int>();
        public MultiSelectList TeamsSelectList { get; set; } = new MultiSelectList(new List<Team>(), "Id", "Name");
    }
}
