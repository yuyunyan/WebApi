using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Sourcing
{
    public class SourceListDb
    {
        public int SourceId { get; set; }
        public string SourceTypeId { get; set; }
        public string TypeName { get; set; }
        public bool IsConfirmed { get; set; }
        public int TypeRank { get; set; }
        public int ItemId { get; set; }
        public string PartNumber { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string Manufacturer { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int Qty { get; set; }
        public decimal Cost { get; set; }
        public string DateCode { get; set; }
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public int PackageConditionID { get; set; }
        public int MOQ { get; set; }
        public int SPQ { get; set; }
        public int LeadTimeDays { get; set; }
        public int ValidForHours { get; set; }
        public string ObjectTypeId { get; set; }
        public string Count { get; set; }
        public bool? IsMatch { get; set; }
        public bool IsJoined { get; set; }
        public int Comments { get; set; }
        public int? RTPQty { get; set; }
        public int AgeInDays { get; set; }
        public string Created { get; set; }
        public string CreatedBy { get; set; }
        public int BuyerID { get; set; }
        public string Rating { get; set; }
        public bool IsIhs { get; set; }
    }
}
