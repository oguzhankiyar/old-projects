using System.Web.Mvc;

namespace OKComplex.Areas.Forum
{
    public class ForumAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Forum";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Forum_Index",
                url: "forum",
                defaults: new { controller = "Forum", action = "Index" }
            );
            context.MapRoute(
                name: "Forum_Category",
                url: "forum/{id}c_{seo}",
                defaults: new { controller = "Forum", action = "Category", seo = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Forum_Forum",
                url: "forum/{id}f{page}_{seo}",
                defaults: new { controller = "Forum", action = "Forum", seo = UrlParameter.Optional, id = UrlParameter.Optional, page = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Forum_Topic",
                url: "forum/{id}t{page}_{seo}",
                defaults: new { controller = "Forum", action = "Topic", seo = UrlParameter.Optional, id = UrlParameter.Optional, page = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Forum_GoTopic",
                url: "go/t{id}",
                defaults: new { controller = "Forum", action = "GoTopic", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Forum_GoComment",
                url: "go/c{id}",
                defaults: new { controller = "Forum", action = "GoComment", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Forum_Messages",
                url: "account/messages/{username}",
                defaults: new { controller = "Account", action = "MessageBox", username = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "Forum_Settings",
                url: "account/settings",
                defaults: new { controller = "Account", action = "Settings" }
            );
            context.MapRoute(
                name: "Forum_Account",
                url: "user/{username}/{action}/{page}",
                defaults: new { controller = "Account", action = "Info", username = UrlParameter.Optional, page = 1 }
            );
            context.MapRoute(
                name: "Forum_Search",
                url: "forum/search/{page}",
                defaults: new { controller = "Forum", action = "Search", page = 1 }
            );
            context.MapRoute(
                name: "Forum_GSearch",
                url: "forum/gsearch/{page}",
                defaults: new { controller = "Forum", action = "GSearch", page = 1 }
            );
            context.MapRoute(
                name: "Forum_New",
                url: "forum/new/{page}",
                defaults: new { controller = "Forum", action = "New", page = 1 }
            );
            context.MapRoute(
                name: "Forum_Popular",
                url: "forum/popular/{page}",
                defaults: new { controller = "Forum", action = "Popular", page = 1 }
            );
            context.MapRoute(
                name: "Forum_Default",
                url: "forum/{controller}/{action}/{page}",
                defaults: new { controller = "Forum", action = "Index", page = 1 }
            );
        }
    }
}
