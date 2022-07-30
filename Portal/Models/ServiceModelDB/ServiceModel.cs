using Portal.Enums;
using Portal.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.ServiceModelDB
{
    public class ServiceModel
    {
        public InventoryTransactions inventoryTransactions { get; set; }
        public List<InventoryTransactions> inventoryTransactionsList { get; set; }
        public InventoryTransactionValueMap inventoryTransactionValueMap { get; set; }
        public List<InventoryTransactionValueMap> inventoryTransactionValueMapsList { get; set; }
        public HttpPostedFile file { get; set; }
        public Personel Personel { get; set; }
        public List<Personel> PersonelList { get; set; }
        public Portal.Models.EntityFramework.Value Value { get; set; }
        public List<Portal.Models.EntityFramework.Value> ValueList { get; set; }
        public Portal.Models.EntityFramework.Types Type { get; set; }
        public List<Portal.Models.EntityFramework.Types> TypeList { get; set; }
        public Portal.Models.EntityFramework.SystemUser SystemUser { get; set; }
        public List<Portal.Models.EntityFramework.SystemUser> SystemUserList { get; set; }
        public Portal.Models.EntityFramework.PermissionRequestForm permissionRequestForm { get; set; }
        public List<PermissionRequestForm> permissionRequestFormsList { get; set; }
        public Portal.Models.EntityFramework.PermissionStatus permissionStatus { get; set; }
        public List<Portal.Models.EntityFramework.PermissionStatus> permissionStatusList { get; set; }
        public Portal.Models.EntityFramework.PermissionType permissionType { get; set; }
        public List<Portal.Models.EntityFramework.PermissionType> permissionTypesList { get; set; }
        public Portal.Models.EntityFramework.AdvanceRequestForm advanceRequestForm { get; set; }
        public List<Portal.Models.EntityFramework.AdvanceRequestForm> advanceRequestsList { get; set; }
        public Portal.Models.EntityFramework.AdvanceType advanceType { get; set; }
        public List<Portal.Models.EntityFramework.AdvanceType> advanceTypeList { get; set; }
        public InventoryRequest inventoryRequest { get; set; }
        public List<InventoryRequest> inventoryRequestsList { get; set; }
        public Portal.Models.EntityFramework.Department department { get; set; }
        public List<Portal.Models.EntityFramework.Department> departmentsList { get; set; }
        public Portal.Models.EntityFramework.Authorizations authorizations { get; set; }
        public List<Portal.Models.EntityFramework.Authorizations> authorizationsList { get; set; }
        public Portal.Models.EntityFramework.Modules modules { get; set; }
        public List<Portal.Models.EntityFramework.Modules> modulesList { get; set; }
        public Portal.Models.EntityFramework.Pages pages { get; set; }
        public List<Portal.Models.EntityFramework.Pages> pagesList { get; set; }
        public Portal.Models.EntityFramework.BranchRole branchRole { get; set; }
        public List<Portal.Models.EntityFramework.BranchRole> branchRoleList{ get; set; }
        public Portal.Models.EntityFramework.Branch branch { get; set; }
        public List<Portal.Models.EntityFramework.Branch> branchList { get; set; }
        public Portal.Models.EntityFramework.WhichGroup whichGroup { get; set; }
        public List<Portal.Models.EntityFramework.WhichGroup> whichGroupsList { get; set; }
        public Portal.Models.EntityFramework.HolidayTable holidayTable { get; set; }
        public List<Portal.Models.EntityFramework.HolidayTable> holidayTablesList { get; set; }
        public Portal.Models.EntityFramework.SeniorDepartment seniorDepartment { get; set; }
        public List<Portal.Models.EntityFramework.SeniorDepartment> seniorDepartmentList { get; set; }



    }
}