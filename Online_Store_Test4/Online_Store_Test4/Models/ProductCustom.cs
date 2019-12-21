using Online_Store_Test4.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Online_Store_Test4.Models
{
    public class ProductCustom
    {
        private Product _products;

        public ProductCustom(Product product)
        {
            product = _products;

            product.product_id = product_id;
            product.name = name;
            product.category_id = category_id;
            product.short_description = short_description;
            product.detail = detail;
            product.image_url = image_url;
            product.cost = cost;
            product.posted_time = posted_time;
            product.admin_name = admin_name;

            product.Admin = Admin;

            product.Orders = Orders;

           
        }

        public ProductCustom()
        {

        }

        public void UpdateProduct(Product product)
        {
            product.product_id = product_id;
            product.name = name;
            product.category_id = category_id;
            product.short_description = short_description;
            product.detail = detail;
            product.image_url = image_url;
            product.cost = cost;
            product.posted_time = posted_time;
            product.admin_name = admin_name;
        }



        public int product_id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
        public string short_description { get; set; }
        [AllowHtml]
        public string detail { get; set; }
        public string image_url { get; set; }
        public double cost { get; set; }
        public System.DateTime posted_time { get; set; }
        public string admin_name { get; set; }

        public virtual Admin Admin { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual Category Category { get; set; }
    }
}