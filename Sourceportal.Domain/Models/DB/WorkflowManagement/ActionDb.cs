namespace Sourceportal.Domain.Models.DB.WorkflowManagement
{
    public class ActionDb
    {
        public int ActionID { get; set; }
        public int ObjectTypeID { get; set; }
        public string ActionName { get; set; }
        public string DynamicValues { get; set; }
    }

    public class ActionDynamicValueDb
    {
        public int ValueID { get; set; }
        public string ValueName { get; set; }
    }
}
