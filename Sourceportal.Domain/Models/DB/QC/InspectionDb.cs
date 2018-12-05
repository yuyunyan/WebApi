using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.QC
{
    public class InspectionDb
    {
        public int InspectionID { get; set; }
        public int InventoryID { get; set; }
        public int ItemID { get; set; }
        public int QtyFailed { get; set; }
        public int CompletedBy { get; set; }
        public string CompletedByUser { get; set; }
        public string CompletedDate { get; set; }
        public int CreatedBy { get; set; }
        public string Created { get; set; }
        public int POLineID { get; set; }
        public int Qty { get; set; }
        public int ItemQty { get; set; }
        public string DateCode { get; set; }
        public int PackagingID { get; set; }
        public int CommodityID { get; set; }
        public int ItemStatusID { get; set; }
        public string PartNumber { get; set; }
        public string PartNumberStrip { get; set; }
        public string MfrName { get; set; }
        public string PartDescription { get; set; }
        public string WarehouseName { get; set; }
        public string CustomerAccount { get; set; }
        public int CustomerAccountID { get; set; }
        public string VendorAccount { get; set; }
        public int VendorAccountID { get; set; }
        public string LotNumber { get; set; }
        public string QCNotes { get; set; }
        public string ExternalID { get; set; }
        public string SOExternalID { get; set; }
        public string POExternalID { get; set; }
        public string UserID { get; set; }
        public int InspectionStatusID { get; set; }
        public string DecisionCode { get; set; }
        public string AcceptanceCode { get; set; }
        public int InspectionQty { get; set; }
        public int SalesOrderID { get; set; }
        public int SOVersionID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int POVersionID { get; set; }
        public int ResultID { get; set; }
        public string InspectionTypeName { get; set; }
        public string VendorType { get; set; }    }
}
