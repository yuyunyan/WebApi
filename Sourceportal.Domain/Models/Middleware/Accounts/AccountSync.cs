using Sourceportal.Domain.Models.Middleware.Owners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Middleware.Accounts
{
    [DataContract]
    public class AccountSync : MiddlewareSyncBase
    {
        public AccountSync(int id, string externalId) : base(id, externalId)
        {
        }

        [DataMember(Name = "accountDetails")]
        public AccountDetails BpDetails { get; set; }

        [DataMember(Name = "locations")]
        public List<LocationDetails> Locations { get; set; }

        [DataMember(Name = "contacts")]
        public List<ContactDetails> Contacts { get; set; }
    }

    [DataContract]
    public class AccountDetails
    {
        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

        [DataMember(Name = "statusExternalId")]
        public string StatusExternalID { get; set; }

        [DataMember(Name = "ownership")]
        public SyncOwnership Ownership { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "accountTypes")]
        public List<string> AccountTypes { get; set; }

        [DataMember(Name = "website")]
        public string Website { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "vendorNum")]
        public string VendorNum { get; set; }

        [DataMember(Name = "approvedVendor")]
        public bool ApprovedVendor { get; set; }

        [DataMember(Name = "companyType")]
        public string CompanyType { get; set; }

        [DataMember(Name = "dbNum")]
        public string DBNum { get; set; }

        [DataMember(Name = "hierarchy")]
        public AccountHierarchyRequest Hierarchy { get; set; }
    }

    [DataContract]
    public class AccountHierarchyRequest
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "parentName")]
        public string ParentName { get; set; }

        [DataMember(Name = "sapHierarchyId")]
        public string SapHierarchyId { get; set; }

        [DataMember(Name = "parentGroupId")]
        public string ParentGroupId { get; set; }

        [DataMember(Name = "regionId")]
        public int RegionId { get; set; }

        [DataMember(Name = "childGroupId")]
        public string ChildGroupId { get; set; }

    }

    [DataContract]
    public class LocationDetails
    {
        [DataMember(Name = "locationId")]
        public int LocationId;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "externalId")]
        public string ExternalId;

        [DataMember(Name = "locationTypeExternalIds")]
        public List<string> LocationTypeExternalId;

        [DataMember(Name = "countryExternalId")]
        public string CountryExternalId;

        [DataMember(Name = "address1")]
        public string Address1;

        [DataMember(Name = "address2")]
        public string Address2;

        [DataMember(Name = "houseNo")]
        public string HouseNo;

        [DataMember(Name = "street")]
        public string Street;

        [DataMember(Name = "address4")]
        public string Address4;

        [DataMember(Name = "city")]
        public string City;

        [DataMember(Name = "stateExternalId")]
        public string StateExternalId;

        [DataMember(Name = "postalCode")]
        public string PostalCode;

        [DataMember(Name = "district")]
        public string District;
    }

    [DataContract]
    public class ContactDetails
    {
        [DataMember(Name = "contactId")]
        public int ContactId { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "officePhone")]
        public string OfficePhone { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "locationExternalId")]
        public string LocationExternalId { get; set; }

        [DataMember(Name = "jobFunctionExternalId")]
        public string JobFunctionExternalId { get; set; }
    }
}
