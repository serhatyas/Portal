using Portal.Models.EntityFramework;
using Portal.Models.ServiceModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class BranchController : Controller
    {
        // GET: Branch
        public ActionResult Index()
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
            }
            return View();
        }

        [HttpGet]

        public ActionResult Create()
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
                return View(model);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ServiceModel model)
        {
            using (PortalDB db = new PortalDB())
            {
                model.branch.IsPassive = false;
                model.branch.Date = DateTime.Now;

                db.Branch.Add(model.branch);
                db.SaveChanges();
            }
            return View();
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.branchList = db.Branch.Where(x => x.IsPassive == false).ToList();
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult Update(ServiceModel model)
        {
            using (PortalDB db = new PortalDB())
            {
                var currentBranch = db.Branch.FirstOrDefault(x => x.ID == model.branch.ID);
                if (currentBranch==null)
                {
                    return HttpNotFound();
                }

                currentBranch.Name = model.department.Name;
                db.SaveChanges();
                return Redirect("/Branch/Index");
            }

        }

        public ActionResult Delete(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                ////var currentDelete = db.PermissionRequestForm.FirstOrDefault(x => x.ID == id);
                ////if (currentItem != null)
                ////{
                ////    currentItem.IsPassive = true;
                ////    db.SaveChanges();
                ////    return Json("basarili", JsonRequestBehavior.AllowGet);
                //}

                return View();
            }
        }



    }
}