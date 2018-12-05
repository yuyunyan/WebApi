using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemPurchaseOrdersListResponse : BaseResponse
    {
        [DataMember(Name = "items")]
        public IList<ItemPurchaseOrderResponse> ItemPOs { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }

    [DataContract]
    public class ItemPurchaseOrderResponse
    {
        [DataMember(Name = "pOLineId")]
        public int POLineID { get; set; }

        [DataMember(Name = "purchaseOrderId")]
        public int PurchaseOrderID { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionID { get; set; }

        [DataMember(Name = "poExternalId")]
        public string POExternalID { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "contactId")]
        public string ContactID { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "organizationId")]
        public int OrganizationID { get; set; }

        [DataMember(Name = "orgName")]
        public string OrgName { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusID { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "orderDate")]
        public string OrderDate { get; set; }

        [DataMember(Name = "dueDate")]
        public string DueDate { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingID { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "packageConditionId")]
        public int PackageConditionID { get; set; }

        [DataMember(Name = "conditionName")]
        public string ConditionName { get; set; }

        [DataMember(Name = "owners")]
        public string Owners { get; set; }

        [DataMember(Name = "warehouseId")]
        public int WarehouseID { get; set; }

        [DataMember(Name = "warehouseName")]
        public string WarehouseName { get; set; }
    }
}


