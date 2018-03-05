using System.Web;
using System.Web.Mvc;

namespace EHSApps.WebAPI.JSSE
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
