using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaManageApp.Models
{
    public class Statistic
    {
        public string departmentName { get; set; }
        public int totalIdea { get; set; }
        public float percentageIdea { get; set; }
        public int contributeNumber { get; set; }
        public int ideaNumberWithoutComment { get; set; }
        public int anonymousIdeaNumber { get; set; }
        public int anonymousCommentNumber { get; set; }
    }
}