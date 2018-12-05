using System.Collections.Generic;
using System.Runtime.Serialization;
using Sourceportal.Domain.Models.SAP_API.Responses;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class ContactDetailsResponse : SapApiBaseResonse
    {
        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "officePhone")]
        public string OfficePhone { get; set; }

        [DataMember(Name = "mobilePhone")]
        public string MobilePhone { get; set; }
        
        [DataMember(Name = "fax")]
        public string Fax { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "preferredContactMethodId")]
        public int PreferredContactMethodId { get; set; }

        [DataMember(Name = "locationId")]
        public int LocationId { get; set; }

        [DataMember(Name = "owners")]
        public List<Owner> Owners { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }

        [DataMember(Name = "isActive")]
        public int IsActive { get; set; }

        [DataMember(Name = "department")]
        public string Department { get; set; }

        [DataMember(Name = "jobFunctionID")]
        public int? JobFunctionID { get; set; }

        [DataMember(Name = "birthDate")]
        public string Birthdate { get; set; }

        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "salutation")]
        public string Salutation { get; set; }

        [DataMember(Name = "maritalStatus")]
        public string MaritalStatus { get; set; }

        [DataMember(Name = "kidsNames")]
        public string KidsNames { get; set; }

        [DataMember(Name = "reportsTo")]
        public string ReportsTo { get; set; }

        [DataMember(Name = "accountTypeIds")]
        public List<int> AccountTypeIds { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }
    }
}
