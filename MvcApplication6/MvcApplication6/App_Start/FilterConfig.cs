using MvcApplication6.Filters;
using MvcApplication6.Models;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication6
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());  
            filters.Add(new PptExceptionHandler());
                     
        }
    }
}