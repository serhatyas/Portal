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
    public class UsersController : Controller
    {
        string PageGuid = "USER";
        // GET: Users
        public ActionResult Index()
        {
            using (PortalDB db=new PortalDB())
            {
                var contextID = Convert.ToInt32(HttpContext.User.Identity.Name);
                var Logging = db.SystemUser.FirstOrDefault(x => x.ID == contextID);

                ServiceModel model = new ServiceModel();

                if (UserAuthentication.IsControl(HttpContext.User.Identity.Name,PageGuid,Enums.AuthorizationTypes.IsRead)==true)
                {
                    model.advanceRequestsList = db.AdvanceRequestForm.Where(x => x.IsPassive == false && x.UserID == Logging.ID).ToList();
                    model.permissionRequestFormsList = db.PermissionRequestForm.Where(x => x.IsPassive == false && x.UserID == Logging.ID).ToList();
                    model.inventoryRequestsList = db.InventoryRequest.Where(x => x.IsPassive == false && x.UserID == Logging.ID).ToList();
                    model.authorizationsList = db.Authorizations.Where(x => x.UserID == Logging.ID).ToList();
                    model.modulesList = db.Modules.ToList();
                    model.pagesList = db.Pages.ToList();
                    model.SystemUserList = db.SystemUser.ToList();
                    model.permissionTypesList = db.PermissionType.ToList();
                    model.permissionStatusList = db.PermissionStatus.ToList();
                    model.departmentsList = db.Department.ToList();
                    model.ValueList = db.Value.Where(x => x.IsPassive == false).ToList();
                    return View(model);
                }

                else
                {
                    return Redirect("/yetkisiz-erisim");
                }

            }

        }
    }
}