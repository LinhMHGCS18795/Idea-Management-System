using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdeaManageApp.Models;

namespace IdeaManageApp.Controllers
{
    public class CommentsController : Controller
    {
        private AppModel db = new AppModel();
        private EmailServiceController emailServiceController = new EmailServiceController();
        // GET: Comments
        public ActionResult Index(Idea Idea)
        {
            ViewData["Role"] = Session["Role"];
            if (Idea.Idea_Id != 0 )
            {
                ViewBag.idea_Id = Idea.Idea_Id;
                return View(db.Comments.Include(c => c.Idea).Include(c => c.User).Where(x => x.Idea_Id == Idea.Idea_Id).ToList());
            }
            else
            {
                return View(db.Comments.Include(c => c.Idea).Include(c => c.User).ToList());
            }
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(int? Id)
        {
            ViewBag.idea_Id = Id;
            ViewBag.id = Convert.ToInt32(Session["User_Id"]);
            Comment c = new Comment();
            c.Idea = db.Ideas.Find(Id);
            return View(c);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Comment_Id,Content,Created_date,User_Id,Idea_Id")] Comment comment)
        {
            if (ModelState.IsValid && CheckTimeComment((int)comment.Idea_Id, (DateTime)comment.Created_date))
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                ViewBag.idea_Id = comment.Idea_Id;
                SendNotification(comment);
                return RedirectToAction("Index", db.Ideas.Find(comment.Idea_Id));
            } else
            {
                if (!CheckTimeComment(((int)comment.Idea_Id), (DateTime)comment.Created_date))
                {
                    Response.Write("Your comment is overdue");
                }
            }

            ViewBag.idea_Id = comment.Idea_Id;
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Comment_Id,Content,Created_date,User_Id,Idea_Id")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void SendNotification(Comment comment)
        {

            var idea = db.Ideas.Include(u => u.User).Where(x => x.Idea_Id == comment.Idea_Id).First();
            if(idea.User_Id != 0 && comment.User_Id != 0 && idea.Idea_Id != 0)
            {
                var userReceive = db.Users.Find(idea.User_Id);
                if(userReceive.Email != null)
                {
                    EmailModel notificationMail = new EmailModel();
                    notificationMail.To = userReceive.Email;
                    notificationMail.Subject = "Your Idea has new comment!";
                    notificationMail.Body = "Idea: " + idea.Idea_Title + " has new comment!";
                    emailServiceController.FillEmailAndSend(notificationMail);
                }
                
            }
        }

        public Boolean CheckTimeComment(int IdeaId, DateTime date)
        {
            Idea currentIdea = db.Ideas.Find(IdeaId);

            if (currentIdea != null)
            {
                int commentValidate = DateTime.Compare(date, (DateTime)currentIdea.Category.Submission.Submission_Final_closure_date);
                //System.Diagnostics.Debug.WriteLine("<<<<<<<1" + date + " ----------2 " + currentIdea.Category.Submission.Submission_Final_closure_date + " ============> " + commentValidate);
                if (commentValidate > 0)
                {
                    // notification out of date
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
