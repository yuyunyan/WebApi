namespace Sourceportal.Domain.Models.DB.WorkflowManagement
{
    public class RuleActionDb
    {
        public int RuleActionID { get; set; }
        public int RuleID { get; set; }
        public int ActionID { get; set; }
        public int ObjectTypeID { get; set; }
        public string ActionName { get; set; }
        public int? ValueID { get; set; }
        public string ValueName { get; set; }
        public string StaticValue { get; set; }
    }
}
