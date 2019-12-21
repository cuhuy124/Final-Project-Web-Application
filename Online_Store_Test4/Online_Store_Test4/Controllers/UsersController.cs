using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Store_Test4.Database;
using System.Security.Cryptography;
using System.Text;
using Online_Store_Test4.Models;
using Online_Store_Test4.App_Start;

namespace Online_Store_Test4.Controllers
{
    public class UsersController : Controller
    {
        private OnlineShopEntities2 db = new OnlineShopEntities2();

        [Authorization]
        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        [AuthorizationUser]
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,fullname,username,password,active")] User user)
        {
            if (ModelState.IsValid)
            {
                user.password = EncMD5(user.password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public static string EncMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] originalBytes = encoder.GetBytes(password);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            password = BitConverter.ToString(encodedBytes).Replace("-", "");
            var result = password.ToLower();
            return result;
        }

        // GET: Users/Edit/5
        [AuthorizationUser]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizationUser]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,fullname,username,password,active")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [Authorization]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserCustom userModelView)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(userModelView.username)
                    && !string.IsNullOrEmpty(userModelView.password))
                {
                    string password = EncMD5(userModelView.password.Trim());
                    var user = db.Users.FirstOrDefault(u => u.username == userModelView.username.Trim() && u.password == password && u.active);

                    if (user != null)
                    {
                        Session["UserID"] = user.user_id;
                        Response.Cookies.Add(new HttpCookie("UserID", user.user_id.ToString()));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "User name or password is incorrect";
                        return View("Login", userModelView);
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "User name or password is empty";
                }

            }
            return View(userModelView);
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["AdminName"] = null;
            return RedirectToAction("Login");
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
