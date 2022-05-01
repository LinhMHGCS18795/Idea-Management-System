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
using Microsoft.Ajax.Utilities;
using PagedList;
using PagedList.Mvc;
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
            //List<Statistic> list = StatisticByContribute();
            //Test();

            //Page List
            if (page == null) page = 1;            
            var idea = (from l in db.Ideas select l).OrderBy(x => x.Idea_Id);                       
            int pageSize = 5;            
            int pageNumber = (page ?? 1);
            
            return View(idea.ToPagedList(pageNumber, pageSize));
        

            //return View(ideas.ToList());

        }

        // GET: Ideas/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name");
            ViewBag.id = Convert.ToInt32(Session["User_Id"]);
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
                SendNotification(db.Ideas.Find(idea.Idea_Id));
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
            
            var department = db.Ideas.Include(u => u.User).Include(d => d.User.Department).
                Where(x => x.User_Id == idea.User_Id).Select(s => s.User.Department_Id).ToList();
            
            if (department.Count != 0)
            {
                var qaList = db.Users.Include(d => d.Department).Where(x=> department.Contains(x.Department_Id) && x.Role.Role_Name == "QA Coordinator").ToList();
                var userSendIDea = db.Users.Find(idea.User_Id);
                if (qaList.Count != 0)
                {
                    foreach (var u in qaList)
                    {
                        System.Diagnostics.Debug.WriteLine("Email>>>>> "+u.User_Name);
                        EmailModel notificationMail = new EmailModel();
                        notificationMail.To = u.Email;
                        notificationMail.Subject = "New Idea has created!";
                        if(userSendIDea != null)
                        {
                            notificationMail.Body = "User: " + userSendIDea.User_Name +" send new idea!";
                        } else
                        {
                            notificationMail.Body = "User in your department send new idea!";
                        }
                       
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

        public List<Statistic> Test()
        {
            List<Statistic> statisticsList = new List<Statistic>();
            //load department
            var department = db.Departments.Select(d => d.Department_Name).ToList();
            if (department.Count() > 0)
            {
                foreach(var d in department)
                {
                    Statistic s = new Statistic();
                    s.departmentName = d.ToString();
                    statisticsList.Add(s);
                }
            }

            var listUser = db.Ideas.Include(i => i.User).Select(x => x.User_Id).Distinct().ToList();
            if(listUser != null)
            {
                /// contribute statistic
                var listUserContributed = db.Users.Include(u => u.Department).
               Where(x => listUser.Contains(x.User_Id)).
               GroupBy(d => d.Department.Department_Name).Select(z => new
               {
                   DepartmentName = z.Key,
                   Count = z.Count()
               });

                /// total idea statistic
                var listTotalIdea = db.Ideas.Include(u => u.User).Include(d => d.User.Department).
                    GroupBy(d =>d.User.Department.Department_Name).Select(z => new
                {
                    DepartmentName = z.Key,
                    Count = z.Count()
                });

                /// percentage statistic
                float totalIdea = db.Ideas.ToList().Count();

                if (listUserContributed.ToList().Count != 0 && listTotalIdea.ToList().Count != 0)
                {
                    
                    foreach(var i in listUserContributed)
                    {
                        foreach(var s in statisticsList)
                        {
                            if(s.departmentName == i.DepartmentName)
                            {
                                s.contributeNumber = i.Count;
                            }
                        }
                    }

                    foreach(var i in listTotalIdea)
                    {
                        foreach(var s in statisticsList)
                        {
                            if(s.departmentName == i.DepartmentName)
                            {
                                s.totalIdea = i.Count;
                                s.percentageIdea = ((i.Count / totalIdea) * 100);
                                s.percentageIdea = (float)Math.Round(s.percentageIdea, 2);
                            }
                        }
                    }
                    return statisticsList;
                }
            }
            return null;
        }

        public DataTable TestDataTable()
        {
            DataTable dtable = new DataTable();
            dtable.Columns.Add("Department_Name", typeof(string));
            dtable.Columns.Add("Contributors", typeof(int));

            var listUser = db.Ideas.Include(i => i.User).Select(x => x.User_Id).Distinct().ToList();
            if (listUser != null)
            {
                var listUserContributed = db.Users.Include(u => u.Department).
               Where(x => listUser.Contains(x.User_Id)).
               GroupBy(d => d.Department.Department_Name).Select(z => new
               {
                   DepartmentName = z.Key,
                   Count = z.Count()
               });
                if (listUserContributed.ToList().Count != 0 && listUserContributed.ToList().Count > 0)
                {
                    

                    List<Statistic> statisticsList = new List<Statistic>();
                    foreach (var i in listUserContributed)
                    {
                        Statistic s = new Statistic();
                        s.departmentName = i.DepartmentName;
                        s.contributeNumber = i.Count;
                        statisticsList.Add(s);
                        dtable.Rows.Add(new object[] { i.DepartmentName, i.Count });
                        System.Diagnostics.Debug.WriteLine(">>>Name " + i.DepartmentName + " ------- " + i.Count);
                    }
                    return dtable;
                }
            }
            return null;
            //Total contribute
            //var listUserContribute = db.Ideas.Include(i => i.User).Include(u => u.User.Department).
            //    Select(x => new { Department_Name = x.User.Department.Department_Name, User_Id = x.User.User_Id }).ToList();
            //System.Diagnostics.Debug.WriteLine(">>>listUserContribute: " + listUserContribute.Distinct().Count());

        }

        /*public List<Statistic> StatisticByContribute()
        {
            //DateTime DateFrom = DateTime.Now;
            //DateTime DateTo = DateTime.Now;

            var users1 = db.Users.Include(d => d.Department).Distinct();

            //  ideas1.Where(i => i.Idea_Create_date >= DateFrom && i.Idea_Create_date <= DateTo);
            var ideas1 = db.Ideas.Include(u => u.User).
                Where(i => users1.Select(u => u.User_Id).ToList().Contains(i.User_Id.Value))
                .Select(u => new { u.User_Id,u.User.Department.Department_Name, u.User.Department_Id }).Distinct();
            System.Diagnostics.Debug.WriteLine(">>> " + ideas1.ToList().Count);
            //// Count user_ID and group by department name in ideas1 ??????
            //ideas1.Select(x => new {x.User_Id, count = Count(x.User_Id)});
            //System.Diagnostics.Debug.WriteLine(">>>>>>" +);
            //ideas1.Select(y => new {y.Department_Name, Count = ideas1.Count(y.User_Id)});
            //ideas1.GroupBy(y => y.Department_Name).Select(z => new { z.Key, Count =)})
 
            foreach (var item in ideas1)
            {
                
                System.Diagnostics.Debug.WriteLine("NAME: "+item.Department_Name+" _____ID: "+item.User_Id);
            }
           

            /// number of contributor of each department
            

            List < Statistic > listStatistic = new List<Statistic>();
            return listStatistic;
        }*/

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
            Idea i = new Idea();
            if (commentList.Count != 0)
            {
                i = db.Ideas.Find(commentList.First().Idea_Id);
            }
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
