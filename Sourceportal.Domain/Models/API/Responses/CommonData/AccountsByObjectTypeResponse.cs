using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
   public class AccountsByObjectTypeResponse
    {
       [DataMember(Name = "accounts")]
       public List<ObjectTypeAccount> Accounts;
    }

    [DataContract]
    public class ObjectTypeAccount
    {
        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "accountTypeId")]
        public int AccountTypeId { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "supplierRating")]
        public string SupplierRating { get; set; }

    }
}
