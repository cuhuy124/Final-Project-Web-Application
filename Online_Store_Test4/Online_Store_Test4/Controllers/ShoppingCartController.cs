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
    public class ShoppingCartController : Controller
    {
        
        private OnlineShopEntities2 db = new OnlineShopEntities2();
        private string strcart = "Cart";
        // GET: ShoppingCart
        [AuthorizationUser]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizationUser]
        public ActionResult OrderNow(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }
            if(Session[strcart] == null)
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
                int check = IsExistingCheck(id);
                if (check == -1)
                    lsCart.Add(new Cart(db.Products.Find(id), 1));
                else
                    lsCart[check].Quantity++;

                
                Session[strcart] = lsCart;
            }
            return View("Index");
        }

        private int IsExistingCheck(int? id)
        {
            List<Cart> lsCart = (List<Cart>)Session[strcart];
            for(int i = 0; i < lsCart.Count; i++)
            {
                if (lsCart[i].Product.product_id == id)
                    return i;
            }
            return -1;
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }
            int check = IsExistingCheck(id);
            List<Cart> lsCart = (List<Cart>)Session[strcart];
            lsCart.RemoveAt(check);
            return View("Index");
        }
    }
}