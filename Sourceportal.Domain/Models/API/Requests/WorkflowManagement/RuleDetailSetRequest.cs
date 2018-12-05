using System.Collections.Generic;

namespace Sourceportal.Domain.Models.API.Requests.WorkflowManagement
{
    public class RuleDetailSetRequest
    {
        public int RuleID { get; set; }
        public int? TriggerID { get; set; }
        public int? RuleOrder { get; set; }
        public string RuleName { get; set; }
        public byte IsDeleted { get; set; }
        public int? ObjectTypeID { get; set; }
        public List<RuleGroupRequest> Groups { get; set; }
        public List<RuleActionRequest> Actions { get; set; }
    }
}
