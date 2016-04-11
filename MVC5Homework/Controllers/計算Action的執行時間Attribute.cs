using System;
using System.Web.Mvc;

namespace MVC5Homework.Controllers
{
    internal class 計算Action的執行時間Attribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.dtStart = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
           TimeSpan dtSpan = DateTime.Now - filterContext.Controller.ViewBag.dtStart;
           System.Diagnostics.Debug.WriteLine("執行時間:"+ dtSpan);
            base.OnActionExecuted(filterContext);
        }
    }
}