using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    [DataContract]
    public class StateEngineDynamicValue
    {
        [DataMember(Name = "valueId")]
        public int ValueID { get; set; }
        [DataMember(Name = "valueName")]
        public string ValueName { get; set; }
    }
}
