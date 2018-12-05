using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    [DataContract]
    public class StateEngineActionsGetResponse
    {
        [DataMember(Name = "actionList")]
        public List<ActionResponse> ActionList { get; set; }
    }

    [DataContract]
    public class ActionResponse
    {
        [DataMember(Name = "actionId")]
        public int ActionID { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "actionName")]
        public string ActionName { get; set; }
        [DataMember(Name = "dynamicValues")]
        public List<StateEngineDynamicValue> DynamicValues { get; set; }
    }
}
