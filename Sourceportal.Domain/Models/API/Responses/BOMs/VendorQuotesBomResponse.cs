using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class VendorQuotesBomResponse
    {
        [DataMember(Name = "vqLines")]
        public List<VendorQuoteLine> VendorQuoteLines { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRows { get; set; }
    }


    [DataContract]
    public class VendorQuoteLine
    {
        [DataMember(Name = "sourceId")]
        public int SourceID { get; set; }

        [DataMember(Name = "offerDate")]
        public string OfferDate { get; set; }

        [DataMember(Name = "vendor")]
        public string Vendor { get; set; }

        [DataMember(Name = "mfgPartNumber")]
        public string MfgPartNumber { get; set; }

        [DataMember(Name = "mfg")]
        public string Mfg { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "buyer")]
        public string Buyer { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "leadTimeDays")]
        public int LeadTimeDays { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }

        [DataMember(Name = "sqp")]
        public int Spq { get; set; }

        [DataMember(Name = "moq")]
        public int Moq { get; set; }

        [DataMember(Name = "bomQty")]
        public int BomQty { get; set; }

        [DataMember(Name = "priceDelta")]
        public decimal PriceDelta { get; set; }

        [DataMember(Name = "potential")]
        public decimal Potential { get; set; }

        [DataMember(Name = "bomPrice")]
        public decimal BomPrice { get; set; }

        [DataMember(Name = "bomPartNumber")]
        public string BomPartNumber { get; set; }

        [DataMember(Name = "bomIntPartNumber")]
        public string BomIntPartNumber { get; set; }

        [DataMember(Name = "bomMfg")]
        public string BomMfg { get; set; }


    }
}
