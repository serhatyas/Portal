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
    
    public partial class HolidayTable
    {
        public int ID { get; set; }
        public Nullable<int> MinWorkDays { get; set; }
        public Nullable<int> MaxWorkDays { get; set; }
        public Nullable<int> MaxAge { get; set; }
        public Nullable<int> MinAge { get; set; }
        public Nullable<int> NoDays { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> IsPassive { get; set; }
    }
}
