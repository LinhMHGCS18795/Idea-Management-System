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
    public class ReactionsController : Controller
    {
        private AppModel db = new AppModel();

        // GET: Reactions
        public ActionResult Index()
        {
            var reactions = db.Reactions.Include(r => r.Idea).Include(r => r.User);
            return View(reactions.ToList());
        }

        // GET: Reactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reaction reaction = db.Reactions.Find(id);
            if (reaction == null)
            {
                return HttpNotFound();
            }
            return View(reaction);
        }

        // GET: Reactions/Create
        public ActionResult Create()
        {
            ViewBag.Idea_Id = new SelectList(db.Ideas, "Idea_Id", "Idea_Title");
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name");
            return View();
        }

        // POST: Reactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Reaction_Id,is_like,Create_Date,User_Id,Idea_Id")] Reaction reaction)
        {
            if (ModelState.IsValid)
            {
                db.Reactions.Add(reaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Idea_Id = new SelectList(db.Ideas, "Idea_Id", "Idea_Title", reaction.Idea_Id);
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", reaction.User_Id);
            return View(reaction);
        }

        // GET: Reactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reaction reaction = db.Reactions.Find(id);
            if (reaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idea_Id = new SelectList(db.Ideas, "Idea_Id", "Idea_Title", reaction.Idea_Id);
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", reaction.User_Id);
            return View(reaction);
        }

        // POST: Reactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Reaction_Id,is_like,Create_Date,User_Id,Idea_Id")] Reaction reaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Idea_Id = new SelectList(db.Ideas, "Idea_Id", "Idea_Title", reaction.Idea_Id);
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", reaction.User_Id);
            return View(reaction);
        }

        // GET: Reactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reaction reaction = db.Reactions.Find(id);
            if (reaction == null)
            {
                return HttpNotFound();
            }
            return View(reaction);
        }

        // POST: Reactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reaction reaction = db.Reactions.Find(id);
            db.Reactions.Remove(reaction);
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
