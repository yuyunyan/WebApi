using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    [DataContract]
    public class StateEngineTriggersGetResponse
    {
        [DataMember(Name = "stateEngineTriggers")]
        public List<StateEngineTriggerResponse> StateEngineTriggers { get; set; }
    }

    [DataContract]
    public class StateEngineTriggerResponse
    {
        [DataMember(Name = "triggerId")]
        public int TriggerID { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "triggerName")]
        public string TriggerName { get; set; }
        [DataMember(Name = "triggerDescription")]
        public string TriggerDescription { get; set; }
    }
}
