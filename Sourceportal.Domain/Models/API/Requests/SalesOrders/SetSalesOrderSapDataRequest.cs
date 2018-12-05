using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.SalesOrders
{
    [DataContract]
    public class SetSalesOrderSapDataRequest : SetExternalIdRequest
    {
        [DataMember(Name = "lines")]
        public List<SalesOrderLinesSapData> Lines { get; set; }

        [DataMember(Name = "items")]
        public List<SalesOrderItemsSapData> Items { get; set; }
    }

    [DataContract]
    public class SalesOrderLinesSapData
    {
        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }

        [DataMember(Name = "productSpec")]
        public string ProductSpec { get; set; }
    }

    [DataContract]
    public class SalesOrderItemsSapData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }

    [DataContract]
    public class SalesOrderIncomingSapResponse : SetExternalIdRequest
    {
        [DataMember(Name = "accountExternalId")]
        public string AccountExternalId { get; set; }

        [DataMember(Name = "contactExternalId")]
        public string ContactExternalId { get; set; }

        [DataMember(Name = "orgExternalId")]
        public string OrgExternalId { get; set; }

        [DataMember(Name = "ownerExternalId")]
        public string OwnerExternalId { get; set; }

        [DataMember(Name = "shipLocationExternalId")]
        public string ShipLocationExternalId { get; set; }

        [DataMember(Name = "customerPo")]
        public string CustomerPo { get; set; }

        [DataMember(Name = "incotermExternalId")]
        public string IncotermExternalId { get; set; }

        [DataMember(Name = "incotermLocation")]
        public string IncotermLocation { get; set; }

        [DataMember(Name = "ultDestinationExternalId")]
        public string UltDestinationExternalId { get; set; }

        [DataMember(Name = "paymentTermExternalId")]
        public string PaymentTermExternalId { get; set; }

        [DataMember(Name = "currencyExternalId")]
        public string CurrencyExternalId { get; set; }

        [DataMember(Name = "freightAccount")]
        public string FreightAccount { get; set; }

        [DataMember(Name = "orderDate")]
        public DateTime OrderDate { get; set; }

        [DataMember(Name = "lines")]
        public List<SalesOrderLineSapDetails> Lines { get; set; }
    }

    [DataContract]
    public class SalesOrderLineSapDetails
    {
        [DataMember(Name = "lineNum")]
        public string LineNum { get; set; }

        [DataMember(Name = "itemExternalId")]
        public string ItemExternalId { get; set; }

        [DataMember(Name = "customerLine")]
        public string CustomerLine { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingExternalId")]
        public string PackagingExternalId { get; set; }

        [DataMember(Name = "packageConditionExternalId")]
        public string PackageConditionExternalId { get; set; }

        [DataMember(Name = "customerPartNum")]
        public string CustomerPartNum { get; set; }

        [DataMember(Name = "shipDate")]
        public DateTime ShipDate { get; set; }

        [DataMember(Name = "dueDate")]
        public DateTime DueDate { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "productSpec")]
        public string ProductSpec { get; set; }

    }
}
