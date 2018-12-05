using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class EMSBomResponse : BaseResponse
    {
        [DataMember(Name = "emsLines")]
        public List<EMSLineBom> EMSLines { get; set; }

        [DataMember(Name = "TotalRows")]
        public int TotalRows { get; set; }
    }

    [DataContract]
    public class EMSLineBom : BaseResponse
    {
        [DataMember(Name = "itemListId")]
        public int ItemListID { get; set; }
        [DataMember(Name = "bomDate")]
        public string BOMDate { get; set; }
        [DataMember(Name = "customer")]
        public string Customer { get; set; }
        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }
        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }
        [DataMember(Name = "qty")]
        public int Qty { get; set; }
        [DataMember(Name = "targetPrice")]
        public decimal TargetPrice { get; set; }
        [DataMember(Name = "createdBy")]
        public string CreatedBy { get; set; }
        [DataMember(Name = "customerPartNum")]
        public string CustomerPartNum { get; set; }
        [DataMember(Name = "priceDelta")]
        public decimal PriceDelta { get; set; }
        [DataMember(Name = "potential")]
        public decimal Potential { get; set; }
        [DataMember(Name = "bomQty")]
        public int BOMQty { get; set; }
        [DataMember(Name = "bomPrice")]
        public int BOMPrice { get; set; }
        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "bomPartNumber")]
        public string BomPartNumber { get; set; }

        [DataMember(Name = "bomIntPartNumber")]
        public string BomIntPartNumber { get; set; }

        [DataMember(Name = "bomMfg")]
        public string BomMfg { get; set; }
    }
}
