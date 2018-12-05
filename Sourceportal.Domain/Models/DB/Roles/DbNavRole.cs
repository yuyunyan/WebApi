using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Roles
{
    public class DbNavRole
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int? UserRoleID { get; set; }
        public int? IsDeleted { get; set; }
    }
}
