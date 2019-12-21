using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Online_Store_Test4.Database;
using Online_Store_Test4.Models;
using Online_Store_Test4.App_Start;
namespace Online_Store_Test4.Controllers
{
    
    public class AdminsController : Controller
    {
        private OnlineShopEntities2 db = new OnlineShopEntities2();

        [Authorization]
        // GET: Admins
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }
        [Authorization]
        // GET: Admins/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }
        [Authorization]
        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "admin_name,password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                admin.password = EncMD5(admin.password);
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
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

        // GET: Admins/Edit/5
        [Authorization]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "admin_name,password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        [Authorization]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [Authorization]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult LoginForAdmin()
        {
            return View("LoginForAdmin");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginForAdmin(AdminCustom adminLogin)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(adminLogin.admin_name)
                    && !string.IsNullOrEmpty(adminLogin.password))
                {
                    string password = EncMD5(adminLogin.password.Trim());
                    var admin = db.Admins.FirstOrDefault(u => u.admin_name == adminLogin.admin_name.Trim() && u.password == password );

                    if (admin != null)
                    {
                        Session["AdminName"] = admin.admin_name;
                        Response.Cookies.Add(new HttpCookie("AdminName", admin.admin_name.ToString()));
                        return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        ViewBag.ErrorMessage = "User name or password is incorrect";
                        return View("LoginForAdmin", adminLogin);
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "User name or password is empty";
                }

            }
            return View(adminLogin);
        }


        [Authorization]
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
