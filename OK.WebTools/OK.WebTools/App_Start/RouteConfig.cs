using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OK.WebTools
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Formatter",
                url: "formatter",
                defaults: new { controller = "Formatter", action = "Index" }
            );
            
            routes.MapRoute(
                name: "Chars",
                url: "chars",
                defaults: new { controller = "Chars", action = "Index" }
            );

            routes.MapRoute(
                name: "ColorCodes",
                url: "color-codes",
                defaults: new { controller = "Home", action = "ColorCodes" }
            );

            routes.MapRoute(
                name: "Requester",
                url: "requester",
                defaults: new { controller = "Requester", action = "Index" }
            );

            routes.MapRoute(
                name: "UrlCoder",
                url: "url-coder",
                defaults: new { controller = "Home", action = "UrlCoder" }
            );

            routes.MapRoute(
                name: "HtmlCoder",
                url: "html-coder",
                defaults: new { controller = "Home", action = "HtmlCoder" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
