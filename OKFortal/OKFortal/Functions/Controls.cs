﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OKFortal.Functions
{
    public class SiteControl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (OK.Config("site-on/off") == "0" && HttpContext.Current.Session["role"].ToString() != "10")
            {
                filterContext.Result = new EmptyResult();
                filterContext.Result = new RedirectResult("~/Error/SiteOff");
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class AdminControl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["role"].ToString() != "10")
            {
                filterContext.Result = new EmptyResult();
                filterContext.Result = new ContentResult() { Content = "<script type=\"text/javascript\">alert('Yönetim paneline girebilmeniz için yönetici girişi yapmanız gerekiyor.. Anasayfaya yönlendiriliyorsunuz..'); window.location.href='/';</script>" };
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
