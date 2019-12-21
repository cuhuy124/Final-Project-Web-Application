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
    public class PublicProductsController : Controller
    {
        private OnlineShopEntities2 db = new OnlineShopEntities2();
        // GET: PublicProducts
        [Route("")]
        [Route("category /{categoryid}/{name}")]
        public ActionResult Index(int? categoryid , string name)
        {
            var products = db.Products.Include(p => p.Admin).Where(p => p.category_id == categoryid);
            ViewBag.Categories = db.Categories;
            return View(products.ToList());
        }

        [Route("detail/{name}-{id}")]
        public ActionResult Details(string name , int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product products = db.Products.Find(id);
            if(products == null)
            {
                return HttpNotFound();
            }

            return View(products);
        }

        public static string EncodeUrl(Product products)
        {
            return string.Format("detail/{0}-{1}", HttpUtility.UrlEncode(products.name), products.product_id);
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