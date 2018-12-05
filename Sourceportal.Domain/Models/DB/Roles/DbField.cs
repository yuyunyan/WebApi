namespace Sourceportal.Domain.Models.DB.Roles
{
    public class DbField
    {
        public int FieldId { get; set; }
        public string ObjectTypeId { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public int PermissionId { get; set; }
        public int CanEdit { get; set; }
        public int IsDeleted { get; set; }
        public int RoleID { get; set; }
    }
}
