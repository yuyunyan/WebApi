using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{

    [DataContract]
    public class GetPurchaseOrderLinesResponse : BaseResponse
    {
        [DataMember(Name = "poLines")]
        public List<PurchaseOrderLineDetail> POLinesResponse { get; set; }
    }

    [DataContract]
    public class PurchaseOrderLineDetail : BaseResponse
    {
        [DataMember(Name = "poLineId")]
        public int POLineId { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "statusCanceled")]
        public int StatusIsCanceled { get; set; }

        [DataMember(Name = "lineNo")]
        public string LineNum { get; set; }

        [DataMember(Name = "vendorLine")]
        public int VendorLine { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        //[DataMember(Name = "commodityId")]
        //public int CommodityId { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        //[DataMember(Name = "customerPN")]
        //public string CustomerPartNum { get; set; }

        [DataMember(Name = "quantity")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public double Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackingId { get; set; }

        [DataMember(Name = "conditionId")]
        public int ConditionID { get; set; }

        [DataMember(Name = "conditionName")]
        public string ConditionName { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "promisedDate")]
        public string PromisedDate { get; set; }

        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }

        [DataMember(Name = "manufacturer")]
        public string MfrName { get; set; }

        [DataMember(Name = "totalRows")]
        public int TotalRows { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }
        [DataMember(Name = "isSpecBuy")]
        public bool? IsSpecBuy { get; set; }

        [DataMember(Name = "specBuyForUserId")]
        public int? SpecBuyForUserID { get; set; }

        [DataMember(Name = "specBuyForAccountId")]
        public int? SpecBuyForAccountID { get; set; }

        [DataMember(Name = "specBuyReason", IsRequired = false)]
        public string SpecBuyReason { get; set; }

        [DataMember(Name = "allocatedQty", IsRequired = false)]
        public int AllocatedQty { get; set; }

        [DataMember(Name = "allocatedSoId")]
        public int? AllocatedSalesOrderId { get; set; }

        [DataMember(Name = "allocatedSoVersionId")]
        public int? AllocatedSalesOrderVersionId { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
        
    }
}
