using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class AccountGroupListResponse
    {
        [DataMember(Name = "accountGroups")]
        public List<AccountGroup> AccountGroups { get; set; }
    }

    [DataContract]
    public class AccountGroup
    {
        [DataMember(Name = "accountGroupId")]
        public int AccountGroupID { get; set; }

        [DataMember(Name = "groupName")]
        public string GroupName { get; set; }
    }
}
