using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGK.Functions
{
    public class MaintenanceControl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!BGKFunction.GetMyRole().BakimdaGiris && BGKFunction.GetConfig("site-on/off").ToString() == "0")
            {
                UrlHelper url = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new EmptyResult();
                filterContext.Result = new RedirectResult(url.Action("Maintenance", "Home", new { area = "" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}