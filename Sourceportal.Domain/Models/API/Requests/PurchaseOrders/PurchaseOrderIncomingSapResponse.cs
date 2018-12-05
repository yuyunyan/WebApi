using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.PurchaseOrders
{
    [DataContract]
    public class PurchaseOrderIncomingSapResponse : SetExternalIdRequest
    {
        [DataMember(Name = "accountExternalId")]
        public string AccountExternalId { get; set; }

        [DataMember(Name = "orgExternalId")]
        public string OrgExternalId { get; set; }

        [DataMember(Name = "ownerExternalId")]
        public string OwnerExternalId { get; set; }

        [DataMember(Name = "incotermExternalId")]
        public string IncotermExternalId { get; set; }

        [DataMember(Name = "paymentTermExternalId")]
        public string PaymentTermExternalId { get; set; }

        [DataMember(Name = "currencyExternalId")]
        public string CurrencyExternalId { get; set; }

        [DataMember(Name = "shippingMethodExternalId")]
        public string ShippingMethodExternalId { get; set; }

        [DataMember(Name = "orderDate")]
        public DateTime OrderDate { get; set; }

        [DataMember(Name = "lines")]
        public List<PurchaseOrderLineDetails> Lines { get; set; }
    }

    [DataContract]
    public class PurchaseOrderLineDetails
    {
        [DataMember(Name = "lineNum")]
        public string LineNum { get; set; }

        [DataMember(Name = "itemExternalId")]
        public string ItemExternalId { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "toLocationExternalId")]
        public string ToWarehouseExternalId { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingExternalId")]
        public string PackagingExternalId { get; set; }

        [DataMember(Name = "packageConditionExternalId")]
        public string PackageConditionExternalId { get; set; }

        [DataMember(Name = "dueDate")]
        public DateTime DueDate { get; set; }

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

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "productType")]
        public string ProductType { get; set; }

    }
}
