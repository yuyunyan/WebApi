using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class CustomerRFQBomResponse : BaseResponse
    {
        [DataMember(Name = "rfqLines")]
        public List<RFQLineBom> RFQLines { get; set; }

        [DataMember(Name = "TotalRows")]
        public int TotalRows { get; set; }
    }

    [DataContract]
    public class RFQLineBom : BaseResponse
    {
        [DataMember(Name = "quoteId")]
        public int QuoteID { get; set; }
        [DataMember(Name = "quoteDate")]
        public string QuoteDate { get; set; }
        [DataMember(Name = "customer")]
        public string Customer { get; set; }
        [DataMember(Name = "contact")]
        public string Contact { get; set; }
        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }
        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }
        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }
        [DataMember(Name = "qty")]
        public int Qty { get; set; }
        [DataMember(Name = "targetPrice")]
        public float TargetPrice { get; set; }
        [DataMember(Name = "owners")]
        public string Owners { get; set; }
        [DataMember(Name = "customerPartNum")]
        public string CustomerPartNum { get; set; }
        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }
        [DataMember(Name = "priceDelta")]
        public decimal PriceDelta { get; set; }
        [DataMember(Name = "potential")]
        public decimal Potential { get; set; }
        [DataMember(Name = "bomPartNumber")]
        public string BOMPartNumber { get; set; }
        [DataMember(Name = "bomIntPartNumber")]
        public string BOMIntPartNumber { get; set; }
        [DataMember(Name = "bomMfg")]
        public string BOMMfg { get; set; }
        [DataMember(Name = "bomQty")]
        public int BOMQty { get; set; }
        [DataMember(Name = "bomPrice")]
        public decimal BOMPrice { get; set; }


        
    }
}
