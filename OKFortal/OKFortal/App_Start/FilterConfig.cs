using OKFortal.Functions;
using System.Web;
using System.Web.Mvc;

namespace OKFortal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}