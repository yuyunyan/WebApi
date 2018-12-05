namespace Sourceportal.Domain.Models.DB.Roles
{
    public class DbRole
    {
        public int UserRoleID { get; set; }
        public int RoleId;
        public int ObjectTypeId;
        public string RoleName;
        public string ObjectName;
        public int FilterObjectId;
        public int FilterObjectTypeId;
        public int FilterTypeId { get; set; }
        public string TypeDescription { get; set; }
        public int TypeSecurityId { get; set; }
    }
}
