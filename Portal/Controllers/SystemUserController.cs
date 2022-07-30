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
    public class SystemUserController : Controller
    {
        // GET: SystemUser
        public ActionResult Index()
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.SystemUserList = db.SystemUser.Where(x => x.IsPassive == false).ToList();
                model.departmentsList = db.Department.Where(x => x.IsPassive == false).ToList();
                model.pagesList = db.Pages.Where(x => x.IsPassive == false).ToList();

                
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Save(int id = 0)
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.departmentsList = db.Department.ToList();
                model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
                model.authorizationsList = new List<Authorizations>();
                model.pagesList = db.Pages.Where(x => x.IsPassive == false).ToList();
                model.modulesList = db.Modules.Where(x => x.IsPassive == false).ToList();
                model.whichGroupsList = db.WhichGroup.ToList();

                if (id != 0) // Guüncellemee Updaateee
                {
                    model.departmentsList = db.Department.ToList();
                    model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
                    model.SystemUser = db.SystemUser.FirstOrDefault(x => x.ID == id);
                    model.authorizationsList = db.Authorizations.Where(x => x.IsPassive == false && x.UserID == id).ToList();
                    model.pagesList = db.Pages.Where(x => x.IsPassive == false).ToList();
                    model.modulesList = db.Modules.Where(x => x.IsPassive == false).ToList();
                    model.whichGroupsList = db.WhichGroup.ToList();
                    foreach (var modul in db.Modules.Where(x => x.IsPassive == false && x.IsDeleted == false).ToList())
                    {
                        foreach (var page in db.Pages.Where(x => x.ModulId == modul.Id && x.IsPassive == false && x.IsDeleted == false).ToList())
                        {
                            var currentPage = model.authorizationsList.FirstOrDefault(x => x.ModuleId == modul.Id && x.PageID == page.ID);
                            if(currentPage==null)
                            {
                                Authorizations auth = new Authorizations();
                                auth.ModuleId = modul.Id;
                                auth.PageID = page.ID;
                                auth.IsCreate = false;
                                auth.IsDeleted = false;
                                auth.IsUpdate = false;
                                auth.IsRead = false;
                                model.authorizationsList.Add(auth);
                            }                          
                        }
                    }
                }
                else // Ekleme - Creaate
                {
                    foreach (var modul in db.Modules.Where(x => x.IsPassive == false && x.IsDeleted == false).ToList())
                    {
                        foreach (var page in db.Pages.Where(x => x.ModulId == modul.Id && x.IsPassive == false && x.IsDeleted == false).ToList())
                        {
                            Authorizations auth = new Authorizations();
                            auth.ModuleId = modul.Id;
                            auth.PageID = page.ID;
                            auth.IsCreate = false;
                            auth.IsDeleted = false;
                            auth.IsUpdate = false;
                            auth.IsRead = false;
                            model.authorizationsList.Add(auth);
                        }
                    }
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
                ServiceModel Model = new ServiceModel();
                if (model.SystemUser.ID == 0)
                {
                    model.SystemUser.Date = DateTime.Now;
                    model.SystemUser.IsPassive = false;
                    //model.SystemUser.PermissionNo = UserAuthentication.Main(Convert.ToDateTime(model.SystemUser.JopStartDate), Convert.ToDateTime(model.SystemUser.BirthDate));
                    db.SystemUser.Add(model.SystemUser);
                    db.SaveChanges();
                    model.authorizationsList.ForEach(x =>
                    {
                        x.UserID = model.SystemUser.ID;
                        x.CreatedDateTime = DateTime.Now;
                        x.UpdatedDateTime = DateTime.Now;
                        x.CreatedUserID = Convert.ToInt32(HttpContext.User.Identity.Name);
                        x.IsPassive = false;
                        x.IsDeleted = false;
                    });
                    db.Authorizations.AddRange(model.authorizationsList);
                    db.SaveChanges();

                }
                else
                {
                    var updateArea = db.SystemUser.FirstOrDefault(x => x.ID == model.SystemUser.ID);
                    if (updateArea == null)
                    {
                        HttpNotFound();
                    }
                    updateArea.Name = model.SystemUser.Name;
                    updateArea.Surname = model.SystemUser.Surname;
                    updateArea.Mail = model.SystemUser.Mail;
                    updateArea.Phone = model.SystemUser.Phone;
                    updateArea.Branch = model.SystemUser.Branch;
                    updateArea.WhichGroup = model.SystemUser.WhichGroup;
                    updateArea.Date = model.SystemUser.Date;
                    updateArea.Password = model.SystemUser.Password;
                    updateArea.DepartmanID = model.SystemUser.DepartmanID;
                    updateArea.Date = DateTime.Now;
                    updateArea.PermissionNo = model.SystemUser.PermissionNo;
                    //updateArea.PermissionNo = UserAuthentication.Main(Convert.ToDateTime(model.SystemUser.JopStartDate),Convert.ToDateTime( model.SystemUser.BirthDate));
                    var authList = db.Authorizations.Where(x => x.UserID == model.SystemUser.ID);
                    db.Authorizations.RemoveRange(authList);
                    db.SaveChanges();
                    model.authorizationsList.ForEach(x =>
                    {
                        x.UserID = model.SystemUser.ID;
                        x.CreatedDateTime = DateTime.Now;
                        x.UpdatedDateTime = DateTime.Now;
                        x.CreatedUserID = Convert.ToInt32(HttpContext.User.Identity.Name);
                        x.IsPassive = false;
                    });
                    db.Authorizations.AddRange(model.authorizationsList);

                }
                db.SaveChanges();
                return Redirect("/SystemUser/Index");
            }
        }

        public ActionResult Delete(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                var currentRequest = db.SystemUser.FirstOrDefault(x => x.ID == id);
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