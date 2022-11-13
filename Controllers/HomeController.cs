using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DP_Portal.Models;

namespace DP_Portal.Controllers
{
    public class HomeController : Controller
    {
        private DP_PortalEntities db = new DP_PortalEntities();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login)
        {
            var user =db.USERS.Where(a => a.USER_NAME == login.UserName && a.PASSWORD == login.Password && a.ISACTIVE == true).Select(a=>a).FirstOrDefault();
            if (user == null)
            {
                Session["Error"] = "Username or Password is wrong.";
                return RedirectToAction("Index","Home");
            }
            Session["Error"] = null;

            Session["login_user"] = login.UserName;
            Session["user_type"] = user.USERS_TYPE.USER_TYPE_NAME.ToString();
            Session["user"] = user.FIRST_NAME + " " + user.LAST_NAME;
            Session["user_id"] = user.USER_ID;

            
            if (user.USERS_TYPE.USER_TYPE_NAME.ToString() !="")
                return RedirectToAction(user.USERS_TYPE.USER_TYPE_NAME.ToString()+"Dashboard", user.USERS_TYPE.USER_TYPE_NAME.ToString());
            return View();
        }


        public  ActionResult Welcome_User()
        {

            return View();
        }

        public ActionResult layout()
        {


            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USER_ID,USER_NAME,EMAIL_ID,PASSWORD,FIRST_NAME,LAST_NAME,MOBILE,USER_TYPE,ISACTIVE")] USER user_details)
        {
            if (ModelState.IsValid)
            {
                user_details.ISACTIVE = true;
                user_details.USER_TYPE = 3;
                db.USERS.Add(user_details);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

                return RedirectToAction("index");
        }
        public ActionResult LogOut()
        {
            Session["login_user"] = null;
            Session["user_type"] = null;
            Session["User"] = null;
            return RedirectToAction("index");
        }


    }
}