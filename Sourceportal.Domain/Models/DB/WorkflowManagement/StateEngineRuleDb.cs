namespace Sourceportal.Domain.Models.DB.WorkflowManagement
{
    public class StateEngineRuleDb
    {
        public int RuleID { get; set; }
        public int TriggerID { get; set; }
        public int ObjectTypeID { get; set; }
        public string RuleName { get; set; }
        public int RuleOrder { get; set; }
    }
}
