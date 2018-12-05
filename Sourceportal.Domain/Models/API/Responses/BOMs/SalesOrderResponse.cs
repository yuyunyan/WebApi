using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
   public class SalesOrderResponse
   {
       [DataMember(Name = "salesOrderLine")]
        public IList<SalesOrderLine> SalesOrderLine { get; set; }
       [DataMember(Name = "totalRowCount")]
       public int TotalRowCount;
    }

    [DataContract]
    public class SalesOrderLine
    {
        [DataMember(Name = "mfg")]
        public string Mfg { get; set; }

        [DataMember(Name = "salesOrderId")]
        public int SalesOrderId { get; set; }

        [DataMember(Name = "recordId")]
        public int RecordId { get; set; }

        [DataMember(Name = "soDate")]
        public string SoDate { get; set; }

        [DataMember(Name = "customer")]
        public string Customer { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "qtySold")]
        public int QtySold { get; set; }

        [DataMember(Name = "soldPrice")]
        public decimal SoldPrice { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "unitCost")]
        public decimal UnitCost { get; set; }

        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }

        [DataMember(Name = "shippedQty")]
        public int ShippedQty { get; set; }

        [DataMember(Name = "orderStatus")]
        public string OrderStatus { get; set; }

        [DataMember(Name = "grossProfitTotal")]
        public decimal GrossProfitTotal { get; set; }

        [DataMember(Name = "salesPerson")]
        public string SalesPerson { get; set; }

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
