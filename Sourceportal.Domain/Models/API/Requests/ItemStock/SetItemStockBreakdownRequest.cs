using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.ItemStock
{
    [DataContract]
    public class SetItemStockBreakdownRequest
    {
        [DataMember(Name = "stockBreakdownId")]
        public int? StockBreakdownID { get; set; }

        [DataMember(Name = "stockId")]
        public int StockID { get; set; }

        [DataMember(Name = "isDiscrepant")]
        public bool IsDiscrepant { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool? IsDeleted { get; set; }

        [DataMember(Name = "packQty")]
        public int? PackQty { get; set; }

        [DataMember(Name = "numPacks")]
        public int? NumPacks { get; set; }

        [DataMember(Name = "packagingTypeId")]
        public int? PackagingTypeID { get; set; }

        [DataMember(Name = "packageConditionId")]
        public int? PackageConditionID { get; set; }

        [DataMember(Name = "expiry")]
        public DateTime Expiry { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "mfrLotNum")]
        public string MfrLotNum { get; set; }

        [DataMember(Name = "coo")]
        public int? COO { get; set; }
    }
}
