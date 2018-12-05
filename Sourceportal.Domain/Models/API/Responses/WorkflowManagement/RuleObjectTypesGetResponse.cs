using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    [DataContract]
    public class RuleObjectTypesGetResponse
    {
        [DataMember(Name = "ruleObjectTypes")]
        public List<RuleObjectType> RuleObjectTypes { get; set; }   
    }

    [DataContract]
    public class RuleObjectType
    {
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
