using PropertyManagesSystem.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Controllers
{
    [MyUserActionFilter]
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            ViewBag.UserName = Session["userName"].ToString();
            return View();
        }

        public ActionResult UserIndex()
        {
            return View();
        }
    }
}