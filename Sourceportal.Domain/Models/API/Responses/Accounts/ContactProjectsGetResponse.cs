using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class ContactProjectsGetResponse
    {
        [DataMember(Name = "contactProjectMaps")]
        public List<ContactProjectMap> ContactProjectMaps { get; set; }
        [DataMember(Name = "contactProjectOptions")]
        public List<ContactProjectOption> ContactProjectOptions { get; set; }
    }

    [DataContract]
    public class ContactProjectMap
    {
        [DataMember(Name = "projectId")]
        public int ProjectID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    public class ContactProjectOption : ContactProjectMap{}
}
