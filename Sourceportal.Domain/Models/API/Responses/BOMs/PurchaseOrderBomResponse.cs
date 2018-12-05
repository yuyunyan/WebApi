using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class PurchaseOrderBomResponse : BaseResponse
    {
        [DataMember(Name = "poLines")]
        public List<PurchaseOrderLineBom> PoLines { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRows { get; set; }
    }

    [DataContract]
    public class PurchaseOrderLineBom : BaseResponse
    {
        [DataMember(Name = "poDate")]
        public string PODate { get; set; }

        [DataMember(Name = "vendor")]
        public string Vendor { get; set; }

        [DataMember(Name = "mfgPartNumber")]
        public string MfgPartNumber { get; set; }

        [DataMember(Name = "mfg")]
        public string Mfg { get; set; }

        [DataMember(Name = "qtyOrdered")]
        public int QtyOrdered { get; set; }

        [DataMember(Name = "poCost")]
        public decimal POCost { get; set; }

        [DataMember(Name = "buyer")]
        public string Buyer { get; set; }

        [DataMember(Name = "receivedDate")]
        public string ReceivedDate { get; set; }

        [DataMember(Name = "receivedQty")]
        public int ReceivedQty { get; set; }

        [DataMember(Name = "orderStatus")]
        public string OrderStatus { get; set; }

        [DataMember(Name = "purchaseOrderId")]
        public int PurchaseOrderID { get; set; }

        [DataMember(Name = "poLineId")]
        public int POLineID { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

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
