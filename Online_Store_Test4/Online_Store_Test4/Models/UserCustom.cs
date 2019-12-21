using Online_Store_Test4.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Online_Store_Test4.Models
{
    public class UserCustom
    {
        public int user_id { get; set; }
        public string fullname { get; set; }
        [Required(ErrorMessage = "User name is required")]
        [RegularExpression("^[A-z]+[A-z0-9]+$", ErrorMessage = "User name has to begin with a letter and no space or special character")]
        [DisplayName("User Name")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public bool active { get; set; }

        
    }
}