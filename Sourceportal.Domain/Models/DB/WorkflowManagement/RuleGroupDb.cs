namespace Sourceportal.Domain.Models.DB.WorkflowManagement
{
    public class RuleGroupDb
    {
        public int RuleGroupID { get; set; }
        public int RuleID { get; set; }
        public int ParentGroupID { get; set; }
        public byte IsAll { get; set; }
        public int ObjectTypeID { get; set; }
        public string RuleConditions { get; set; }
    }

    public class RuleConditionDb
    {
        public int RuleConditionID { get; set; }
        public int RuleGroupID { get; set; }
        public int ConditionID { get; set; }
        public string Comparison { get; set; }
        public string ComparisonType { get; set; }
        public int ValueID { get; set; }
        public string StaticValue { get; set; }
        public string ValueName { get; set; }
        public string ConditionName { get; set; }
        public int ObjectTypeID { get; set; }
    }
}
