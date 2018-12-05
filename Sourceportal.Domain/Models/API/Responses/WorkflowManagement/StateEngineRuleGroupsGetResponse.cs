using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    [DataContract]
    public class StateEngineRuleGroupsGetResponse
    {
        [DataMember(Name = "rootRuleGroup")]
        public StateEngineRuleGroup RootRuleGroup { get; set; }
    }

    [DataContract]
    public class StateEngineRuleGroup : StateEngineRuleGroupColumn
    {
        [DataMember(Name = "ruleGroupId")]
        public int RuleGroupID { get; set; }
        [DataMember(Name = "ruleId")]
        public int RuleID { get; set; }
        [DataMember(Name = "parentGroupId")]
        public int? ParentGroupID { get; set; }
        [DataMember(Name = "isAll")]
        public byte IsAll { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; } = "ruleGroup";
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "columns")]
        public List<StateEngineRuleGroupColumn> Columns { get; set; }
    }

    [DataContract]
    public class StateEngineRuleCondtion: StateEngineRuleGroupColumn
    {
        [DataMember(Name = "ruleConditionId")]
        public int RuleConditionID { get; set; }
        [DataMember(Name = "ruleGroupId")]
        public int RuleGroupID { get; set; }
        [DataMember(Name = "conditionId")]
        public int ConditionID { get; set; }
        [DataMember(Name = "comparison")]
        public string Comparison { get; set; }
        [DataMember(Name = "valueId")]
        public int ValueID { get; set; }
        [DataMember(Name = "staticValue")]
        public string StaticValue { get; set; }
        [DataMember(Name = "valueName")]
        public string ValueName { get; set; }
        [DataMember(Name = "conditionName")]
        public string ConditionName { get; set; }
        [DataMember(Name = "comparisonType")]
        public string ComparisonType { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeID { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; } = "ruleCondition";
    }

    public class StateEngineRuleGroupColumn
    {
    }
}
