using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    public class ContactFocusesGetResponse
    {
        [DataMember(Name = "contactFocusMaps")]
        public List<ContactFocusMap> ContactFocusMaps { get; set; }
        [DataMember(Name = "contactFocusOptions")]
        public List<ContactFocusOption> ContactFocusOptions { get; set; }
    }


    [DataContract]
    public class ContactFocusMap
    {
        [DataMember(Name = "focusId")]
        public int FocusID { get; set; }
        [DataMember(Name = "focusName")]
        public string FocusName { get; set; }
        [DataMember(Name = "objectName")]
        public string ObjectName { get; set; }
    }

    public class ContactFocusOption : ContactFocusMap { }
}
