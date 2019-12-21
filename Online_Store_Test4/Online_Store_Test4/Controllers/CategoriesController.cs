using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Store_Test4.Database;
using Online_Store_Test4.App_Start;
using Online_Store_Test4.Models;
using Online_Store_Test4;
using System.IO;
using PagedList;

namespace Online_Store_Test4.Controllers
{
    [Authorization]
    public class CategoriesController : Controller
    {
        private OnlineShopEntities2 db = new OnlineShopEntities2();

        // GET: Categories
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.Admin);
            return View(categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.admin_name = new SelectList(db.Admins, "admin_name", "password");

            CategoryCustom category = new CategoryCustom();

            return View(category);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCustom categoryCustom , HttpPostedFileBase ImageFile)
        {
            var newcategory = new Category();

            if(ImageFile != null)
            {
                string relativePath = "/CategoryImages/" + DateTime.Now.Ticks.ToString() + "_" + ImageFile.FileName;

                string physicalPath = Server.MapPath(relativePath);

                string imageFolder = Path.GetDirectoryName(physicalPath);

                if(!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                ImageFile.SaveAs(physicalPath);
                categoryCustom.category_image_URL = relativePath;
            }

            categoryCustom.admin_name = Session["AdminName"].ToString();
            categoryCustom.UpdateCategory(newcategory);

            if (ModelState.IsValid)
            {
                db.Categories.Add(newcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.admin_name = new SelectList(db.Admins, "admin_name", "password", categoryCustom.admin_name);
            return View(categoryCustom);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.admin_name = new SelectList(db.Admins, "admin_name", "password", category.admin_name);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "category_id,name,category_image_URL,admin_name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.admin_name = new SelectList(db.Admins, "admin_name", "password", category.admin_name);
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Ads(int? id, int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.Where(x => x.category_id == id).OrderByDescending(x => x.product_id).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
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
