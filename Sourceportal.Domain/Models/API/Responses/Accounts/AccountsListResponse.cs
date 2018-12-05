using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
   public class AccountsListResponse
    {
        [DataMember(Name = "accounts")]
        public IList<AllAccounts> AccountsList { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount { get; set; }
    }
    [DataContract]
    public class AllAccounts
    {
        [DataMember(Name = "accountId")]
        public string AccountId { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "accountNum")]
        public String AccountNum { get; set; }

        [DataMember(Name = "accountType")]
        public string AccountType { get; set; }

        [DataMember(Name = "countryName")]
        public string CountryName { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "organization")]
        public string Organization { get; set; }

        [DataMember(Name = "owner")]
        public string Owners { get; set; }

        [DataMember(Name = "accountStatus")]
        public string AccountStatus { get; set; }

        [DataMember(Name = "totalContactCount")]
        public string TotalContactCount { get; set; }

        [DataMember(Name = "accountNameAndNum")]
        public string AccountNameAndNum { get; set; }

        [DataMember(Name = "contactId")]
        public string ContactID { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "phone")]
        public string Phone { get; set; }

    }
}
