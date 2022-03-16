using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb
{
    public class LinkHandler
    {
        public static string CdnBase { get; set; } = "https://cdn.allsve.com";
        public static string FootballCdnBase { get; set; } = LinkHandler.CdnBase + "/football";

        public static string MakeImageLink(string imagePath, string subFolderPath)
        {
            return String.Format("{0}/images/{1}/{2}",
                LinkHandler.CdnBase, subFolderPath, imagePath);
        }
        public static string MakeFootballImageLink(string imagePath,string subFolderPath)
        {
            return String.Format("{0}/images/{1}/{2}",
                
                LinkHandler.FootballCdnBase, subFolderPath, imagePath);
        }
    }
}
