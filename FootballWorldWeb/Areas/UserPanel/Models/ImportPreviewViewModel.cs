using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldWeb.Areas.UserPanel.Models
{
    public class ImportPreviewViewModel<T> 
    {
        public List<T> Items { get; set; } = new List<T>();
    }
}
