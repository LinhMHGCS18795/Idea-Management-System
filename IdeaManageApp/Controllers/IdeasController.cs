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
using PagedList;
using Xceed.Wpf.Toolkit;

namespace IdeaManageApp.Controllers
{
    public class IdeasController : Controller
    {
        private EmailServiceController emailServiceController = new EmailServiceController();
        private AppModel db = new AppModel();

        // GET: Ideas
        public ActionResult Index(int? page)
        {
            ViewData["Role"] = Session["Role"];
            var ideas = db.Ideas.Include(i => i.Category).Include(i => i.User);
            return View(ideas.ToList());

            if (page == null) page = 1;

            var links = (from l in db.Ideas
                         select l).OrderBy(x => x.Idea_Id);

            int pageSize = 3;

            int pageNumber = (page ?? 1);

            return View(links.ToPagedList(pageNumber, pageSize));
        }

        // GET: Ideas/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name");
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name");
            return View();
        }

        //[Bind(Include = "Idea_Id,Idea_Title,Idea_Content,Idea_Create_date,Idea_File_path,User_Id,Category_Id")]
        //POST: UPLOAD FILE
        [HttpPost]
        public ActionResult Create(Idea idea)
        {
            if (ModelState.IsValid && idea.TermsAndCondition == true && CheckTimeIdea(idea.Category_Id, idea.Idea_Create_date))
            {
                if (idea.MyFile != null)
                {
                    UploadFile(idea);
                }
                db.Ideas.Add(idea);
                // db.SaveChanges();
                View view = new View();
                view.Total_View = 0;
                view.Idea_Id = idea.Idea_Id;
                db.Views.Add(view);
                db.SaveChanges();
                //SendNotification(idea);
                return RedirectToAction("Index");
            }
            else
            {
                if (!CheckTimeIdea(idea.Category_Id, idea.Idea_Create_date))
                {
                    Response.Write("Your idea is overdue");
                }
                if (!idea.TermsAndCondition)
                {
                    Response.Write("You have to agree terms and Conditions!!!");
                }
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name");
            ViewBag.User_Id = new SelectList(db.Users, "User_Id", "User_Name");
            return View(idea);
        }

        public void UploadFile(Idea idea)
        {
            string fileName = Path.GetFileNameWithoutExtension(idea.MyFile.FileName);
            string extension = Path.GetExtension(idea.MyFile.FileName);
            fileName = fileName + extension;
            idea.Idea_File_path = fileName;
            fileName = Path.Combine(Server.MapPath("../Files/"), fileName);
            idea.MyFile.SaveAs(fileName);
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

        protected Boolean CheckTimeIdea(int? cateID, DateTime? createDate)
        {
            Category cate = db.Categories.Find(cateID);
            //DateTime current = DateTime.Today;
            int ideaValid = DateTime.Compare((DateTime)createDate, (DateTime)cate.Submission.Submission_Closure_date);
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

        public Boolean CheckTimeComment(int IdeaId)
        {
            DateTime current = DateTime.Today;

            Idea currentIdea = db.Ideas.Find(IdeaId);

            if (currentIdea != null)
            {
                //System.Diagnostics.Debug.WriteLine(">>>>>>222" + IdeaId);

                int commentValidate = DateTime.Compare((DateTime)current, (DateTime)currentIdea.Category.Submission.Submission_Final_closure_date);
                if (commentValidate > 0)
                {
                    // notification out of date
                    return false;
                   
                }
                else
                {
                    return true;
                    //return View(create comment);
                }
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine(">>>>>>333" + IdeaId);

                // notification this idea not exist
                //Response.Write("This idea is not exist!!!");
                return false;
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
            var department = from d in db.Departments
                                 join u in db.Users on d.Department_Id equals u.Department_Id
                                 where u.User_Id == idea.User_Id select d;
            Department dept = new Department();
            if (department != null)
            {
                dept = department.First();
                var list = db.Users.Include(d => d.Department).Where(u => u.Role.Role_Name == "QA Coordinator" && u.Department_Id == dept.Department_Id);
                if (list != null)
                {
                    foreach (var u in list.ToList())
                    {
                        
                        notificationMail.To = u.Email;
                        notificationMail.Subject = "New Idea has created!";
                        notificationMail.Body = "User: " + u.User_Name + " send new idea";
                        emailServiceController.FillEmailAndSend(notificationMail);
                    }
                }
            }
        }



        public List<Statistic> statisticPercentageIdea()
        {
            var ideas = db.Ideas.Include(i => i.User);
            var departments = db.Departments;
            var users = db.Users.Include(u => u.Department);

            /// percentage of idea of each department 
            var query1 = from dept in departments
                         join user in users on dept.Department_Id equals user.Department_Id
                         join idea in ideas on user.User_Id equals idea.User.User_Id
                         where idea.User == dept.Users
                         group user.Department by user.Department_Id into dt
                         select new
                         {
                             DepartmentName = dt.Key,
                             Count = dt.Count()
                         };
            int totalIdea = 0;

            List<Statistic> listStatistic = new List<Statistic>();
            Statistic statistic = new Statistic();
            Boolean firstFlag = true;

            foreach (var idea in query1.ToList())
            {
                if (firstFlag)
                {
                    // first item
                    statistic.departmentName = idea.DepartmentName.ToString();
                    statistic.totalIdea += idea.Count;
                    totalIdea += idea.Count;

                    firstFlag = false;
                }
                else
                {
                    if (statistic.departmentName.Equals(idea.DepartmentName))
                    {
                        // same department
                        statistic.totalIdea = statistic.totalIdea + idea.Count;
                        totalIdea = totalIdea + idea.Count;
                    }
                    else
                    {
                        // difference department
                        listStatistic.Add(statistic);
                        statistic = new Statistic();
                        statistic.departmentName = idea.DepartmentName.ToString();
                        statistic.totalIdea += idea.Count;
                        totalIdea = totalIdea + idea.Count;
                    }
                }
            }

            foreach (var s in listStatistic)
            {
                s.percentageIdea = s.totalIdea / totalIdea;
            }

            return listStatistic;
        }

        public List<Statistic> StatisticByTotalIdea()
        {
            var ideas = db.Ideas.Include(i => i.User);
            var users = db.Users.Include(u => u.Department);

            /// Number of idea of each department 
            var query = from idea in ideas
                        join user in users on idea.User_Id equals user.User_Id
                        where user.User_Id == idea.User_Id
                        group user.Department by user.Department_Id into dt
                        select new
                        {
                            DepartmentName = dt.Key,
                            Count = dt.Count()
                        };
            List<Statistic> listStatistic = new List<Statistic>();

            foreach (var s in query.ToList())
            {
                Statistic sta = new Statistic();
                sta.departmentName = s.DepartmentName.ToString();
                sta.totalIdea = s.Count;
                listStatistic.Add(sta);
            }
            return listStatistic;
        }

        public List<Statistic> StatisticByContribute()
        {
            var ideas = db.Ideas.Include(i => i.User);
            var users = db.Users.Include(u => u.Department);
            var departments = db.Departments;

            /// number of contributor of each department
            var query = from idea in ideas
                        join user in users on idea.User_Id equals user.User_Id
                        join department in departments on user.Department_Id equals department.Department_Id
                        group user.Department_Id by user.Department into dt
                        select new
                        {
                            DepartmentName = dt.Key,
                            Count = dt.Key.Users.Distinct().Count()
                        };

            List < Statistic > listStatistic = new List<Statistic>();

            foreach (var s in query.ToList())
            {
                Statistic sta = new Statistic();
                sta.departmentName = s.DepartmentName.Department_Name;
                sta.contributeNumber = s.Count;
                listStatistic.Add(sta);
            }
            return listStatistic;
        }

        public Idea PopularIdea()
        {
            // idea popular id 
            int popularIdea = 0;

            // reaction count
            int popularCount = 0;

            // number of reaction in popular idea
            int maxCount = 0;

            // last reaction id 
            int previousId = 0;
            var reactionList = db.Reactions.Include(i => i.Idea_Id).AsQueryable().GroupBy(i => i.Idea_Id).OrderByDescending(i => i.Key).ToList();
            if (reactionList.Count > 0)
            {
                int i = 0;
                System.Diagnostics.Debug.WriteLine("!!!!!!!Value" + reactionList.ElementAt(i));

                foreach (var p in reactionList)
                {

                    Reaction reaction = p.ElementAt(0);
                    if (previousId == 0)
                    {
                        // first record
                        if (reaction.is_like == true)
                        {
                            popularCount++;
                        }
                        else
                        {
                            popularCount--;
                        }
                        popularIdea = (int)reaction.Idea_Id;
                        previousId = (int)reaction.Idea_Id;
                        maxCount = popularCount;
                    }
                    else
                    {
                        // next record 
                        if (reaction.Idea_Id == previousId)
                        {
                            // same idea
                            if (reaction.is_like == true)
                            {
                                popularCount++;
                            }
                            else
                            {
                                popularCount--;
                            }
                            // current idea not popular
                            if (popularIdea != reaction.Idea_Id)
                            {
                                if (popularCount >= maxCount)
                                {
                                    popularIdea = (int)reaction.Idea_Id;
                                    maxCount = popularCount;
                                }
                            }
                            else
                            {
                                // current idea is popular 
                                if (popularCount >= maxCount)
                                {
                                    // save current idea as popular idea
                                    maxCount = popularCount;
                                }
                            }
                        }
                        else
                        {
                            // differenct idea
                            popularCount = 0;
                            if (reaction.is_like == true)
                            {
                                popularCount++;
                            }
                            else
                            {
                                popularCount--;
                            }

                            previousId = (int)reaction.Idea_Id;
                            if (popularCount >= maxCount)
                            {
                                // save current idea as popular idea
                                popularIdea = (int)reaction.Idea_Id;
                                maxCount = popularCount;
                            }
                        }
                    }
                    i++;
                }
            }
            System.Diagnostics.Debug.WriteLine("!!!!!!!maxCount" + maxCount);

            System.Diagnostics.Debug.WriteLine("!!!!!!!popular" + popularIdea.ToString());
            Idea idea = db.Ideas.Find(popularIdea);
            return idea;
        }
        public Idea LatestIdea()
        {
            //var latestIdea1 = from s in db.Ideas 
            //                  orderby s.Idea_Create_date descending
            //                  select s;
            var ideaList = db.Ideas.OrderByDescending(x => x.Idea_Create_date).AsQueryable().ToList();
            Idea i = ideaList.First();

            System.Diagnostics.Debug.WriteLine("!!!!!!!latest idea" + i.Idea_Id);
            return i;
        }

        public Idea LatestComment()
        {
            //var latestComment = from s in db.Comments
            //                 orderby s.Created_date descending
            //                 select s;
            var commentList = db.Comments.OrderByDescending(x => x.Created_date).AsQueryable().ToList();
            Comment c = commentList.First();
            Idea i = new Idea();
            System.Diagnostics.Debug.WriteLine("!!!!!!!ccccc comment" + c.Idea_Id);

            if (c != null)
            {
                i = db.Ideas.Find(c.Idea_Id);
            }
            System.Diagnostics.Debug.WriteLine("!!!!!!!lastest comment" + i.Idea_Id);
            return i;
        }

        public Idea MostViewIdea()
        {
            var viewList = db.Views.AsQueryable().OrderByDescending(x => x.Total_View);
            View v = viewList.FirstOrDefault();

            Idea i = new Idea();
            if (v != null)
            {
                i = db.Ideas.Find(v.Idea_Id);
            }
            System.Diagnostics.Debug.WriteLine("!!!!!!!Most View" + i.Idea_Id);
            return i;
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
