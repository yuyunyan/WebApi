using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.ItemStock
{
    [DataContract]
    public class SetItemStockRequest
    {
        [DataMember(Name= "stockId")]
        public int StockID { get; set; }

        [DataMember(Name = "isRejected")]
        public bool IsRejected { get; set; }

        [DataMember(Name = "warehouseBinId")]
        public int WarehouseBinID { get; set; }

        [DataMember(Name = "packagingTypeId")]
        public int PackagingTypeID { get; set; }

        [DataMember(Name = "mfrLotNum")]
        public string MfrLotNum { get; set; }

        [DataMember(Name = "packageConditionId")]
        public int PackageConditionID { get; set; }

        [DataMember(Name = "poLineId")]
        public int POLineID { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "invStatusId")]
        public int InvStatusID { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

        [DataMember(Name = "stockDescription")]
        public string StockDescription { get; set; }

        [DataMember(Name = "expiry")]
        public DateTime? Expiry { get; set; }

        [DataMember(Name = "receivedDate")]
        public DateTime ReceivedDate { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "coo")]
        public int COO { get; set; }

        [DataMember(Name = "inspectionId")]
        public int InspectionID { get; set; }
        
    }
}
