using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.Middleware.Accounts;
using Sourceportal.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    [DataContract]
    public class SetAccountExternalIdsRequest : SetExternalIdRequest
    /*{
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "customerDetails")]
        public AccountTypeDetailsSap CustomerDetails { get; set; }

        [DataMember(Name = "supplierDetails")]
        public AccountTypeDetailsSap SupplierDetails { get; set; }

        [DataMember(Name = "contactExternalIds")]
        public List<PairedIdsRequest> ContactExternalIds { get; set; }

        [DataMember(Name = "locationExternalIds")]
        public List<PairedIdsRequest> LocationExternalIds { get; set; }

        [DataMember(Name = "hierarchy")]
        public AccountHierarchyRequest Hierarchy { get; set; }
    }

    [DataContract]
    public class PairedIdsRequest
    {
        [DataMember(Name = "localId")]
        public int LocalId { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }


    [DataContract]
    public class AccountHierarchyRequest
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "parentName")]
        public string ParentName { get; set; }

        [DataMember(Name = "parentId")]
        public string HierarchyId { get; set; }

        [DataMember(Name = "parentGroupId")]
        public string ParentGroupId { get; set; }

        [DataMember(Name = "parentUUID")]
        public string ParentUUID { get; set; }

        [DataMember(Name = "children")]
        public List<AccountHierarchyChildRequest> Children { get; set; }
    }

    [DataContract]
    public class AccountHierarchyChildRequest
    {
        [DataMember(Name = "childName")]
        public string childGroupName { get; set; }

        [DataMember(Name = "childId")]
        public string ChildId { get; set; }

        [DataMember(Name = "accounts")]
        public List<string> Accounts { get; set; }

    }*/

    {
        [DataMember(Name = "accountDetails")]
        public AccountDetailsIncoming AccountDetails { get; set; }

        [DataMember(Name = "locations")]
        public List<LocationDetails> Locations { get; set; }

        [DataMember(Name = "contacts")]
        public List<ContactDetails> Contacts { get; set; }
    }

    [DataContract]
    public class AccountDetailsIncoming
    {
        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

        [DataMember(Name = "companyType")]
        public string CompanyType { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

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

        [DataMember(Name = "nonStockSupplier")]
        public bool NonStockSupplier { get; set; }

        [DataMember(Name = "dbNum")]
        public string DBNum { get; set; }

        [DataMember(Name = "customerDetails")]
        public AccountTypeDetailsSap CustomerDetails { get; set; }

        [DataMember(Name = "supplierDetails")]
        public AccountTypeDetailsSap SupplierDetails { get; set; }

        [DataMember(Name = "hierarchy")]
        public AccountHierarchySapResponse Hierarchy { get; set; }
    }

    [DataContract]
    public class AccountHierarchySapResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "parentName")]
        public string ParentName { get; set; }

        [DataMember(Name = "parentId")]
        public string HierarchyId { get; set; }

        [DataMember(Name = "parentGroupId")]
        public string ParentGroupId { get; set; }

        [DataMember(Name = "parentUUID")]
        public string ParentUUID { get; set; }

        [DataMember(Name = "children")]
        public List<AccountHierarchyChildSapResponse> Children { get; set; }
    }

    [DataContract]
    public class AccountHierarchyChildSapResponse
    {
        [DataMember(Name = "childName")]
        public string childGroupName { get; set; }

        [DataMember(Name = "childId")]
        public string ChildId { get; set; }

        [DataMember(Name = "assignedToAccount")]
        public bool AssignedToAccount { get; set; }

    }

    [DataContract]
    public class PairedIdsRequest
    {
        [DataMember(Name = "localId")]
        public int LocalId { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }




}
