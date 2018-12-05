namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class ContactDetailsDb
    {
        public int ContactId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int AccountTypeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OfficePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public int IsActive { get; set; }
        public string Title { get; set; }
        public int PreferredContactMethodId { get; set; }
        public int LocationId { get; set; }
        public string ExternalId { get; set; }
        public string Note { get; set; }
        public string Department { get; set; }
        public int? JobFunctionID { get; set; }
        public string Birthdate { get; set; }
        public string Gender { get; set; }
        public string Salutation { get; set; }
        public string MaritalStatus { get; set; }
        public string KidsNames { get; set; }
        public string ReportsTo { get; set; }
        public int AccountTypeBitwise { get; set; }
    }
}
