using System.Collections.Generic;

namespace Sourceportal.Domain.Models.API.Requests.WorkflowManagement
{
    public class RuleGroupRequest
    {
        public int? RuleGroupID { get; set; }
        public int RuleID { get; set; }
        public int? ParentGroupID { get; set; }
        public bool IsAll { get; set; }
        public string Type { get; set; } = "ruleGroup";
        public List<RuleGroupRequest> Groups { get; set; }
        public List<RuleConditionRequest> Conditions { get; set; }
    }

    public class RuleConditionRequest
    {
        public int? RuleConditionID { get; set; }
        public int? RuleGroupID { get; set; }
        public int ConditionID { get; set; }
        public string Comparison { get; set; }
        public int? ValueID { get; set; }
        public string StaticValue { get; set; }
        public string Type { get; set; } = "ruleCondition";
    }

}
