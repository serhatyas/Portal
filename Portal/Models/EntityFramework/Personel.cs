//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Models.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Personel
    {
        public int ID { get; set; }
        public Nullable<int> DepartmanID { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<System.DateTime> StartWorkDate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> IsPassive { get; set; }
    
        public virtual Department Department { get; set; }
    }
}
