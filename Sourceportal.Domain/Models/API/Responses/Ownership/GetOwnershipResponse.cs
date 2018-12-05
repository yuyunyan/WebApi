using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.Accounts;

namespace Sourceportal.Domain.Models.API.Responses.Ownership
{   
    [DataContract]
    public class GetOwnershipResponse
    {
        [DataMember(Name = "objectId")]
        public int ObjectID { get; set; }
        
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }

        [DataMember(Name="owners")]
        public List<Owner> Owners { get; set; }
    }
}
