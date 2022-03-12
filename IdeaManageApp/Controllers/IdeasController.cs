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
    public class IdeasController : Controller
    {
        private IdeaModel db = new IdeaModel();

        // GET: Ideas
        public ActionResult Index()
        {
            var ideas = db.Ideas.Include(i => i.Category).Include(i => i.Submission).Include(i => i.User);
            return View(ideas.ToList());
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
            ViewBag.Submission_Id = new SelectList(db.Submissions, "Submission_Id", "Submission_Name");
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name");
            return View();
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Idea_Id,Idea_Title,Idea_Description,Idea_Content,Idea_Create_date,Idea_File_path,User_Id,Category_Id,Submission_Id,Idea_IsDisable")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Ideas.Add(idea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name", idea.Category_Id);
            ViewBag.Submission_Id = new SelectList(db.Submissions, "Submission_Id", "Submission_Name", idea.Submission_Id);
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", idea.User_Id);
            return View(idea);
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
            ViewBag.Submission_Id = new SelectList(db.Submissions, "Submission_Id", "Submission_Name", idea.Submission_Id);
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", idea.User_Id);
            return View(idea);
        }

        // POST: Ideas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Idea_Id,Idea_Title,Idea_Description,Idea_Content,Idea_Create_date,Idea_File_path,User_Id,Category_Id,Submission_Id,Idea_IsDisable")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(idea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name", idea.Category_Id);
            ViewBag.Submission_Id = new SelectList(db.Submissions, "Submission_Id", "Submission_Name", idea.Submission_Id);
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
