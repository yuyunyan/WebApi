using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.OrderFulfillment
{
    [DataContract]
    public class OrderFulfillmentListResponse
    {
        [DataMember(Name = "oFList")]
        public IList<OrderFillmentResponse> OrderFillment { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }

    [DataContract]
    public class OrderFillmentResponse
    {
        [DataMember(Name = "orderNo")]
        public int OrderNo { get; set; }

        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }

        [DataMember(Name = "customer")]
        public string Customer { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "partNo")]
        public string PartNo { get; set; }

        [DataMember(Name = "mfr")]
        public string Mfr { get; set; }

        [DataMember(Name = "orderQty")]
        public int OrderQty { get; set; }

        [DataMember(Name = "soVersionId")]
        public int SOVersionID { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "shipDate")]
        public string ShipDate { get; set; }

        [DataMember(Name = "salesPerson")]
        public string SalesPerson { get; set; }

        [DataMember(Name = "soLineId")]
        public int SoLineId { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "buyers")]
        public List<OFBuyerResponse> Buyers { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }

        [DataMember(Name = "allocatedQty")]
        public int AllocatedQty { get; set; }

        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }

        [DataMember(Name = "cost")]
        public double Cost { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

    }
}
