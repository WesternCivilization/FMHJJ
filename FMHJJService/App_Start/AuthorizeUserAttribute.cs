using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using FMHJJService.Business.FMHJJ;

namespace FMHJJService
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);
			if (filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
                if (HttpContext.Current.Session["loginUser"] == null)
                {
                    HttpContext.Current.Session["loginUser"] = UserBusiness.GetUser(filterContext.HttpContext.User.Identity.Name);
                    HttpContext.Current.Session.Timeout = 300;
                }
			}			
		}
    }
}