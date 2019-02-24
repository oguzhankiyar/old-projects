using BGK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGK
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            Session["memberInfo"] = null;
            Session["memberID"] = 0;
            Session["memberRole"] = 0;
            Session["Index"] = "0";
            if (Request.Cookies["BGK_memberID"] != null && Request.Cookies["BGK_password"] != null)
            {
                int memberID = Convert.ToInt32(Request.Cookies["BGK_memberID"].Value);
                string password = Request.Cookies["BGK_password"].Value;
                BGKEntities Db = new BGKEntities();
                var member = Db.bgk_uye.SingleOrDefault(x => x.Id == memberID && x.Sifre == password && x.Onay == true);
                if (member != null)
                {
                    Session["memberInfo"] = member;
                    Session["memberID"] = member.Id;
                    Session["memberRole"] = member.Yetki;
                }
                Db.Dispose();
            }
        }
        protected void Session_End()
        {
            Session["Index"] = "0";
        }
    }
}