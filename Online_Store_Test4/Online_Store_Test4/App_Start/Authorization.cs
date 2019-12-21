using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Store_Test4.App_Start
{
    public class AuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["AdminName"] == null)
            {
                filterContext.HttpContext.Response.Redirect("/Home/Index");
            }      

            
        }
    }
}