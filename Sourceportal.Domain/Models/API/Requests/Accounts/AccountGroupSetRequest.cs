using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.Accounts;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    [DataContract]
    public class AccountGroupSetRequest
    {
        [DataMember(Name = "accountGroupId")]
        public int? AccountGroupID { get; set; }

        [DataMember(Name = "groupName")]
        public string GroupName { get; set; }

        [DataMember(Name = "isDeleted")]
        public int IsDeleted { get; set; }

        [DataMember(Name = "accountGroupLines")]
        public IList<AccountGroupLine> AccountGroupLines { get; set; }
    }
}
