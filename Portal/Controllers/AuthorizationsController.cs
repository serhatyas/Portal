using Portal.Models.EntityFramework;
using Portal.Models.ServiceModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Helper;

namespace Portal.Controllers
{
    public class AuthorizationsController : Controller
    {
        // GET: Authorizations
        public ActionResult Index()
        {
            using (PortalDB db=new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.authorizationsList = db.Authorizations.Where(x => x.IsPassive == false).ToList();
                model.pagesList = db.Pages.Where(x => x.IsPassive == false).ToList();
                model.modulesList = db.Modules.Where(x => x.IsPassive == false).ToList();
                model.SystemUserList = db.SystemUser.Where(x => x.IsPassive == false).ToList();
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult Save(int id = 0)
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.modulesList = db.Modules.Where(x => x.IsPassive == false).ToList();
                model.pagesList = db.Pages.Where(x => x.IsPassive == false).ToList();
                model.SystemUserList=db.SystemUser.Where(x => x.IsPassive == false).ToList();
                if (id!=0)
                {
                    model.modulesList = db.Modules.Where(x => x.IsPassive == false).ToList();
                    model.pagesList = db.Pages.Where(x => x.IsPassive == false).ToList();
                    model.SystemUserList = db.SystemUser.Where(x => x.IsPassive == false).ToList();
                    model.authorizations = db.Authorizations.FirstOrDefault(x => x.ID == id);
                }
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Save(ServiceModel model)
        {
            using (PortalDB db=new PortalDB())
            {
                ServiceModel Model = new ServiceModel();
                if (model.authorizations.ID==0)
                {
                    model.authorizations.IsPassive = false;
                    model.authorizations.CreatedDateTime = DateTime.Now;
                    model.authorizations.UpdatedDateTime = DateTime.Now;
                    model.authorizations.CreatedUserID = Convert.ToInt32(User.Identity.Name);
                    db.Authorizations.Add(model.authorizations);
                }
                else
                {
                    var currentUpdate = db.Authorizations.FirstOrDefault(x => x.ID == model.authorizations.ID);
                    if (currentUpdate==null)
                    {
                        HttpNotFound();
                    }
                    currentUpdate.ModuleId = model.authorizations.ModuleId;
                    currentUpdate.PageID = model.authorizations.PageID;
                    currentUpdate.UserID = model.authorizations.UserID;
                    currentUpdate.IsRead = model.authorizations.IsRead;
                    currentUpdate.IsUpdate = model.authorizations.IsUpdate;
                    currentUpdate.IsDeleted = model.authorizations.IsDeleted;
                    currentUpdate.UpdatedDateTime = DateTime.Now;
                    currentUpdate.UpdateUserID = Convert.ToInt32(User.Identity.Name);
                }
                db.SaveChanges();
                return Redirect("/Authorizations/Index");
            }
        }

        public ActionResult delete(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                var currentAuth = db.Authorizations.FirstOrDefault(x => x.ID == id);
                if (currentAuth != null)
                {
                    currentAuth.IsPassive = true;
                    db.Authorizations.Remove(currentAuth);
                    db.SaveChanges();
                    return Json("Basarili", JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
    }
}