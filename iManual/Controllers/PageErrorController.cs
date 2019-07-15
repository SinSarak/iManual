using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iManual.Controllers
{
    public class PageErrorController : Controller
    {
        [HandleError]
        public ActionResult Index()
        {
            if (Response.StatusCode == 404.15)
            {
                return View("Error404_15");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        //[HandleError(ExceptionType = typeof(SqlException), View = "SqlExceptionView")]
        public ActionResult Error404()
        {

            return View();
        }

        public ActionResult Error404_15()
        {
            return View();
        }

        public ActionResult Error500()
        {
            return View();
        }

        public ActionResult AccessDenied(object message)
        {
            return View(message);
        }

        public ActionResult NotMatch(object message)
        {
            return View(message);
        }
        public ActionResult EmptyPage(object message)
        {
            return View(message);
        }
    }
}