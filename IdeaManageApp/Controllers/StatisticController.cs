using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdeaManageApp.Models;

namespace IdeaManageApp.Controllers
{
    public class StatisticController : Controller
    {
        private AppModel db = new AppModel();
        private IdeasController IdeasController = new IdeasController();
        List<Statistic> statistics = new List<Statistic>();
        // GET: Statistic
        public ActionResult Index()
        {
            ViewData["Role"] = Session["Role"];
            if (IdeasController.Test() != null)
            {
                return View(IdeasController.Test());
            }
            return View();
        }

        //public void StatisticByContributor()
        //{
        //    var ideas = db.Ideas.ToList();
        //    var users = db.Users.ToList();
        //    var departments = db.Departments.ToList();

        //    var query = from idea in ideas
        //                join user in users on idea.User_Id equals user.User_Id
        //                join department in departments on user.Department_Id equals department.Department_Id
        //                group user.Department by user.Department_Id into dt
        //                select new
        //                {
        //                    DepartmentId = dt.Key,
        //                    Count = dt.Distinct().Count()
        //                };
        //    System.Diagnostics.Debug.WriteLine(">>>query: " + query.Count());

        //    var listUserContribute = db.Ideas.Include(i => i.User).Include(u => u.User.Department).
        //        Select(x => new { Department_Name = x.User.Department.Department_Name, User_Id = x.User.User_Id }).ToList();
        //    System.Diagnostics.Debug.WriteLine(">>>listUserContribute: " + listUserContribute.Count());

        //    var listContribute = db.Ideas.Include(i => i.User).Include(u => u.User.Department).
        //        Where(x => x.User.Ideas != null).
        //        GroupBy(g => g.User.Department_Id).
        //        Select(x => new { id = x.Key, contributeNumber = x.Count() }).
        //        Distinct().ToList();
        //    System.Diagnostics.Debug.WriteLine(">>>listContribute: " + listContribute.Count());
        //}


    }


}