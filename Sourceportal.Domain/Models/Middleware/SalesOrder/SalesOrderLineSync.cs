using Sourceportal.Domain.Models.Middleware.Items;
using Sourceportal.Domain.Models.Shared;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.Middleware.SalesOrder
{
    [DataContract]
    public class SalesOrderLineSync
    {
        [DataMember(Name = "itemExternalId")]
        public string ItemExternalId { get; set; }

        [DataMember(Name = "itemDetails")]
        public ItemSync ItemDetails { get; set; }

        [DataMember(Name = "soLineId")]
        public int SOLineId { get; set; }

        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }

        [DataMember(Name = "customerLine")]
        public int CustomerLine { get; set; }

        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }

        [DataMember(Name = "customerPartNum")]
        public string CustomerPartNum { get; set; }

        [DataMember(Name = "shipDate")]
        public string ShipDate { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public string PackagingId { get; set; }

        [DataMember(Name = "packageConditionId")]
        public string PackageConditionId { get; set; }

        [DataMember(Name = "productSpec")]
        public string ProductSpec { get; set; }

        [DataMember(Name = "sourceOfSupply")]
        public SourceOfSupply SourceOfSupply { get; set; }
    }
}