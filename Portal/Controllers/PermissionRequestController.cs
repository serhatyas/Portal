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
    public class PermissionRequestController : Controller
    {
        string PageGuid = "TLB2";
        // GET: PermissionRequest
        public ActionResult Index()
        {
            using (PortalDB db = new PortalDB())
            {
                if (UserAuthentication.IsControl(HttpContext.User.Identity.Name, PageGuid, Enums.AuthorizationTypes.IsRead))
                {
                    ServiceModel Model = new ServiceModel();
                    Model.permissionRequestFormsList = db.PermissionRequestForm.Where(x => x.IsPassive == false).ToList();
                    Model.permissionTypesList = db.PermissionType.ToList();
                    Model.permissionStatusList = db.PermissionStatus.ToList();
                    return View(Model);
                }
                else return Redirect("/yetkisiz-erisim");
            }

        }

        [HttpGet]
        public ActionResult Save(int id = 0)
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.permissionRequestFormsList = db.PermissionRequestForm.ToList();
                model.permissionTypesList = db.PermissionType.Where(x => x.IsPassive == false).ToList();
                model.permissionStatusList = db.PermissionStatus.Where(x => x.IsPassive == false).ToList();
                model.permissionRequestForm = new PermissionRequestForm
                {
                    DateStart = DateTime.Today,
                    DateFinish = DateTime.Today.AddDays(1)
                };

                if (id != 0)
                {
                    model.permissionRequestFormsList = db.PermissionRequestForm.ToList();
                    model.permissionTypesList = db.PermissionType.Where(x => x.IsPassive == false).ToList();
                    model.permissionStatusList = db.PermissionStatus.Where(x => x.IsPassive == false).ToList();
                    model.permissionRequestForm = db.PermissionRequestForm.FirstOrDefault(x => x.ID == id);
                }
               
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(ServiceModel model)
        {
            using (PortalDB db = new PortalDB())
            {
                var contextID = Convert.ToInt32(HttpContext.User.Identity.Name);
                var Logging = db.SystemUser.FirstOrDefault(x => x.ID == contextID);

                ServiceModel Model = new ServiceModel();
                if (model.permissionRequestForm.ID == 0)
                {
                    model.permissionRequestForm.Date = DateTime.Now;
                    model.permissionRequestForm.IsPassive = false;
                    model.permissionRequestForm.UserID = Logging.ID;
                    model.permissionRequestForm.BranchID = Logging.BranchID;
                    db.PermissionRequestForm.Add(model.permissionRequestForm);
                }
                else
                {
                    var updatePermission = db.PermissionRequestForm.FirstOrDefault(x => x.ID == model.permissionRequestForm.ID);
                    if (updatePermission == null)
                    {
                        return HttpNotFound();
                    }
                    updatePermission.DateStart = model.permissionRequestForm.DateStart;
                    updatePermission.DateFinish = model.permissionRequestForm.DateFinish;
                    updatePermission.Description = model.permissionRequestForm.Description;
                    updatePermission.PermissionStatusID = model.permissionRequestForm.PermissionStatusID;
                    updatePermission.PermissionTypeID = model.permissionRequestForm.PermissionTypeID;
                    updatePermission.Date = DateTime.Now;

                }
                db.SaveChanges();

                MailManager mailManager = new MailManager();
                var user = db.SystemUser.FirstOrDefault(x => x.ID == model.permissionRequestForm.UserID); // Kullanıcıyı bulduk
                var userDepartment = db.SeniorDepartment.FirstOrDefault(x => x.DepartmentID == user.DepartmanID); // Kullanıcının kayıtlı olduğu departmanı bulduk
                var departmenSenior = db.SystemUser.FirstOrDefault(x => x.ID == userDepartment.SystemUserID); // Kullanıcının kayıtlı olduğu departman şef bilgilerini bulduk
                string mailTemplate ="İzin Açıklaması: "+ model.permissionRequestForm.Description+" İzin talebi detayları için aşağıdaki linke tıklayınız. https://localhost:44339/permissionrequest/approved/" + model.permissionRequestForm.ID; // Mail Template oluşturuldu
                mailManager.SendSingleMail(departmenSenior.Mail, "Özyer Group", "İzin Talebi", mailTemplate); // Oluşturulan template şefe mail olarak gönderildi.
                return Redirect("/PermissionRequest/Index");
            }

        }

        [HttpGet]
        public ActionResult Approved(int id)
        {
            using(PortalDB db=new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.permissionRequestForm = db.PermissionRequestForm.FirstOrDefault(x => x.ID == id);
                model.SystemUser = db.SystemUser.FirstOrDefault(x => x.ID == model.permissionRequestForm.UserID);
                model.permissionTypesList = db.PermissionType.ToList();
                model.permissionStatusList = db.PermissionStatus.ToList();
            return View(model);
            }
        }

        [HttpGet]
        public ActionResult ApprovedSuccess(int permissionid,int result)
        {
            using (PortalDB db = new PortalDB())
            {
                var permissionRequest = db.PermissionRequestForm.FirstOrDefault(x => x.ID == permissionid);
                permissionRequest.IsApproved = Convert.ToBoolean(result);
                permissionRequest.ApprovedDate = DateTime.Now;
                db.SaveChanges();
                return Redirect("/permissionrequest");
            }
        }
        public ActionResult Delete(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                var currentItem = db.PermissionRequestForm.FirstOrDefault(x => x.ID == id);
                if (currentItem != null)
                {
                    currentItem.IsPassive = true;
                    db.SaveChanges();
                    return Json("basarili", JsonRequestBehavior.AllowGet);
                }
                return View();
            }
        }
    }
}