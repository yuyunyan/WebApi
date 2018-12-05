using System.Collections.Generic;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class ContactListContactDb
    {
        public int ContactId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OfficePhone { get; set; }
        public string Email { get; set; }
        public string ExternalID { get; set; }
        public string Details { get; set; }
        public bool IsActive { get; set; }
        public string AccountStatus { get; set; }
        public string AccountTypes { get; set; }
        public string Owners { get; set; }
        public string Title { get; set; }
        public int TotalRows { get; set; }
    }


}
