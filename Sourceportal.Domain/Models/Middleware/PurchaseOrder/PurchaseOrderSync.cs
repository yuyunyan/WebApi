using System.Collections.Generic;
using System.Runtime.Serialization;
using Sourceportal.Domain.Models.Middleware.Owners;
using Sourceportal.Domain.Models.Middleware.Items;

namespace Sourceportal.Domain.Models.Middleware.PurchaseOrder
{
    public class PurchaseOrderSync : MiddlewareSyncBase
    {
        public PurchaseOrderSync(int id, string externalId) : base(id, externalId)
        {
        }
        
        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }
        
        [DataMember(Name = "accountExternalId")]
        public string AccountExternalId { get; set; }

        [DataMember(Name = "incoTermExternalId")]
        public string IncotermId { get; set; }

        [DataMember(Name = "paymentTermExternalId")]
        public string PaymentTermId { get; set; }

        [DataMember(Name = "currencyExternalId")]
        public string CurrencyId { get; set; }

        [DataMember(Name = "orgExternalId")]
        public string OrganizationId { get; set; }

        [DataMember(Name = "orderDate")]
        public string OrderDate { get; set; }

        [DataMember(Name = "toLocationExternalId")]
        public string ToLocationExternalId { get; set; }

        [DataMember(Name = "toLocationCity")]
        public string ToLocationCity { get; set; }

        [DataMember(Name = "ownership")]
        public SyncOwnership Ownership { get; set; }

        [DataMember(Name = "lines")]
        public List<PurchaseOrderLineSync> Lines { get; set; }
        
    }

    [DataContract]
    public class PurchaseOrderLineSync
    {
        [DataMember(Name = "lineNum")]
        public string LineNum { get; set; }

        [DataMember(Name = "itemExternalId")]
        public string ItemId { get; set; }

        [DataMember(Name = "itemDetails")]
        public ItemSync ItemDetails { get; set; }

        [DataMember(Name = "quantity")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public double Cost { get; set; }

        [DataMember(Name = "packagingTypeExternalId")]
        public string PackagingTypeExternalId { get; set; }

        [DataMember(Name = "promisedDate")]
        public string PromisedDate { get; set; }

        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }

        [DataMember(Name = "isSpecBuy")]
        public bool IsSpecBuy { get; set; }

        [DataMember(Name = "specBuyForUser")]
        public string SpecBuyForUser { get; set; }

        [DataMember(Name = "specBuyForAccount")]
        public string SpecBuyForAccount { get; set; }

        [DataMember(Name = "specBuyReason")]
        public string SpecBuyReason { get; set; }

        [DataMember(Name = "productSpec")]
        public string ProductSpec { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packageConditionExternalId")]
        public string PackageConditionExternalId { get; set; }

    }
    
}
