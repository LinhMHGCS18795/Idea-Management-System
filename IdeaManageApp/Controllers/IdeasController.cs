using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdeaManageApp.Models;

namespace IdeaManageApp.Controllers
{    
    public class IdeasController : Controller
    {
        private EmailServiceController emailServiceController;
        private AppModel db = new AppModel();

        // GET: Ideas
        public ActionResult Index()
        {
            var ideas = db.Ideas.Include(i => i.Category).Include(i => i.User);
            return View(ideas.ToList());
        }

        //POST: UPLOAD FILE
        [HttpPost]
        public ActionResult Create(Idea idea)
        {
            string fileName = Path.GetFileNameWithoutExtension(idea.MyFile.FileName);
            string extension = Path.GetExtension(idea.MyFile.FileName);
            fileName = fileName + extension;
            idea.Idea_File_path = fileName;
            fileName = Path.Combine(Server.MapPath("../Files/"), fileName);
            idea.MyFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Ideas.Add(idea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(idea);
        }


        // GET: Ideas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // GET: Ideas/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name");
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name");
            return View();
        }     
        

        // GET: Ideas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name", idea.Category_Id);
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", idea.User_Id);
            return View(idea);
        }

        // POST: Ideas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Idea_Id,Category_Id,Idea_Title,Idea_Content,Idea_Create_date,Idea_File_path,User_Id,TermsAndCondition")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(idea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name", idea.Category_Id);
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", idea.User_Id);
            return View(idea);
        }

        // GET: Ideas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Idea idea = db.Ideas.Find(id);
            db.Ideas.Remove(idea);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected Boolean CheckTimeIdea(int? cateID)
        {
            Category cate = db.Categories.Find(cateID);
            DateTime current = DateTime.Today;
            int ideaValid = DateTime.Compare(current, (DateTime)cate.Submission.Submission_Closure_date);

            if (ideaValid > 0)
            {
                /// out of date
                return false;
            }
            else
            {
                /// Create 
                return true;
            }
        }

        protected void CheckTimeComment(int IdeaId)
        {
            DateTime current = DateTime.Today;

            Idea currentIdea = db.Ideas.Find(IdeaId);

            if (currentIdea != null)
            {
                int commentValidate = DateTime.Compare(current, (DateTime)currentIdea.Category.Submission.Submission_Final_closure_date);
                if (commentValidate > 0)
                {
                    // notification out of date
                }
                else
                {
                    db.Entry(currentIdea).State = EntityState.Modified;
                    //return View(create comment);
                }
            }
            else
            {
                // notification this idea not exist
            }
        }

        protected void StatisticByDepartment(int departmentId)
        {
            if (departmentId != null && departmentId > 0)
            {
                var ideas = db.Ideas.Include(i => i.User);
                var users = db.Users.Include(u => u.Department);

                var query = from idea in ideas
                            join user in users on idea.User_Id equals user.User_Id
                            where user.Department_Id == departmentId
                            group user.Department by user.Department_Id into dt
                            select new
                            {
                                DepartmentName = dt.Key,
                                Count = dt.Count()
                            };

                //return View(query.ToList(););
            }
        }

        protected void StatisticByUser(int userId)
        {
            if (userId != null && userId > 0)
            {
                var ideas = db.Ideas.Include(i => i.User);
                var users = db.Users.Include(u => u.Department);

                var query = from idea in ideas
                            join user in users on idea.User_Id equals user.User_Id
                            where user.User_Id == userId
                            group user by user.User_Id into ud
                            select new
                            {
                                UserName = ud.Key,
                                Count = ud.Count()
                            };

                //return View(query.ToList());
            }
        }

        protected void StatisticByIdea(int ideaId)
        {
            var query = from i in db.Ideas
                        join u in db.Users on i.User_Id equals u.User_Id
                        join d in db.Departments on u.Department_Id equals d.Department_Id
                        select new
                        {
                            DepartmentId = d.Department_Id,
                            DepartmentName = d.Department_Name,
                            UserFullName = u.User_Name,
                            IdeaId = i.Idea_Id,
                        };
            // In progress
        }

        protected void SendNotification(Idea idea)
        {
            EmailModel notificationMail = new EmailModel();

            User qa = db.Users.Where(u => u.Role.Role_Name == "QACoordinator" && u.Department_Id == idea.User.Department_Id).FirstOrDefault();

            notificationMail.To = qa.Email;
            notificationMail.Subject = "New Idea has created!";
            notificationMail.Body = "User: " + idea.User.User_Name + " send new idea on category " + idea.Category.Category_Name;
            emailServiceController.FillEmailAndSend(notificationMail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
