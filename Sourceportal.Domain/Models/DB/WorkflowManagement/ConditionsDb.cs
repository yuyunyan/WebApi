namespace Sourceportal.Domain.Models.DB.WorkflowManagement
{
    public class ConditionsDb
    {
        public int ConditionID { get; set; }
        public int ObjectTypeID { get; set; }
        public string ComparisonType { get; set; }
        public string ConditionName { get; set; }
        public string DynamicValues { get; set; }
    }

    public class ConditionDynamicValueDb
    {
        public int ValueID { get; set; }
        public string ValueName { get; set; }
    }
}
