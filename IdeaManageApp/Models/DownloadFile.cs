using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaManageApp.Models
{
    public class DownloadFile
    {
        public string FileName { get; set; }
        public string Idea_File_path { get; set; }
        public bool IsSelected { get; set; }
    }
}