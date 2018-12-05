namespace Sourceportal.Domain.Models.Middleware.SalesOrder
{
    using Sourceportal.Domain.Models.Shared;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Line
    {

        [DataMember(Name = "itemExternalId")]
        public string ItemExternalId
        {
            get
            {
                ItemDetails = ItemDetails ?? new ItemDetails();
                return ItemDetails.ExternalId;
            }
            set { }
        }

        [DataMember(Name = "itemDetails")]
        public ItemDetails ItemDetails { get; set; }

        [DataMember(Name = "soLineId")]
        public int SoLineId { get; set; }

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
