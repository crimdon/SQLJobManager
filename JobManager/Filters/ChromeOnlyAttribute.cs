using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobManager.Filters
{
    public class ChromeOnlyAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var userAgent = filterContext.HttpContext.Request.Headers["User-Agent"];
            if (!IsChrome(userAgent))
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Unauthorized.cshtml"
                };
            }
        }

        private bool IsChrome(string userAgent)
        {
            if (userAgent.Contains("Chrome"))
            {
                return true;
            }
            return false;
        }
    }
}