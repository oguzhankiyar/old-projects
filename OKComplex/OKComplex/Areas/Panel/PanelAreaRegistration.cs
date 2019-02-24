using System.Web.Mvc;

namespace OKComplex.Areas.Panel
{
    public class PanelAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Panel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Panel_default",
                "panel/{controller}/{action}/{num}",
                new { controller = "Home", action = "Index", num = 1 },
                new[] { "OKComplex.Areas.Panel.Controllers" }
            );
        }
    }
}
