using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class InventoryBomResponse
    {
        [DataMember(Name = "invLines")]
        public List<InventoryLineBom> InvLines { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRows { get; set; }
    }

    [DataContract]
    public class InventoryLineBom
    {
        [DataMember(Name = "warehouse")]
        public string Warehouse { get; set; }

        [DataMember(Name = "mfgPartNumber")]
        public string MfgPartNumber { get; set; }

        [DataMember(Name = "mfg")]
        public string Mfg { get; set; }

        [DataMember(Name = "invQty")]
        public int InvQty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "resQty")]
        public int ResQty { get; set; }

        [DataMember(Name = "availQty")]
        public int AvailQty { get; set; }

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
