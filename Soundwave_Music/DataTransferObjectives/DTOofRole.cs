using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soundwave_Music.DataTransferObjectives
{
    public class DTOofRole
    {
        public List<CheckPermissionToRole> rolePermission { get; set; }
        public int Role_id { get; set; }
        public string Role_name { get; set; }
        public int Count_account_role { get; set; }
        public int Permission_id { get; set; }
        public string Permission_name { get; set; }

        public class CheckPermissionToRole
        {
            public int Role_id { get; set; }
            public int Permission_id { get; set; }
            public string Permission_name { get; set; }
            public bool Checked_permission { get; set; }
        }
    }
}