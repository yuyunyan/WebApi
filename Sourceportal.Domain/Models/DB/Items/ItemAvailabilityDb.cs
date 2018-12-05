using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Items
{
    public class ItemAvailabilityDb
    {
        public int SourceId;
        public int AccountId;
        public string AccountName;
        public string Created;
        public string CreatedBy;
        public string DateCode;
        public int Qty;
        public int Rating;
        public string Cost;
        public int RtpQty;
        public int PackagingID;
        public string PackagingName;
        public string MOQ;
        public string LeadTimeDays;
        public string TypeName;
        public string ExternalID;
        public int TotalRows;
    }
}
