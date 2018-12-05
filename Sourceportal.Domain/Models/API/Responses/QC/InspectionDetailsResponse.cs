using Sourceportal.Domain.Models.API.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class InspectionDetailsResponse : BaseResponse
    {
        [DataMember(Name = "inspectionId")]
        public int InspectionID { get; set; }

        [DataMember(Name = "inventoryId")]
        public int InventoryID { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "qtyFailed")]
        public int QtyFailed { get; set; }

        [DataMember(Name = "completedBy")]
        public int CompletedBy { get; set; }

        [DataMember(Name = "completedDate")]
        public string CompletedDate { get; set; }

        [DataMember(Name = "completedByUser")]
        public string CompletedByUser { get; set; }

        [DataMember(Name = "createdBy")]
        public int CreatedBy { get; set; }

        [DataMember(Name = "createdDate")]
        public string CreatedDate { get; set; }

        [DataMember(Name = "PoLineId")]
        public int POLineID { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingID { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityID { get; set; }

        [DataMember(Name = "itemstatusID")]
        public int ItemStatusID { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "partNumberStrip")]
        public string PartNumberStrip { get; set; }

        [DataMember(Name = "mfrName")]
        public string mfrName { get; set; }

        [DataMember(Name = "partDescription")]
        public string PartDescription { get; set; }

        [DataMember(Name = "warehouseName")]
        public string WarehouseName { get; set; }

        [DataMember(Name = "customerAccount")]
        public string CustomerAccount { get; set; }

        [DataMember(Name = "vendorAccount")]
        public string VendorAccount { get; set; }

        [DataMember(Name = "lotNumber")]
        public string LotNumber { get; set; }
        [DataMember(Name = "qcNotes")]
        public string QCNotes { get; set; }

        [DataMember(Name = "customerAccountId")]
        public int CustomerAccountID { get; set; }

        [DataMember(Name = "vendorAccountId")]
        public int VendorAccountID { get; set; }

        [DataMember(Name = "itemQty")]
        public int ItemQty { get; set; }

        [DataMember(Name = "userId")]
        public string UserID { get; set; }
        
        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
        [DataMember(Name = "poExternalID")]
        public string POExternalID { get; set; }
        [DataMember(Name = "soExternalID")]
        public string SOExternalID { get; set; }


        [DataMember(Name = "inspectionStatusId")]
        public int InspectionStatusId { get; set; }

        [DataMember(Name = "salesOrderID")]
        public int SalesOrderID { get; set; }

        [DataMember(Name = "soVersionID")]
        public int SOVersionID { get; set; }

        [DataMember(Name = "purchaseOrderID")]
        public int PurchaseOrderID { get; set; }

        [DataMember(Name = "poVersionID")]
        public int POVersionID { get; set; }

        [DataMember(Name = "resultID")]
        public int ResultID { get; set; }

        [DataMember(Name = "inspectionTypeName")]
        public string InspectionTypeName { get; set; }

        [DataMember(Name = "vendorType")]
        public string VendorType { get; set; }
    }
}
