namespace Sourceportal.Domain.Models.API.Responses.WorkflowManagement
{
    public class RuleDetailSetResponse
    {
        public int RuleID { get; set; }
        public int TriggerID { get; set; }
        public int ObjectTypeID { get; set; }
        public int RuleOrder { get; set; }
        public string RuleName { get; set; }
        public byte IsDeleted { get; set; } = 0;
        public int ActionRowCount { get; set; }
    }
}
