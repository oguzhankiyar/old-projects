using System.Web.Mvc;

namespace OKFortal.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin",
                "admin",
                new { area = "Admin", controller = "Statistics", action = "Index" }
            );
            context.MapRoute(
                "AdminDefault",
                "admin/{controller}/{action}/{num}",
                new { area = "Admin", num = 1 }
            );
        }
    }
}
