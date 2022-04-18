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
    public class ViewsController : Controller
    {
        private AppModel db = new AppModel();

        // GET: Views
        public ActionResult Index()
        {
            var views = db.Views.Include(v => v.Idea);
            return View(views.ToList());
        }

        // GET: Views/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            View view = db.Views.Find(id);
            if (view == null)
            {
                return HttpNotFound();
            }
            return View(view);
        }

        // GET: Views/Create
        public ActionResult Create()
        {
            ViewBag.Idea_Id = new SelectList(db.Ideas, "Idea_Id", "Idea_Title");
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name");
            return View();
        }

        // POST: Views/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "View_Id,User_Id,Idea_Id")] View view)
        {
            if (ModelState.IsValid)
            {
                db.Views.Add(view);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Idea_Id = new SelectList(db.Ideas, "Idea_Id", "Idea_Title", view.Idea_Id);
            //ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", view.User_Id);
            return View(view);
        }

        // GET: Views/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            View view = db.Views.Find(id);
            if (view == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idea_Id = new SelectList(db.Ideas, "Idea_Id", "Idea_Title", view.Idea_Id);
            //ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", view.User_Id);
            return View(view);
        }

        // POST: Views/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "View_Id,User_Id,Idea_Id")] View view)
        {
            if (ModelState.IsValid)
            {
                db.Entry(view).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Idea_Id = new SelectList(db.Ideas, "Idea_Id", "Idea_Title", view.Idea_Id);
            //ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name", view.User_Id);
            return View(view);
        }

        // GET: Views/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            View view = db.Views.Find(id);
            if (view == null)
            {
                return HttpNotFound();
            }
            return View(view);
        }

        // POST: Views/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            View view = db.Views.Find(id);
            db.Views.Remove(view);
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
