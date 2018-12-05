using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class OrganizationsResponse
    {
        [DataMember(Name = "organizations")]
        public IList<Organization> Organizations { get; set; }
    }

    [DataContract]
    public class Organization
    {
        [DataMember(Name = "id")]
        public int OrganizationID { get; set; }

        [DataMember(Name = "parentOrgId")]
        public int ParentOrgID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
    }
}
