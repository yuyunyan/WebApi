using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class AccountBasicDetails : BaseResponse
    {
        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "number")]
        public string Number { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }

        [DataMember(Name = "statusExternalId")]
        public string StatusExternalId { get; set; }

        [DataMember(Name = "accountTypeIds")]
        public List<int> AccountTypeIds { get; set; }

        [DataMember(Name = "companyTypeId")]
        public int? CompanyTypeId { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }

        [DataMember(Name = "organizationId")]
        public int OrganizationId { get; set; }

        [DataMember(Name = "currencyId")]
        public string CurrencyId { get; set; }

        [DataMember(Name = "paymentTermId")]
        public int PaymentTermID { get; set; }

        [DataMember(Name = "accountHierarchyId")]
        public int? AccountHierarchyId { get; set; }

        [DataMember(Name = "openBalance")]
        public string OpenBalance { get; set; }

        [DataMember(Name = "creditLimit")]
        public string CreditLimit { get; set; }

        [DataMember(Name = "regionId")]
        public int? RegionId { get; set; }

        [DataMember(Name = "hierarchyName")]
        public string HierarchyName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "website")]
        public string Website { get; set; }

        [DataMember(Name = "yearEstablished")]
        public string YearEstablished { get; set; }

        [DataMember(Name = "numOfEmployees")]
        public int NumOfEmployees { get; set; }

        [DataMember(Name = "productFocus")]
        public string EndProductFocus { get; set; }

        [DataMember(Name = "carryStock")]
        public int? CarryStock { get; set; }

        [DataMember(Name = "minimumPO")]
        public float MinimumPO { get; set; }

        [DataMember(Name = "shippingInstructions")]
        public string ShippingInstructions { get; set; }

        [DataMember(Name = "vendorNum")]
        public string VendorNum { get; set; }

        [DataMember(Name = "supplierRating")]
        public string SupplierRating { get; set; }

        [DataMember(Name = "qcNotes")]
        public string QCNotes { get; set; }

        [DataMember(Name = "poNotes")]
        public string PONotes { get; set; }

        [DataMember(Name = "approvedVendor")]
        public bool ApprovedVendor { get; set; }
        [DataMember(Name = "isDeleted")]
        public byte IsDeleted { get; set; } = 0;

        [DataMember(Name = "incotermID")]
        public int IncotermID { get; set; }

        [DataMember(Name = "dbNum")]
        public string DBNum { get; set; }
    }
}
