using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    [DataContract]
    public class StateEngineRulesGetResponse
    {
        [DataMember(Name = "stateEngineRuleList")]
        public List<StateEngineRuleResponse> StateEngineRuleList { get; set; }
    }

    [DataContract]
    public class StateEngineRuleResponse
    {
        [DataMember(Name = "ruleId")]
        public int RuleID { get; set; }
        [DataMember(Name = "triggerId")]
        public int TriggerID { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "ruleName")]
        public string RuleName { get; set; }
        [DataMember(Name = "ruleOrder")]
        public int RuleOrder { get; set; }
    }
}
