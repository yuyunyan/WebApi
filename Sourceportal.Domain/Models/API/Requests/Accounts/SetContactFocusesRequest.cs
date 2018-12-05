using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    [DataContract]
    public class SetContactFocusesRequest
    {
        [DataMember(Name = "contactId")]
        public int ContactID { get; set; }
        [DataMember(Name = "focusId")]
        public int FocusID { get; set; }
        [DataMember(Name = "isDeleted")]
        public int IsDeleted { get; set; }
    }
}
