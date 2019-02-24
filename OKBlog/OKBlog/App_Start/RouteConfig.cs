using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OKBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "Home", action = "About" }
            );
            routes.MapRoute(
                name: "PostDetails",
                url: "post/{url}",
                defaults: new { controller = "Post", action = "Details", url = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Tags",
                url: "tags",
                defaults: new { controller = "Tag", action = "Index" }
            );
            routes.MapRoute(
                name: "TagDetails",
                url: "tag/{url}",
                defaults: new { controller = "Tag", action = "Details", url = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CategoryDetails",
                url: "category/{url}",
                defaults: new { controller = "Category", action = "Details", url = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Categories",
                url: "categories",
                defaults: new { controller = "Category", action = "Index" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

        }
    }
}