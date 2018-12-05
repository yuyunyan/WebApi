using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    [DataContract]
    public class AccountHierarchySapDetails
    {

        public int HierarchyId { get; set; }

        public string HierarchyName { get; set; }

        public string ExternalHierarchyId { get; set; }

        public string GroupId { get; set; }

        public int ParentId { get; set; }

        public int RegionId { get; set; }

    }
}
