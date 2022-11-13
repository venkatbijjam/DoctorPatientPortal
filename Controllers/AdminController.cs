using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DP_Portal;
using DP_Portal.Models;

namespace DP_Portal.Controllers
{
    public class AdminController : Controller
    {
        private DP_PortalEntities db = new DP_PortalEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminDashboard(string code =null)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Models.AdminDashboard admin = new AdminDashboard();
            var results = db.USERS.Where(a => a.ISACTIVE == true).Select(a => a).ToList();
            admin.doctor_count = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "DOCTOR").Count();
            admin.patient_count = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "PATIENT").Count();
            admin.admin_count = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "ADMIN").Count();
            admin.total_count = results.Count();
            if(code =="" || code == null)
            {
                admin.users_details = results;
            }
            else
            {
                admin.users_details = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == code).Select(a => a).ToList();

            }


            return View(admin);
        }

        public ActionResult AdminManageUsers(string searchString)
        {
            var uSERS = db.USERS.Select(a => a);

            if (!String.IsNullOrEmpty(searchString))
            {
                uSERS = uSERS.Where(s => s.EMAIL_ID.Contains(searchString)
                                       || s.FIRST_NAME.Contains(searchString)
                                       || s.LAST_NAME.Contains(searchString)
                                       || s.USER_NAME.Contains(searchString)
                                       || s.USERS_TYPE.USER_TYPE_NAME.Contains(searchString));
            }
           
            return View(uSERS.ToList());
        }
        //[HttpPost]
        //public ActionResult AdminDashboard(string code)
        //{
        //    if (Session["user"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    Models.AdminDashboard admin = new AdminDashboard();
        //    var results = db.USERS.Where(a => a.ISACTIVE == true).Select(a => a).ToList();
        //    admin.doctor_count = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "DOCTOR").Count();
        //    admin.patient_count = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "PATIENT").Count();
        //    admin.admin_count = results.Where(a => a.USERS_TYPE.USER_TYPE_NAME == "ADMIN").Count();
        //    admin.users_details = results.Where(a=>a.USERS_TYPE.USER_TYPE_NAME== code).Select(a=>a).ToList();

        //    return View(admin);
        //}



        // GET: USERs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }

        // GET: USERs/Create
        public ActionResult Create()
        {
            ViewBag.USER_TYPE = new SelectList(db.USERS_TYPE, "USER_TYPE_ID", "USER_TYPE_NAME");
            return View();
        }

        // POST: USERs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USER_ID,USER_NAME,EMAIL_ID,PASSWORD,FIRST_NAME,LAST_NAME,MOBILE,USER_TYPE,ISACTIVE")] USER uSER)
        {
            if (ModelState.IsValid)
            {
                db.USERS.Add(uSER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.USER_TYPE = new SelectList(db.USERS_TYPE, "USER_TYPE_ID", "USER_TYPE_NAME", uSER.USER_TYPE);
            return View(uSER);
        }

        // GET: USERs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            ViewBag.USER_TYPE = new SelectList(db.USERS_TYPE, "USER_TYPE_ID", "USER_TYPE_NAME", uSER.USER_TYPE);
            return View(uSER);
        }

        // POST: USERs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_ID,USER_NAME,EMAIL_ID,PASSWORD,FIRST_NAME,LAST_NAME,MOBILE,USER_TYPE,ISACTIVE")] USER uSER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.USER_TYPE = new SelectList(db.USERS_TYPE, "USER_TYPE_ID", "USER_TYPE_NAME", uSER.USER_TYPE);
            return View(uSER);
        }

        // GET: USERs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }

        // POST: USERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USER uSER = db.USERS.Find(id);
            db.USERS.Remove(uSER);
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