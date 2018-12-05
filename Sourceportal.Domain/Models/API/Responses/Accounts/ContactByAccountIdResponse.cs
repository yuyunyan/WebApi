using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
   public class ContactByAccountIdResponse
    {
        [DataMember(Name = "contacts")]
        public IList<Contacts> ContactsByAccountId;
    }

    [DataContract]
    public class Contacts
    {
        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "phone")]
        public string OfficePhone { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "isActive")]
        public int IsActive { get; set; }
    }
}
