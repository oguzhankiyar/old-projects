using System.Web.Mvc;

namespace OKComplex.Areas.Admin
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
                new { controller = "Statistics", action = "Index", num = 1 },
                new[] { "OKComplex.Areas.Admin.Controllers" }
            );
        }
    }
}
