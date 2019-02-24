using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace OKFortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Index",
                url: "forum",
                defaults: new { area = "", controller = "Forum", action = "Index" }
            );
            routes.MapRoute(
                name: "Category",
                url: "forum/{id}c_{seo}",
                defaults: new { area = "", controller = "Forum", action = "Category", seo = RouteParameter.Optional, id = RouteParameter.Optional }
            );
            routes.MapRoute(
                name: "Forum",
                url: "forum/{id}f{page}_{seo}",
                defaults: new { area = "", controller = "Forum", action = "Forum", seo = RouteParameter.Optional, id = RouteParameter.Optional, page = RouteParameter.Optional }
            );
            routes.MapRoute(
                name: "Topic",
                url: "forum/{id}t{page}_{seo}",
                defaults: new { area = "", controller = "Forum", action = "Topic", seo = RouteParameter.Optional, id = RouteParameter.Optional, page = RouteParameter.Optional }
            );
            routes.MapRoute(
                name: "GoTopic",
                url: "go/t{id}",
                defaults: new { area = "", controller = "Forum", action = "GoTopic", id = RouteParameter.Optional }
            );
            routes.MapRoute(
                name: "GoComment",
                url: "go/c{id}",
                defaults: new { area = "", controller = "Forum", action = "GoComment", id = RouteParameter.Optional }
            );
            routes.MapRoute(
                name: "Messages",
                url: "messages/{username}",
                defaults: new { area = "", controller = "Account", action = "MessageBox", username = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Settings",
                url: "settings",
                defaults: new { area = "", controller = "Account", action = "Settings" }
            );
            routes.MapRoute(
                name: "Account",
                url: "user/{username}/{action}/{page}",
                defaults: new { area = "", controller = "Account", action = "Info", username = UrlParameter.Optional, page = 1 }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{page}",
                defaults: new { area = "", controller = "Home", action = "Index", page = 1 }
            );
        }
    }
}