using Portal.Models.EntityFramework;
using Portal.Models.ServiceModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult Index()
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.departmentsList = db.Department.Where(x => x.IsPassive == false).ToList();
                model.seniorDepartmentList = db.SeniorDepartment.Where(x => x.IsPassive == false).ToList();
                model.SystemUserList = db.SystemUser.Where(x => x.IsPassive == false).ToList();
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult Create()
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.departmentsList = db.Department.Where(x => x.IsPassive == false).ToList();
                model.seniorDepartmentList = db.SeniorDepartment.Where(x => x.IsPassive == false).ToList();
                model.SystemUserList = db.SystemUser.Where(x => x.IsPassive == false).ToList();
                return View(model);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ServiceModel model) // view tarafından gelen data
        {
            using (PortalDB db = new PortalDB())
            {

                model.department.IsPassive = false;
                model.department.Date = DateTime.Now;
                db.Department.Add(model.department);
                db.SaveChanges();

                var department = db.Department.FirstOrDefault(x => x.ID == model.department.ID);
                var systemuser = db.SystemUser.FirstOrDefault(x => x.ID == model.seniorDepartment.SystemUserID);

                SeniorDepartment spMap = new SeniorDepartment(); //bellekte yer açma
                spMap.Date = DateTime.Now;
                spMap.SystemUserID = systemuser.ID;
                spMap.DepartmentID = department.ID;
                spMap.IsPassive = false;
                spMap.Date = DateTime.Now;
                db.SeniorDepartment.Add(spMap);
                db.SaveChanges();

                return Redirect("/Department");
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (PortalDB db = new PortalDB())
            {
                ServiceModel model = new ServiceModel();
                model.departmentsList = db.Department.Where(x => x.IsPassive == false).ToList();
                model.seniorDepartmentList = db.SeniorDepartment.Where(x => x.IsPassive == false).ToList();
                return View(model);
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Update(ServiceModel model)
        {

            using (PortalDB db = new PortalDB())
            {
                var CurrentDepartmant = db.Department.FirstOrDefault(x => x.ID == model.department.ID);
                if (CurrentDepartmant == null)
                {
                    return HttpNotFound();
                }
                CurrentDepartmant.Name = model.department.Name;

                var seniorMap = db.SeniorDepartment.Where(x => x.DepartmentID == CurrentDepartmant.ID).ToList();

                return Redirect("/Department");
            }

        }

    }
}