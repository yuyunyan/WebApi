namespace Sourceportal.Domain.Models.API.Requests.WorkflowManagement
{
    public class RuleActionRequest
    {
        public int? RuleActionID { get; set; }
        public int ActionID { get; set; }
        public int? ValueID { get; set; }
        public string StaticValue { get; set; }
    }
}
