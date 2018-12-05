using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.PurchaseOrders
{
    [DataContract]
    public class SetPurchaseOrderLineRequest
    {
        [DataMember(Name = "poLineId")]
        public int POLineId { get; set; }

        [DataMember(Name = "lineNum")]
        public int? LineNum { get; set; }

        [DataMember(Name = "lineRev")]
        public int? LineRev { get; set; }

        [DataMember(Name = "statusId")]
        public int? StatusId { get; set; }

        [DataMember(Name = "vendorLineNo")]
        public int? VendorLine { get; set; }

        [DataMember(Name = "itemId")]
        public long ItemId { get; set; }

        [DataMember(Name = "qty")]
        public int? Qty { get; set; }

        [DataMember(Name = "cost")]
        public double? Cost { get; set; }

        [DataMember(Name = "dateCode" , IsRequired = false)]
        public string DateCode { get ; set; }

        [DataMember(Name = "packagingId" )]
        public int? PackagingId { get; set; }

        [DataMember(Name = "packageConditionId")]
        public int? PackageConditionID { get; set; }
        
        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }

        [DataMember(Name = "promisedDate")]
        public string PromisedDate { get; set; }

        [DataMember(Name = "poId")]
        public int PurchaseOrderId { get; set; }

        [DataMember(Name = "poVersionId")]
        public int? POVersionId { get; set; }
        [DataMember(Name = "isSpecBuy")]
        public bool? IsSpecBuy { get; set; }

        [DataMember(Name = "specBuyForUserId")]
        public int? SpecBuyForUserID { get; set; }

        [DataMember(Name = "specBuyForAccountId")]
        public int? SpecBuyForAccountID { get; set; }

        [DataMember(Name = "specBuyReason" , IsRequired = false)]
        public string SpecBuyReason { get; set; }

        [DataMember(Name = "isIhsItem")]
        public bool IsIhsItem { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "clonedFromId")]
        public int? ClonedFromID { get; set; }

        [DataMember(Name = "toWarehouseId")]
        public int ToWarehouseID { get; set; }
    }
}
