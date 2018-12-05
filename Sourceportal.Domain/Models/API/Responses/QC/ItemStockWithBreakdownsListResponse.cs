using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class ItemStockWithBreakdownsListResponse
    {
        [DataMember(Name = "itemStockList")]
        public List<ItemStockResponse> ItemStockList { get; set; }

    }

    public class ItemStockResponse
    {
        [DataMember(Name = "itemStockID")]
        public int ItemStockID { get; set; }

        [DataMember(Name = "poLineID")]
        public int POLineID { get; set; }

        [DataMember(Name = "itemID")]
        public int ItemID { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "warehouseId")]
        public int WarehouseID { get; set; }

        [DataMember(Name = "warehouseBinId")]
        public int WarehouseBinID { get; set; }

        [DataMember(Name = "invStatusId")]
        public int InvStatusID { get; set; }

        [DataMember(Name = "receivedDate")]
        public DateTime? ReceivedDate { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "mfrLotNum")]
        public string MfrLotNum { get; set; }

        [DataMember(Name = "packagingId")]
        public int? PackagingID { get; set; }

        [DataMember(Name = "packageConditionId")]
        public int? PackageConditionID { get; set; }

        [DataMember(Name = "isRejected")]
        public bool IsRejected { get; set; }

        [DataMember(Name = "coo")]
        public int? COO { get; set; }

        [DataMember(Name = "expiry")]
        public DateTime? Expiry { get; set; }

        [DataMember(Name = "acceptedBinId")]
        public int? AcceptedBinID { get; set; }

        [DataMember(Name = "acceptedBinName")]
        public string AcceptedBinName { get; set; }

        [DataMember(Name = "rejectedBinId")]
        public int? RejectedBinID { get; set; }

        [DataMember(Name = "rejectedBinName")]
        public string RejectedBinName { get; set; }

        [DataMember(Name = "stockDescription")]
        public string StockDescription { get; set; }

        [DataMember(Name = "stockDescription")]
        public int InspectionWarehouseID { get; set; }
        

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        [DataMember(Name = "itemStockBreakdownList")]
        public List<ItemStockBreakdownResponse> ItemStockBreakdownList { get; set; }
    }

    [DataContract]
    public class ItemStockBreakdownResponse
    {
        [DataMember(Name = "breakdownId")]
        public int BreakdownID { get; set; }

        [DataMember(Name = "stockId")]
        public int StockID { get; set; }

        [DataMember(Name = "isDiscrepant")]
        public bool IsDiscrepant { get; set; }

        [DataMember(Name = "packQty")]
        public int PackQty { get; set; }

        [DataMember(Name = "numPacks")]
        public int NumPacks { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingID { get; set; }

        [DataMember(Name = "packageConditionId")]
        public int PackageConditionID { get; set; }

        [DataMember(Name = "coo")]
        public int COO { get; set; }

        [DataMember(Name = "expiry")]
        public DateTime Expiry { get; set; }

        [DataMember(Name = "mfrLotNum")]
        public string MfrLotNum { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
