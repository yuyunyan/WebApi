using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    [DataContract]
    public class StateEngineRuleActionsGetResponse
    {
        [DataMember(Name = "ruleActions")]
        public List<StateEngineRuleAction> RuleActions { get; set; }
    }

    [DataContract]
    public class StateEngineRuleAction
    {
        [DataMember(Name = "ruleActionId")]
        public int RuleActionID { get; set; }
        [DataMember(Name = "ruleId")]
        public int RuleID { get; set; }
        [DataMember(Name = "actionId")]
        public int ActionID { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "actionName")]
        public string ActionName { get; set; }
        [DataMember(Name = "valueId")]
        public int? ValueID { get; set; }
        [DataMember(Name = "valueName")]
        public string ValueName { get; set; }
        [DataMember(Name = "staticValue")]
        public string StaticValue { get; set; }
    }
}
