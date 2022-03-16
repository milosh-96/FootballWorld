using FluentFTP;
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
        private string host;
        private string userName;
        private string password;
        private string remoteDir;

      

        public static void Upload(IFormFile file)
        {
            var uploader = new ImageUpload.FileUploader();
            uploader.Initialize("allsve.com", "webview", "adriano10inter", "/allsve.com/cdn/football/images");
            uploader.Upload(file);

        }
        public static void Upload(IFormFile file,string subfolder)
        {
            var uploader = new ImageUpload.FileUploader();
            uploader.Initialize("allsve.com", "webview", "adriano10inter", "/allsve.com/cdn/football/images/"+subfolder);
            uploader.Upload(file);

        }
    }
}
