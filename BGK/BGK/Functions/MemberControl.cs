using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGK.Functions
{
    public class MemberControl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["memberID"].ToString() == "0")
            {
                UrlHelper url = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new EmptyResult();
                filterContext.Result = new RedirectResult(url.Action("NoMember", "Error"));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}