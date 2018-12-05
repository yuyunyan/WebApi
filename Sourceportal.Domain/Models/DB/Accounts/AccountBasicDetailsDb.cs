using System.Collections.Generic;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class AccountBasicDetailsDb
    {
        public int AccountId { get; set; }
        public string AccountNum { get; set; }
        public string AccountName { get; set; }
        public int AccountStatusId { get; set; }
        public int AccountHierarchyId { get; set; }
        public string StatusExternalId { get; set; }
        public int AccountTypeBitwise { get; set; }
        public int CompanyTypeId { get; set; }
        public string Error { get; set; }
        public string StatusName { get; set; }
        public string ExternalId { get; set; }
        public int OrganizationId { get; set; }
        public string CurrencyId { get; set; }
        public int PaymentTermID { get; set; }
        public string CreditLimit { get; set; }
        public string OpenBalance { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string YearEstablished { get; set; }
        public int NumOfEmployees { get; set; }
        public string EndProductFocus { get; set; }
        public int? CarryStock { get; set; }
        public float MinimumPO { get; set; }
        public string ShippingInstructions { get; set; }
        public string VendorNum { get; set; }
        public string SupplierRating { get; set; }
        public string QCNotes { get; set; }
        public string PONotes { get; set; }
        public bool ApprovedVendor { get; set; }
        public int IncotermID { get; set; }
        public string DBNum { get; set; }
        public bool IsSourceability { get; set; }
    }
}
