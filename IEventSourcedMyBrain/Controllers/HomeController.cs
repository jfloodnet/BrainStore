using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEventSourcedMyBrain.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    public class LiveController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
