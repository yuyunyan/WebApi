using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Roles
{
    public class DbPermission
    {
        public int PermissionID { get; set; }
        public string ObjectTypeID { get; set; }
        public int IsDeleted { get; set; }
        public string PermName { get; set; }
        public int? RoleID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
