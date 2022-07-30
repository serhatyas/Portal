using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult Index()
        {
            return View();
        }

        [Route("yetkisiz-erisim")]
        public ActionResult yetkisiz()
        {
            return View();
        }
    }
}