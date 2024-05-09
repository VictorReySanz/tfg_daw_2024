using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TfgDAW.ActionFilter
{
    //[AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PerfilAction : ActionFilterAttribute
    {
        public bool Disable { get; set; }
        public string Profiles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Disable) return;
            var sessionContents = filterContext.HttpContext.Session.Contents;

            if (!this.Profiles.Contains(sessionContents["rol"].ToString()))
            {
                filterContext.Result = new RedirectResult("~/Home/Unauthorized");
            }
        }

    }
}