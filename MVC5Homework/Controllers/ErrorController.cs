using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Homework.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [HandleError(ExceptionType = typeof(ArgumentException), View = ("Error2"))]
        public ActionResult Index(string errType)
        {
            switch (errType)
            {
                case "custom":
                    throw new ArgumentException("參數錯誤!");
                    break;
                case "default":
                    throw new Exception("自訂錯誤");
                    break;
            }
            return View();
        }
    }
}