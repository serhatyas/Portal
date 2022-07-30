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
    public class InventoryRequestController : Controller
    {
        string PageGuid = "TLB3";

        // GET: InventoryRequest
        public ActionResult Index() // Read
        {
            using (PortalDB db = new PortalDB())
            {

                var contextID = Convert.ToInt32(HttpContext.User.Identity.Name);
                var Logging = db.SystemUser.FirstOrDefault(x => x.ID == contextID);

                if (UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid, Enums.AuthorizationTypes.IsRead) == true)
                {
                    ServiceModel model = new ServiceModel();

                    if (Logging.IsSuperAdmin == true)
                    {
                        model.inventoryRequestsList = db.InventoryRequest.Where(x => x.IsPassive == false).ToList();
                    }
                    else
                    {
                        model.inventoryRequestsList = db.InventoryRequest.Where(x => x.IsPassive == false && x.BranchID == Logging.BranchID).ToList();
                    }
                    model.ValueList = db.Value.Where(x => x.IsPassive == false).ToList();
                    return View(model);
                }
                else
                {
                    return Redirect("/yetkisiz-erisim");
                }
            }

        }

        [HttpGet]
        public ActionResult Save(int id = 0) // IsCreate  // IsUpdate
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.inventoryRequestsList = db.InventoryRequest.Where(x => x.IsPassive == false).ToList();
                model.ValueList = db.Value.Where(x => x.IsPassive == false).ToList();
                model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
                if (id != 0)
                {
                    model.inventoryRequestsList = db.InventoryRequest.Where(x => x.IsPassive == false).ToList();
                    model.ValueList = db.Value.Where(x => x.IsPassive == false).ToList();
                    model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
                }
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Save(ServiceModel model) // IsCreate  // IsUpdate
        {
            using (PortalDB db = new PortalDB())
            {

                var contextID = Convert.ToInt32(HttpContext.User.Identity.Name);
                var Logging = db.SystemUser.FirstOrDefault(x => x.ID == contextID);

                ServiceModel Model = new ServiceModel();
                if (model.inventoryRequest.ID == 0)
                {
                    model.inventoryRequest.Date = DateTime.Now;
                    model.inventoryRequest.IsPassive = false;
                    model.inventoryRequest.ID = Logging.ID;
                    model.inventoryRequest.BranchID = Logging.BranchID;
                }
                else
                {
                    var updateArea = db.InventoryRequest.FirstOrDefault(x => x.ID == model.inventoryRequest.ID);
                    if (updateArea == null)
                    {
                        HttpNotFound();
                    }
                    else

                    {
                        updateArea.InventorySection = model.inventoryRequest.InventorySection;
                        updateArea.AddInventoryName = model.inventoryRequest.AddInventoryName;
                        updateArea.InventoryDescription = model.inventoryRequest.InventoryDescription;
                        updateArea.Description = model.inventoryRequest.Description;
                        updateArea.Date = DateTime.Now;
                    }
                }
                db.InventoryRequest.Add(model.inventoryRequest);
                db.SaveChanges();
                return Redirect("/InventoryRequest/Index");
            }

        }

        public ActionResult delete(int id) // IsDeleted
        {
            using (PortalDB db = new PortalDB())
            {
                var currentRequest = db.InventoryRequest.FirstOrDefault(x => x.ID == id);
                if (currentRequest != null)
                {
                    currentRequest.IsPassive = true;
                    db.SaveChanges();
                    return Json("Basarili", JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
    }
}