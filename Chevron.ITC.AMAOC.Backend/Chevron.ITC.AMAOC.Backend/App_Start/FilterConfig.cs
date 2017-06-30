using System.Web;
using System.Web.Mvc;

namespace Chevron.ITC.AMAOC.Backend
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
