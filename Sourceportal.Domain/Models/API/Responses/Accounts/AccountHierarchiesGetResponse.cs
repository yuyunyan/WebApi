using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class AccountHierarchiesGetResponse
    {
        [DataMember(Name = "accountHierarchies")]
        public List<AccountHierarchyResponse> AccountHierarchies { get; set; }
    }

    [DataContract]
    public class AccountHierarchyResponse
    {
        [DataMember(Name = "accountHierarchyId")]
        public int AccountHierarchyID { get; set; }
        [DataMember(Name = "parentId")]
        public int? ParentID { get; set; }
        [DataMember(Name = "regionId")]
        public int RegionID { get; set; }
        [DataMember(Name = "hierarchyName")]
        public string HierarchyName { get; set; }
        [DataMember]
        public string SAPHierarchyID { get; set; }
        [DataMember]
        public string SAPGroupID { get; set; }
    }
}
