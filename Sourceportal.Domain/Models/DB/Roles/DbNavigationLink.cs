using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Roles
{
    public class DbNavigationLink
    {
        public int NavID { get; set; }
        public int? ParentNavID { get; set; }
        public string NavName { get; set; }
        public int IsDeleted { get; set; }
        public int? RoleID { get; set; }
    }
}
