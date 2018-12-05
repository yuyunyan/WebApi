using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class AccountHierarchyDb
    {
        public int AccountHierarchyID { get; set; }
        public int? ParentID { get; set; }
        public int RegionID { get; set; }
        public string HierarchyName { get; set; }
        public string SAPHierarchyID { get; set; }
        public string SAPGroupID { get; set; }
    }
}
