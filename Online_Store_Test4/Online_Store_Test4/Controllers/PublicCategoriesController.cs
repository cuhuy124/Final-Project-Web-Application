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
    public class PublicCategoriesController : Controller
    {
        private OnlineShopEntities2 db = new OnlineShopEntities2();
        private string strcart = "Cart";
        // GET: PublicCategories
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.Admin);
            return View(categories.ToList());
        }

        public ActionResult Ads(int? id, int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.Where(x => x.category_id == id).OrderByDescending(x => x.product_id).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }     
    }
}