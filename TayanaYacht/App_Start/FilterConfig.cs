using System.Web;
using System.Web.Mvc;

namespace TayanaYacht
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //預設要登入才能使用網站
            filters.Add(new AuthorizeAttribute());
        }
    }
}
