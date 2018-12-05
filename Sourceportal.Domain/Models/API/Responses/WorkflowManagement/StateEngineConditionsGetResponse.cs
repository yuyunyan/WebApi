using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    [DataContract]
    public class StateEngineConditionsGetResponse
    {
        [DataMember(Name = "stateEngineConditions")]
        public List<StateEngineCondition> StateEngineConditions { get; set; }
    }

    [DataContract]
    public class StateEngineCondition
    {
        [DataMember(Name = "conditionId")]
        public int ConditionID { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "comparisonType")]
        public string ComparisonType { get; set; }
        [DataMember(Name = "conditionName")]
        public string ConditionName { get; set; }
        [DataMember(Name = "dynamicValues")]
        public List<StateEngineDynamicValue> DynamicValues { get; set; }
    }
}
