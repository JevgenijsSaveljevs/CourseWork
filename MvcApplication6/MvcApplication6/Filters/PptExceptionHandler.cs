using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace MvcApplication6.Filters
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PptExceptionHandler : HandleErrorAttribute
    {
        public PptExceptionHandler()
        {
            //Empty
        }
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is PptException)
                (filterContext.Exception as PptException).KillThread();

           // base.OnException(filterContext);
        }



    }
}