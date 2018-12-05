namespace Sourceportal.Domain.Models.Middleware.SalesOrder
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ItemDetails
    {
        [DataMember(Name = "mfg")]
        public string Mfg { get; set; }

        [DataMember(Name = "commodityExternalId")]
        public string CommodityExternalId { get; set; }

        [DataMember(Name = "sourceDataId")]
        public string SourceDataId { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "partNumberStrip")]
        public string PartNumberStrip { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "eurohs")]
        public bool Eurohs { get; set; }

        [DataMember(Name = "eccn")]
        public string Eccn { get; set; }

        [DataMember(Name = "hts")]
        public string Hts { get; set; }

        [DataMember(Name = "mls")]
        public string Msl { get; set; }

        [DataMember(Name = "datasheetUrl")]
        public string DatasheetUrl { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalId { get; set; }
    }
}
