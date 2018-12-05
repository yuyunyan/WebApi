using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.SalesOrders
{
    [DataContract]
   public class AccountsResponse
    {
        [DataMember(Name = "accounts")]
        public List<Account> Accounts;
    }

    [DataContract]
    public class Account
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "typeId")]
        public int TypeId { get; set; }
       
    }
}
