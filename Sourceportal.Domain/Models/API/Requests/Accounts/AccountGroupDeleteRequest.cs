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
    public class AccountGroupDeleteRequest
    {
        [DataMember(Name = "accountGroup")]
        public AccountGroup AccountGroup { get; set; }
    }
}
