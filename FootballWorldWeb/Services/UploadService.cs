using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FootballWorldWeb.Services
{
    public class UploadService
    {
        public static void Upload(IFormFile file)
        {
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            System.IO.File.WriteAllBytes(Directory.GetCurrentDirectory() + "/wwwroot/images/" + file.FileName, ms.ToArray());
        }
    }
}
