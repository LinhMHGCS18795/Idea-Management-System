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
        private IdeasController IdeasController = new IdeasController();
        List<Statistic> statistics;
        // GET: Statistic
        public ActionResult Index()
        {
            StatisticByContributor();
            return View();
        }

        public void StatisticByContributor()
        {
            statistics = new List<Statistic>();
            statistics = IdeasController.StatisticByContribute();
            System.Diagnostics.Debug.WriteLine(">>>>>" + statistics.First().departmentName);

            System.Diagnostics.Debug.WriteLine(">>>>>2" + statistics.First().contributeNumber);
        }
    }

    
}