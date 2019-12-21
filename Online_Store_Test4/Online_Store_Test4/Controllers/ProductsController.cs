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
using System.IO;

namespace Online_Store_Test4.Controllers
{
    
    public class ProductsController : Controller
    {
        private OnlineShopEntities2 db = new OnlineShopEntities2();

        // GET: Products
       
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Admin);
            return View(products.ToList());

        }

        
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorization]
        public ActionResult Create()
        {
            ViewBag.admin_name = new SelectList(db.Admins, "admin_name", "password");

            ProductCustom products = new ProductCustom();
            products.posted_time = DateTime.Now;
            return View(products);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCustom productcustom , HttpPostedFileBase ImageFile)
        {

            if (ModelState.IsValid)
            {
                var newProduct = new Product();

                if(ImageFile != null)
                {
                    string relativePath = "/ProductImages/" + DateTime.Now.Ticks.ToString() + "_" + ImageFile.FileName;

                    string physicalPath = Server.MapPath(relativePath);

                    string imageFolder = Path.GetDirectoryName(physicalPath);

                    if(!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    ImageFile.SaveAs(physicalPath);
                    productcustom.image_url = relativePath;
                }

                productcustom.admin_name = Session["AdminName"].ToString();

                productcustom.UpdateProduct(newProduct);

                db.Products.Add(newProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.admin_name = int.Parse(Session["AdminName"].ToString());
            return View(productcustom);
        }

        // GET: Products/Edit/5
        [Authorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.admin_name = new SelectList(db.Admins, "admin_name", "password", product.admin_name);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,name,short_description,detail,image_url,cost,posted_time,admin_name")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.admin_name = new SelectList(db.Admins, "admin_name", "password", product.admin_name);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [Authorization]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private string strcart = "Cart";

        public ActionResult OderNow()
        {
            return View("OrderNow");
        }

        public ActionResult OrderNow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }
            if (Session[strcart] == null)
            {
                List<Cart> lsCart = new List<Cart>
                {
                    new Cart(db.Products.Find(id),1)
                };
                Session[strcart] = lsCart;
            }
            else
            {
                List<Cart> lsCart = (List<Cart>)Session[strcart];
                lsCart.Add(new Cart(db.Products.Find(id), 1));
                Session[strcart] = lsCart;
            }
            return View("OrderNow");
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
