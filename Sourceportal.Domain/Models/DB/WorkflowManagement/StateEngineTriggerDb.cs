namespace Sourceportal.Domain.Models.DB.WorkflowManagement
{
    public class StateEngineTriggerDb
    {
        public int TriggerID { get; set; }
        public int ObjectTypeID { get; set; }
        public string TriggerName { get; set; }
        public string TriggerDescription { get; set; }
    }
}
