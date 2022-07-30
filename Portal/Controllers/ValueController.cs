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
    public class ValueController : Controller
    {
        string PageGuid = "BLG2";
        // GET: Value
        public ActionResult Index()
        {
            using (PortalDB db = new PortalDB())
            {
                if (UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid, Enums.AuthorizationTypes.IsRead))
                {
                    ServiceModel Model = new ServiceModel();
                    Model.ValueList = db.Value.Where(x => x.IsPassive == false).ToList();
                    Model.TypeList = db.Types.ToList();
                    return View(Model);
                }
                else
                    return Redirect("/yetkisiz-erisim");
            }
        }

        [HttpGet]
        public ActionResult Save(int id = 0)
        {
            using (PortalDB db = new PortalDB())
            {
                if (id == 0 && UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid, Enums.AuthorizationTypes.IsCreate))
                {
                    ServiceModel model = new ServiceModel();
                    model.inventoryTransactionValueMapsList = new List<InventoryTransactionValueMap>();
                    model.TypeList = db.Types.Where(x => x.IsPassive == false).ToList();
                    model.ValueList = db.Value.Where(x => x.IsPassive == false).ToList();
                    if (id != 0)
                    {
                        model.Value = db.Value.FirstOrDefault(x => x.ID == id);
                        model.Type = db.Types.FirstOrDefault(x => x.ID == id);
                    }
                    return View(model);
                }
                else if (id != 0 && UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid, Enums.AuthorizationTypes.IsUpdate))
                {
                    ServiceModel model = new ServiceModel();
                    model.inventoryTransactionValueMapsList = new List<InventoryTransactionValueMap>();
                    model.TypeList = db.Types.Where(x => x.IsPassive == false).ToList();
                    model.ValueList = db.Value.Where(x => x.IsPassive == false).ToList();
                    if (id != 0)
                    {
                        model.Value = db.Value.FirstOrDefault(x => x.ID == id);
                        model.Type = db.Types.FirstOrDefault(x => x.ID == id);
                    }
                    return View(model);
                }
                else
                    return Redirect("/yetkisiz-erisim");


            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(ServiceModel model)
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel Model = new ServiceModel();
                if (model.Value.ID == 0 && UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid, Enums.AuthorizationTypes.IsCreate))
                {
                    model.Value.Date = DateTime.Now;
                    model.Value.IsPassive = false;
                    model.Value.TypeID = model.Value.TypeID;
                    db.Value.Add(model.Value);


                }
                else
                {
                    if (!UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid, Enums.AuthorizationTypes.IsUpdate))
                        return Redirect("/yetkisiz-erisim");
                    var updateArea = db.Value.FirstOrDefault(x => x.ID == model.Value.ID);
                    if (updateArea == null)
                    {
                        return HttpNotFound();
                    }
                    updateArea.IsPassive = false;
                    updateArea.Name = model.Value.Name;
                    updateArea.TypeID = model.Value.TypeID;
                    updateArea.Date = DateTime.Now;

                }

                db.SaveChanges();
                return Redirect("/Value/Index");
            }

        }

        public ActionResult delete(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                if (UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid, Enums.AuthorizationTypes.IsDeleted))
                {
                    var currentValue = db.Value.FirstOrDefault(x => x.ID == id);
                    if (currentValue != null)
                    {
                        currentValue.IsPassive = true;
                        db.SaveChanges();
                        return Json("Basarili", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(null);

            }
            return View();
        }
    }
}