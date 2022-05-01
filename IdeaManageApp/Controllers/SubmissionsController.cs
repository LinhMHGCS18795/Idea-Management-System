using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdeaManageApp.Models;
using PagedList;
using StackExchange.Redis;

namespace IdeaManageApp.Controllers
{
    public class SubmissionsController : Controller
    {
        private AppModel db = new AppModel();
        private IdeasController IdeasController = new IdeasController();
        private ReactionsController reactionsController = new ReactionsController();
        private CommentsController commentsController = new CommentsController();

        // GET: Submissions


        public ActionResult Index(int? page)
        {
            ViewData["Role"] = Session["Role"];

            //PageList
            if (page == null) page = 1;            
            var submission = (from l in db.Submissions select l).OrderBy(x => x.Submission_Id);                       
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            
            return View(submission.ToPagedList(pageNumber, pageSize));
        }



        // GET: Submissions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        public ActionResult StaffDetail(int? id ,int? page)
        {
            ViewData["Role"] = Session["Role"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Page List
            if (page == null) page = 1;
            var idea = (from l in db.Ideas select l).OrderBy(x => x.Idea_Id);
            int pageSize = 5;
            int pageNumber = (page ?? 1);


            loadPopular();                       

            var ideas = db.Ideas.Include(i => i.Category).Include(s => s.Category.Submission).Include(v => v.Views).
                Where(x => x.Category.Submission_Id == id).OrderBy(x=> x.Idea_Id);
            
            return View(ideas.ToPagedList(pageNumber, pageSize));

        }

        // GET: Submissions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Submissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Submission_Id,Submission_Name,Submission_Description,Submission_Closure_date,Submission_Final_closure_date")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Submissions.Add(submission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(submission);
        }

        // GET: Submissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Submission_Id,Submission_Name,Submission_Description,Submission_Closure_date,Submission_Final_closure_date")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(submission);
        }

        // GET: Submissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Submission submission = db.Submissions.Find(id);
            db.Submissions.Remove(submission);
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

        public void loadPopular()
        {
            Idea popularIdea = IdeasController.PopularIdea();
            if (popularIdea.Idea_Id != 0)
            {
                ViewData["popularIdeaCateName"] = popularIdea.Category.Category_Name;
                ViewData["popularIdeaTitle"] = popularIdea.Idea_Title;
                ViewData["popularIdeaContent"] = popularIdea.Idea_Content;
                ViewData["popularIdeaCreateDate"] = popularIdea.Idea_Create_date;
            }
            Idea mostViewIdea = IdeasController.MostViewIdea();
            if (popularIdea.Idea_Id != 0)
            {
                ViewData["mostViewIdeaCateName"] = mostViewIdea.Category.Category_Name;
                ViewData["mostViewIdeaTitle"] = mostViewIdea.Idea_Title;
                ViewData["mostViewIdeaContent"] = mostViewIdea.Idea_Content;
                ViewData["mostViewIdeaCreateDate"] = mostViewIdea.Idea_Create_date;
            }
            Idea latestComment = IdeasController.LatestComment();
            if (latestComment.Idea_Id != 0)
            {
                ViewData["latestCommentCateName"] = latestComment.Category.Category_Name;
                ViewData["latestCommentTitle"] = latestComment.Idea_Title;
                ViewData["latestCommentContent"] = latestComment.Idea_Content;
                ViewData["latestCommentCreateDate"] = latestComment.Idea_Create_date;
            }
            Idea latestIdea = IdeasController.LatestIdea();
            if (latestIdea.Idea_Id != 0)
            {
                ViewData["latestIdeaCateName"] = latestIdea.Category.Category_Name;
                ViewData["latestIdeaTitle"] = latestIdea.Idea_Title;
                ViewData["latestIdeaContent"] = latestIdea.Idea_Content;
                ViewData["latestIdeaCreateDate"] = latestIdea.Idea_Create_date;
            }
        }

        public ActionResult ideaDetails(int? id)
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

            View view = db.Views.Include(v => v.Idea).Where(v => v.Idea_Id == id).FirstOrDefault();
            if(view != null)
            {
                view.Total_View++;
                db.Entry(view).State = EntityState.Modified;
                db.SaveChanges();
            }

            int rId = reactionsController.findReaction(idea.User_Id, idea.Idea_Id);
            if (rId != 0)
            {
                ViewData["Reaction"] = db.Reactions.Find(rId).is_like;
            }
            
            return View(idea);
        }


        public void Reaction(Idea idea)
        {
            if (idea.User_Id != null && idea.Idea_Id != null)
            {
                int reactId = reactionsController.findReaction(idea.User_Id, idea.Idea_Id);
                reactionsController.Reaction(reactId, idea);
            }
        }

        

    }
}
