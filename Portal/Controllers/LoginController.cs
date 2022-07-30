using Portal.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            using (PortalDB db = new PortalDB())
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(SystemUser systemUser)
        {
            using (PortalDB db=new PortalDB())
            {
                var CurrentUser = db.SystemUser.FirstOrDefault(x => x.Mail == systemUser.Mail && x.Password == systemUser.Password);
                if(CurrentUser!=null)
                {
                    FormsAuthentication.SetAuthCookie(CurrentUser.ID.ToString(), true);
                    return Redirect("/DataProcessing/Index");
                }
                ViewBag.Hata = "Kullanıcı adınız veya şifreniz hatalı";
                return View();
            }
        }
    }
}