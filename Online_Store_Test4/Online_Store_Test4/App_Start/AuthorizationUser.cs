using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Store_Test4.App_Start
{
    public class AuthorizationUser : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(filterContext.HttpContext.Session["UserID"] == null)
            {
                filterContext.HttpContext.Response.Redirect("/Users/Login");
            
            }
        }
    }
}