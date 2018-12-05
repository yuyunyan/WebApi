using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB
{
    public class User
    {
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int OrganizationID { get; set; }
        public string TimezoneName { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime dateLastLogin { get; set; }
        public DateTime dateCreated { get; set; }
        public int CreatorUserID { get; set; }
        public DateTime dateModified { get; set; }
        public int ModifiedBy { get; set; }
        public int Status { get; set; }
        public string Error { get;  set;}
        public bool IsSuccessful {
            get { return Status == 0; }
        }
        public string ExternalId { get; set; }
        public string OrganizationName { get; set; }
    }

    public class UserRole
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int ObjectTypeID { get; set; }
        public string ObjectName { get; set; }
    }

}
