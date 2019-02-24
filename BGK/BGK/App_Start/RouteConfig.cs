using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGK
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Page",
                url: "p/{seo}",
                defaults: new { controller = "Page", action = "Details", seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Communication",
                url: "iletisim",
                defaults: new { controller = "Home", action = "Communication" },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Polls",
                url: "anketler/{page}",
                defaults: new { controller = "Poll", action = "Index", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Activities",
                url: "etkinlikler/{page}",
                defaults: new { controller = "Activity", action = "Index", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Activity_Details",
                url: "etkinlik/{id}",
                defaults: new { controller = "Activity", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Activity_Speaker",
                url: "etkinlik/konusmaci/{id}_{seo}",
                defaults: new { controller = "Activity", action = "Speaker", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Activity_Officer",
                url: "etkinlik/gorevli/{id}_{seo}",
                defaults: new { controller = "Activity", action = "Officer", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Groups",
                url: "gruplar/{page}",
                defaults: new { controller = "Group", action = "Index", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Group_Details",
                url: "grup/{id}_{seo}",
                defaults: new { controller = "Group", action = "Details", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Group_MemberDetails",
                url: "grup/uye/{id}_{seo}",
                defaults: new { controller = "Group", action = "MemberDetails", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Group_AddMember",
                url: "grup/uye-ekle/{id}",
                defaults: new { controller = "Group", action = "AddMember", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Group_EditMember",
                url: "grup/uye-duzenle/{id}",
                defaults: new { controller = "Group", action = "EditMember", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Group_GiveMission",
                url: "grup/gorev-ver/{id}",
                defaults: new { controller = "Group", action = "GiveMission", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Factories",
                url: "firmalar/{page}",
                defaults: new { controller = "Factory", action = "Index", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Factory_Details",
                url: "firma/{id}_{seo}",
                defaults: new { controller = "Factory", action = "Details", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Member_Messaging",
                url: "mesajlasma/{id}",
                defaults: new { controller = "Member", action = "Messaging", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Member_Messages",
                url: "mesajlarim/{page}",
                defaults: new { controller = "Member", action = "Messages", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Member_Groups",
                url: "uye/{id}_{seo}/gruplar/{page}",
                defaults: new { controller = "Member", action = "Groups", id = UrlParameter.Optional, page = 1, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Member_Posts",
                url: "uye/{id}_{seo}/yazilar/{page}",
                defaults: new { controller = "Member", action = "Posts", id = UrlParameter.Optional, page = 1, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Member_Comments",
                url: "uye/{id}_{seo}/yorumlar/{page}",
                defaults: new { controller = "Member", action = "Comments", id = UrlParameter.Optional, page = 1, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_MyMissions",
                url: "gorevlerim/{page}",
                defaults: new { controller = "Mission", action = "MyMissions", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_SubscribedCategories",
                url: "aboneliklerim",
                defaults: new { controller = "Mission", action = "SubscribedCategories" },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_CreatedMissions",
                url: "olusturdugum-gorevler/{page}",
                defaults: new { controller = "Mission", action = "CreatedMissions", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_List",
                url: "siradaki-gorevler/{page}",
                defaults: new { controller = "Mission", action = "List", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_MyCategories",
                url: "olusturdugum-kategoriler",
                defaults: new { controller = "Mission", action = "MyCategories"},
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_GiveMission",
                url: "kategori/gorev-ver/{id}",
                defaults: new { controller = "Mission", action = "GiveMission", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_Categories",
                url: "kategoriler",
                defaults: new { controller = "Mission", action = "Categories" },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_TakeMission",
                url: "gorev/al/{id}",
                defaults: new { controller = "Mission", action = "TakeMission", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_MissionMember",
                url: "gorev/uye/{id}",
                defaults: new { controller = "Mission", action = "MissionMember", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_Subscribe",
                url: "kategori/abone-ol/{id}",
                defaults: new { controller = "Mission", action = "Subscribe", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_CategoryDetails",
                url: "kategori/{id}",
                defaults: new { controller = "Mission", action = "CategoryDetails", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_Details",
                url: "gorev/{id}",
                defaults: new { controller = "Mission", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_AcceptMission",
                url: "gorev/kabul/{id}",
                defaults: new { controller = "Mission", action = "AcceptMission", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_RevertMission",
                url: "gorev/red/{id}",
                defaults: new { controller = "Mission", action = "RevertMission", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_ApproveMission",
                url: "gorev/onayla/{id}",
                defaults: new { controller = "Mission", action = "ApproveMission", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_UnapproveMission",
                url: "gorev/onaylama/{id}",
                defaults: new { controller = "Mission", action = "UnapproveMission", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Mission_CompleteMission",
                url: "gorev/teslim/{id}",
                defaults: new { controller = "Mission", action = "CompleteMission", id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Member_List",
                url: "uyeler/{page}",
                defaults: new { controller = "Member", action = "List", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Member_Details",
                url: "uye/{id}_{seo}",
                defaults: new { controller = "Member", action = "Details", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Login",
                url: "giris",
                defaults: new { controller = "Member", action = "Login" },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Gallery_View",
                url: "resim/{seo}/{id}",
                defaults: new { controller = "Gallery", action = "View", seo = UrlParameter.Optional, id = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Gallery_Images",
                url: "resimler/{seo}/{page}",
                defaults: new { controller = "Gallery", action = "Images", seo = UrlParameter.Optional, page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Gallery",
                url: "galeri/{page}",
                defaults: new { controller = "Gallery", action = "Index", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Document_Details",
                url: "dokuman/{id}_{seo}",
                defaults: new { controller = "Document", action = "Details", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Document_Category",
                url: "dokumanlar/{id}_{seo}/{page}",
                defaults: new { controller = "Document", action = "Category", id = UrlParameter.Optional, seo = UrlParameter.Optional, page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Documents",
                url: "dokumanlar/{page}",
                defaults: new { controller = "Document", action = "Index", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Search",
                url: "arama/{page}",
                defaults: new { controller = "Post", action = "Search", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Category",
                url: "k_{seo}/{page}",
                defaults: new { controller = "Post", action = "Category", seo = UrlParameter.Optional, page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Post_Create",
                url: "yazi-yaz",
                defaults: new { controller = "Post", action = "Create" },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "PopularPosts",
                url: "neler-okunuyor/{page}",
                defaults: new { controller = "Post", action = "Popular", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Posts",
                url: "blog/{page}",
                defaults: new { controller = "Post", action = "Index", page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Post",
                url: "yazi/{id}_{seo}/{page}",
                defaults: new { controller = "Post", action = "Details", id = UrlParameter.Optional, seo = UrlParameter.Optional, page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Error",
                url: "hata/{action}",
                defaults: new { controller = "Error" },
                namespaces: new[] { "BGK.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{page}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, page = 1 },
                namespaces: new[] { "BGK.Controllers" }
            );
        }
    }
}