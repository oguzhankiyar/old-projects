using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OKComplex
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "PageDetail",
                url: "p/{seo}",
                defaults: new { area = "", controller = "Page", action = "Detail", seo = UrlParameter.Optional },
                namespaces: new[] { "OKComplex.Controllers" }
            );
            routes.MapRoute(
                name: "NoticeDetail",
                url: "n{id}_{seo}",
                defaults: new { area = "", controller = "Home", action = "NoticeDetail", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                namespaces: new[] { "OKComplex.Controllers" }
            );
            routes.MapRoute(
                name: "PostCategory",
                url: "c/{seo}/{page}",
                defaults: new { area = "", controller = "Post", action = "Category", seo = UrlParameter.Optional, page = 1 },
                namespaces: new[] { "OKComplex.Controllers" }
            );
            routes.MapRoute(
                name: "Posts",
                url: "posts/{page}",
                defaults: new { area = "", controller = "Post", action = "List", page = 1 },
                namespaces: new[] { "OKComplex.Controllers" }
            );
            routes.MapRoute(
                name: "PostDetail",
                url: "{id}p{page}_{seo}",
                defaults: new { area = "", controller = "Post", action = "Detail", id = UrlParameter.Optional, page = 1, seo = UrlParameter.Optional },
                namespaces: new[] { "OKComplex.Controllers" }
            );
            routes.MapRoute(
                name: "Groups",
                url: "groups",
                defaults: new { area = "", controller = "Group", action = "List" },
                namespaces: new[] { "OKComplex.Controllers" }
            );
            routes.MapRoute(
                name: "Search",
                url: "search",
                defaults: new { area = "", controller = "Post", action = "Search", page = 1 },
                namespaces: new[] { "OKComplex.Controllers" }
            );
            routes.MapRoute(
                name: "Group",
                url: "group/{seo}/{action}/{page}",
                defaults: new { area = "", controller = "Group", action = "Detail", seo = UrlParameter.Optional, page = UrlParameter.Optional },
                namespaces: new[] { "OKComplex.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { area = "", controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OKComplex.Controllers" }
            );
        }
    }
}