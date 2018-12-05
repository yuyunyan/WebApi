using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.SalesOrders
{
    [DataContract]
    public class GetSalesOrderLinesResponse : BaseResponse
    {   
        [DataMember(Name = "soLines")]
        public List<SalesOrderLineDetail> SOLinesResponse { get; set; }
    }

    [DataContract]
    public class SalesOrderLineDetail : BaseResponse
    {
        [DataMember(Name = "soLineId")]
        public int SOLineId { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "statusCanceled")]
        public int StatusIsCanceled { get; set; }

        [DataMember(Name = "lineNo")]
        public int LineNum { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "customerLineNo")]
        public int CustomerLine { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityId { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "customerPN")]
        public string CustomerPartNum { get; set; }

        [DataMember(Name = "quantity")]
        public int Qty { get; set; }
        [DataMember(Name = "reserved")]
        public int Reserved { get; set; }
        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackingId { get; set; }

        [DataMember(Name = "packagingConditionId")]
        public int PackageConditionId { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "deliveryRuleId")]
        public int? DeliveryRuleId { get; set; }

        [DataMember(Name = "shipDate")]
        public string ShipDate { get; set; }

        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }

        [DataMember(Name = "manufacturer")]
        public string MfrName { get; set; }

        [DataMember(Name = "totalRows")]
        public int TotalRows { get; set; }

        [DataMember(Name = "soId")]
        public int SalesOrderId { get; set; }

        [DataMember(Name = "soVersionId")]
        public int SalesOrderVersionId { get; set; }
        
        [DataMember(Name = "comments")]
        public int Comments { get; set; }

        [DataMember(Name = "isIhsItem")]
        public bool IsIhsItem { get; set; }

        [DataMember(Name = "productSpec")]
        public string ProductSpec { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool? IsDeleted { get; set; }
    }
}
