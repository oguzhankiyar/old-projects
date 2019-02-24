using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OKPort
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "okml_decode",
                url: "okml_decode",
                defaults: new { controller = "Home", action = "decode" },
                namespaces: new[] { "OKPort.Controllers" }
            );

            routes.MapRoute(
                name: "okml",
                url: "okml",
                defaults: new { controller = "Home", action = "OKML" },
                namespaces: new[] { "OKPort.Controllers" }
            );

            routes.MapRoute(
                name: "Search",
                url: "search/{query}",
                defaults: new { controller = "Home", action = "Search" },
                namespaces: new[] { "OKPort.Controllers" }
            );

            routes.MapRoute(
                name: "Post",
                url: "{pageURL}/{sectionURL}/{categoryURL}/{postURL}",
                defaults: new { controller = "Page", action = "Post" },
                namespaces: new[] { "OKPort.Controllers" }
            );

            routes.MapRoute(
                name: "Category",
                url: "{pageURL}/{sectionURL}/{categoryURL}",
                defaults: new { controller = "Page", action = "Category" },
                namespaces: new[] { "OKPort.Controllers" }
            );

            routes.MapRoute(
                name: "Section",
                url: "{pageURL}/{sectionURL}",
                defaults: new { controller = "Page", action = "Section" },
                namespaces: new[] { "OKPort.Controllers" }
            );

            routes.MapRoute(
                name: "Page",
                url: "{pageURL}",
                defaults: new { controller = "Page", action = "Details" },
                namespaces: new[] { "OKPort.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "OKPort.Controllers" }
            );
        }
    }
}
