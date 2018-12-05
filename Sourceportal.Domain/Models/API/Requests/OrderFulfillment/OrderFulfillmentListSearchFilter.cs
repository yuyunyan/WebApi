using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.OrderFulfillment
{
    public class OrderFulfillmentListSearchFilter
    {
        public string SearchString { get; set; }

        public int RowOffset { get; set; }

        public int RowLimit { get; set; }

        public string SortCol { get; set; }

        public bool UnderallocatedOnly { get; set; }

        public bool DescSort { get; set; }
        
        public string FilterBy { get; set; }

        public string FilterText { get; set; }
    }
}
