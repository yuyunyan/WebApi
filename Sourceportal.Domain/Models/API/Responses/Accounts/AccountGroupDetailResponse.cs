using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class AccountGroupDetailResponse
    {
        [DataMember(Name = "accountGroupId")]
        public int AccountGroupId { get; set; }

        [DataMember(Name = "userId")]
        public int UserId { get; set; }

        [DataMember(Name = "groupName")]
        public string GroupName { get; set; }

        [DataMember(Name = "groupLines")]
        public List<AccountGroupLine> AccoutGroupLines { get; set; }
    }

    [DataContract]
    public class AccountGroupLine
    {
        [DataMember(Name = "groupLineId")]
        public int GroupLineId { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "accountTypes")]
        public IList<string> AccountTypes { get; set; }

        [DataMember(Name = "accountStatusId")]
        public int AccountStatusId { get; set; }

        [DataMember(Name = "accountStatus")]
        public string AccountStatus { get; set; }

        [DataMember(Name = "contactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name="phone")]
        public string Phone { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "isDeleted")]
        public int? IsDeleted { get; set; }
    }
}
