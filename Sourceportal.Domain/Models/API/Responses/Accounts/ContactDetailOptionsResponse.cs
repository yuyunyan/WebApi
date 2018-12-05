using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class ContactDetailOptionsResponse
    {
        [DataMember(Name= "locations")]
        public IList<Location> Locations;

        [DataMember(Name = "statuses")]
        public IList<ContactStatus> Statuses;

        [DataMember(Name = "preferredContactMethods")]
        public IList<ContactMethod> PreferredContactMethods;

        [DataMember(Name = "contactJobFunctions")]
        public IList<ContactJobFunction> ContactJobFunctions { get; set; }
    }
}
