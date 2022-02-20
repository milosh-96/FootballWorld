using FootballWorld.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class GroupFormViewModel
    {
        public GroupFormViewModel()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            foreach(int item in Enum.GetValues(typeof(GroupType)))
            {
                selectListItems.Add(
                    new SelectListItem() { Text = Enum.GetName(typeof(GroupType), item),Value=item.ToString() }
                    );
            }
            this.GroupTypeSelect = new SelectList(selectListItems, "Value", "Text", null);
        }

        public int Id { get; set; }
        public string Name { get; set; } = "Group";
        public int SeasonId { get; set; }
        public int GroupTypeValue { get; set; } = 0;
        public SelectList GroupTypeSelect { get; set; }
        public int Order { get; set; } = 0;

    }
}
