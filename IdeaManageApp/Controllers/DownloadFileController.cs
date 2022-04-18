using IdeaManageApp.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdeaManageApp.Controllers
{
    public class DownloadFileController : Controller
    {
        // GET: DownloadFile
        public ActionResult Index()
        {
            ViewData["Role"] = Session["Role"];
            string[] filePaths = Directory.GetFiles(Server.MapPath("../Files/"));
            List<DownloadFile> files = new List<DownloadFile>();
            foreach (string filePath in filePaths)
            {
                files.Add(new DownloadFile()
                {
                    FileName = Path.GetFileName(filePath),
                    Idea_File_path = filePath
                });
            }

            return View(files);
        }

        [HttpPost]
        public ActionResult Index(List<DownloadFile> files)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");
                foreach (DownloadFile file in files)
                {
                    if (file.IsSelected)
                    {
                        zip.AddFile(file.Idea_File_path, "Files");
                    }
                }
                string zipName = String.Format("FilesZip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd"));

                using (MemoryStream memstrem = new MemoryStream())
                {
                    zip.Save(memstrem);
                    return File(memstrem.ToArray(), "application/zip", zipName);
                }
            }
        }
    }
}