using IdeaManageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdeaManageApp.Controllers
{
    public class LoginController : Controller
    {
        IdeaModel db = new IdeaModel();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                using (IdeaModel db = new IdeaModel())
                {
                    var obj = db.Users.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password)).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["User_Id"] = obj.User_Id.ToString();
                        Session["Email"] = obj.Email.ToString();
                        return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        ModelState.AddModelError("", "You are input the wrong email or password");
                    }
                }
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
