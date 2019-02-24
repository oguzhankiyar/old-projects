using System.Web.Mvc;

namespace BGK.Areas.Admin
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
                "Admin_default",
                "admin/{controller}/{action}/{num}",
                new { controller = "Shared", action = "Index", num = 1 },
                new[] { "BGK.Areas.Admin.Controllers" }
            );
        }
    }
}
