using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class ModuleRoles
    {
        public static List<ModuleRole> roles = new List<ModuleRole>();

        public ModuleRoles()
        {
            roles.Add(new ModuleRole { });
        }
    }

    public class ModuleRole
    {
        public string Modul { get; set; }
        public string AltModul { get; set; }
        public bool Read { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}