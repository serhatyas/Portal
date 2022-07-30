using Portal.Helper;
using Portal.Models.EntityFramework;
using Portal.Models.ServiceModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class AdvanceRequestController : Controller
    {
        string PageGuid = "TLB2";

        // GET: AdvanceRequest
        public ActionResult Index()
        {
            using (PortalDB db= new PortalDB())
            {
                if(UserAuthentication.IsControl(HttpContext.User.Identity.Name,PageGuid,Enums.AuthorizationTypes.IsRead)==true)
                {
                    ServiceModel model = new ServiceModel();

                    var ContextID = Convert.ToInt32(HttpContext.User.Identity.Name);
                    model.SystemUser = db.SystemUser.FirstOrDefault(x => x.ID == ContextID);

                    if (model.SystemUser.IsSuperAdmin==true)
                    {
                        model.advanceRequestsList = db.AdvanceRequestForm.Where(x => x.IsPassive == false).ToList();
                        model.advanceTypeList = db.AdvanceType.Where(x => x.IsPassive == false).ToList();
                    }
                    else
                    {
                        model.advanceRequestsList = db.AdvanceRequestForm.Where(x => x.IsPassive == false && x.UserID == model.SystemUser.ID).ToList();
                        model.advanceTypeList = db.AdvanceType.Where(x => x.IsPassive == false).ToList();
                    }

                    return View(model);
                }
                else
                {
                    return Redirect("/yetkisiz-erisim");
                }
            }

        }

        [HttpGet]
        public ActionResult Save(int id=0)
        {
            using (PortalDB db =new PortalDB())
            {
                if (UserAuthentication.IsControl(HttpContext.User.Identity.Name,PageGuid,Enums.AuthorizationTypes.IsCreate))
                {

                }

                ServiceModel model = new ServiceModel();
                model.advanceRequestsList = db.AdvanceRequestForm.ToList();
                model.advanceTypeList = db.AdvanceType.ToList();
                if (id!=0)
                {
                    model.advanceRequestsList = db.AdvanceRequestForm.ToList();
                    model.advanceTypeList = db.AdvanceType.ToList();
                    model.advanceRequestForm = db.AdvanceRequestForm.FirstOrDefault(x => x.ID == id);
                }
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult Save(ServiceModel model)
        {
            using (PortalDB db=new PortalDB())
            {
                var contextID = Convert.ToInt32(HttpContext.User.Identity.Name);
                var Logging = db.SystemUser.FirstOrDefault(x => x.ID == contextID);

                ServiceModel Model = new ServiceModel();
                if (model.advanceRequestForm.ID==0)
                {
                    model.advanceRequestForm.Date = DateTime.Now;
                    model.advanceRequestForm.IsPassive = false;
                    model.advanceRequestForm.UserID = Logging.ID;
                    model.advanceRequestForm.BranchID = Logging.BranchID;
                    db.AdvanceRequestForm.Add(model.advanceRequestForm);
                }
                else
                {
                    var updateArea = db.AdvanceRequestForm.FirstOrDefault(x => x.ID == model.advanceRequestForm.ID);
                    if (updateArea==null)
                    {
                        HttpNotFound();
                    }
                    updateArea.Amount = model.advanceRequestForm.Amount;
                    updateArea.AdvanceDate = model.advanceRequestForm.AdvanceDate;
                    updateArea.Description = model.advanceRequestForm.Description;
                    updateArea.AdvanceTypeID = model.advanceRequestForm.AdvanceTypeID;
                    updateArea.Date = DateTime.Now;
                }
                db.SaveChanges();



                return Redirect("/AdvanceRequest/Index");
            }
        }

        public ActionResult delete(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                var currentARequest = db.AdvanceRequestForm.FirstOrDefault(x => x.ID == id);
                if (currentARequest != null)
                {
                    currentARequest.IsPassive = true;
                    db.SaveChanges();
                    return Json("Basarili", JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
    }
}