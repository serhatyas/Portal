using Portal.Enums;
using Portal.Models.EntityFramework;
using Portal.Models.ServiceModelDB;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Portal.Helper;

namespace Portal.Controllers
{
    public class DataProcessingController : Controller
    {
        string PageGuid = "BLG1";
        // GET: DataProcessing
        public ActionResult Index()
        {
            using (PortalDB db = new PortalDB())
            {
                if (UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid,Enums.AuthorizationTypes.IsRead) == true)
                {
                    ServiceModel Model = new ServiceModel();

                    var ContextID = Convert.ToInt32(HttpContext.User.Identity.Name);
                    Model.SystemUser = db.SystemUser.FirstOrDefault(x => x.ID == ContextID);
                    if (Model.SystemUser.IsSuperAdmin == true)
                    {
                        Model.inventoryTransactionValueMapsList = db.InventoryTransactionValueMap.Where(x => x.IsPassive == false).ToList();
                        Model.inventoryTransactionsList = db.InventoryTransactions.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreateDate).ToList();
                    }
                    else
                    {
                        Model.inventoryTransactionsList = db.InventoryTransactions.Where(x => x.IsPassive == false && x.BranchId == Model.SystemUser.BranchID).ToList();
                    }
                    return View(Model);
                }
                else
                {
                    return Redirect("/yetkisiz-erisim");
                }
            }
        }

        [HttpGet]
        public ActionResult details(string inventoryCode)
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel Model = new ServiceModel();
                Model.inventoryTransactions = db.InventoryTransactions.FirstOrDefault(x => x.InventoryCode == inventoryCode);
                Model.inventoryTransactionValueMapsList = db.InventoryTransactionValueMap.Where(x =>x.InventoryTransactionId == Model.inventoryTransactions.ID).ToList();
                Model.ValueList = db.Value.ToList();
                Model.TypeList = db.Types.ToList();
                return View(Model);
            }
        }

        [HttpGet]
        public ActionResult Save(int id = 0)
        {
            using (PortalDB db = new PortalDB())
            {
                //if (UserAuthentication.IsControl(HttpContext.User.Identity.Name,PageGuid,AuthorizationTypes.IsCreate)==true)
                //{

                //}
                ServiceModel model = new ServiceModel();
                model.SystemUserList = db.SystemUser.Where(x => x.IsPassive == false).ToList();
                model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
                model.inventoryTransactionsList = db.InventoryTransactions.Where(x => x.IsPassive == false).ToList();
                model.inventoryTransactionValueMapsList = new List<InventoryTransactionValueMap>();
                model.ValueList = db.Value.ToList();
                model.TypeList = db.Types.ToList();
                if (id != 0)
                {
                    model.ValueList = db.Value.ToList();
                    model.TypeList = db.Types.ToList();
                    model.SystemUserList = db.SystemUser.Where(x => x.IsPassive == false).ToList();
                    model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
                    model.inventoryTransactions = db.InventoryTransactions.FirstOrDefault(x => x.ID == id);
                    model.inventoryTransactionValueMapsList = db.InventoryTransactionValueMap.Where(x => x.InventoryTransactionId == model.inventoryTransactions.ID).ToList();

                }
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult save(ServiceModel model, HttpPostedFileBase file)
        {
            using (PortalDB db = new PortalDB())
            {
                int Logging = Convert.ToInt32(User.Identity.Name);
                var LoggingInBranch = db.SystemUser.FirstOrDefault(x => x.ID == Logging);

                ServiceModel Model = new ServiceModel();

                string path = "";
                if (file != null)
                {
                    string filePath = System.IO.Path.Combine(Server.MapPath("/uploads/images/") + file.FileName);
                    file.SaveAs(filePath);
                    path = "/uploads/images/" + file.FileName;
                }

                if (model.inventoryTransactions.ID == 0)
                {
                    Random rnd = new Random();
                    model.inventoryTransactions.InventoryCode = "OZYER" + DateTime.Now.ToString().Replace(".", "").Replace("-", "").Replace(" ", "").Replace(":", "");
                    model.inventoryTransactions.Path = path;
                    model.inventoryTransactions.UpdateDate = DateTime.Now;
                    model.inventoryTransactions.CreateDate = DateTime.Now;
                    model.inventoryTransactions.IsPassive = false;
                    model.inventoryTransactions.IsDeleted = false;
                    model.inventoryTransactions.CreateUser = User.Identity.Name;
                    model.inventoryTransactions.BranchId = LoggingInBranch.BranchID;
                    db.InventoryTransactions.Add(model.inventoryTransactions);
                    db.SaveChanges();

                    foreach (var item in model.inventoryTransactionValueMapsList)
                    {
                        if (item.ValueId != null)
                        {
                            var value = db.Value.FirstOrDefault(x => x.ID == item.ValueId);
                            var type = db.Types.FirstOrDefault(x => x.ID == value.TypeID);
                            InventoryTransactionValueMap ivmp = new InventoryTransactionValueMap();
                            ivmp.Date = DateTime.Now;
                            ivmp.InventoryTransactionId = model.inventoryTransactions.ID;
                            ivmp.IsDeleted = false;
                            ivmp.IsPassive = false;
                            ivmp.TypeName = type.Name.Trim();
                            ivmp.ValueId = value.ID;
                            db.InventoryTransactionValueMap.Add(ivmp);
                            db.SaveChanges();
                        }

                    }

                    return Redirect("/DataProcessing/Index");
                }
                else
                {
                    var updateArea = db.InventoryTransactions.FirstOrDefault(x => x.ID == model.inventoryTransactions.ID);
                    if (updateArea == null)
                    {
                        return HttpNotFound();
                    }
                    updateArea.IsPassive = false;
                    updateArea.IsDeleted = false;
                    updateArea.InIp = model.inventoryTransactions.InIp;
                    updateArea.OutIp = model.inventoryTransactions.OutIp;
                    updateArea.TopIp = model.inventoryTransactions.TopIp;
                    updateArea.BuyDate = model.inventoryTransactions.BuyDate;
                    updateArea.InvoiceNo = model.inventoryTransactions.InvoiceNo;
                    updateArea.InventoryCode = model.inventoryTransactions.InventoryCode;
                    updateArea.InventoryName = model.inventoryTransactions.InventoryName;
                    updateArea.UniversalIdentity = model.inventoryTransactions.UniversalIdentity;
                    updateArea.InventoryMacAddress = model.inventoryTransactions.InventoryMacAddress;
                    updateArea.UpdateDate = DateTime.Now;

                    var valueMapList = db.InventoryTransactionValueMap.Where(x => x.InventoryTransactionId == updateArea.ID).ToList();
                    db.InventoryTransactionValueMap.RemoveRange(valueMapList);
                    db.SaveChanges();

                    foreach (var item in model.inventoryTransactionValueMapsList)
                    {
                        if (item.ValueId != null)
                        {
                            var value = db.Value.FirstOrDefault(x => x.ID == item.ValueId);
                            var type = db.Types.FirstOrDefault(x => x.ID == value.TypeID);
                            InventoryTransactionValueMap ivmp = new InventoryTransactionValueMap();
                            ivmp.Date = DateTime.Now;
                            ivmp.InventoryTransactionId = updateArea.ID;
                            ivmp.IsDeleted = false;
                            ivmp.IsPassive = false;
                            ivmp.TypeName = type.Name.Trim();
                            ivmp.ValueId = value.ID;
                            db.InventoryTransactionValueMap.Add(ivmp);
                            db.SaveChanges();
                        }

                    }

                    db.SaveChanges();

                    return Redirect("/DataProcessing/Index");
                }

            }
        }

        public ActionResult delete(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                var currentData = db.InventoryTransactions.FirstOrDefault(x => x.ID == id);
                if (currentData != null)
                {
                    var valueMapList = db.InventoryTransactionValueMap.Where(x => x.InventoryTransactionId == currentData.ID).ToList();
                    db.InventoryTransactionValueMap.RemoveRange(valueMapList);
                    db.SaveChanges();
                    currentData.IsDeleted = true;
                    db.SaveChanges();
                    return Json("Basarili", JsonRequestBehavior.AllowGet);
                }
                return View("/DataProcessing/Index");
            }
        }

        public ActionResult isPassive(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                var currentItem = db.InventoryTransactions.FirstOrDefault(x => x.ID == id);
                if (currentItem.IsPassive == true)
                    currentItem.IsPassive = false;

                else
                    currentItem.IsPassive = true;
                db.SaveChanges();
                return Json("Basarili", JsonRequestBehavior.AllowGet);
            }
        }

    }
}