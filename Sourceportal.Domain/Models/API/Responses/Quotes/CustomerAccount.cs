using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
    public class CustomerAccount
    {
        [DataMember(Name = "accountName")]
        public string Name { get; set; }
        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }
    }
}
