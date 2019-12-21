using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Online_Store_Test4.Database;

namespace Online_Store_Test4.Models
{
    public class CategoryCustom
    {
        private Category _category;

        public CategoryCustom(Category category)
        {
            _category = category;
            category.category_id = category_id;
            category.name = name;
            category.category_image_URL = category_image_URL;
            category.admin_name = admin_name;
            category.Admin = Admin;
            category.Products = Products;
        }

        public void UpdateCategory(Category category)
        {
            category.category_id = category_id;
            category.name = name;
            category.category_image_URL = category_image_URL;
            category.admin_name = admin_name;
        }

        public CategoryCustom()
        {

        }

        public int category_id { get; set; }
        public string name { get; set; }
        public string category_image_URL { get; set; }
        public string admin_name { get; set; }

        public virtual Admin Admin { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}