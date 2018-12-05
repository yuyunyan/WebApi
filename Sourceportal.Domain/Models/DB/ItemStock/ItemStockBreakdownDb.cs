using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.ItemStock
{
    public class ItemStockBreakdownDb
    {
        public int BreakdownID { get; set; }
        public int StockID { get; set; }
        public bool IsDiscrepant { get; set; }
        public int PackQty { get; set; }
        public int NumPacks { get; set; }
        public string DateCode { get; set; }
        public int PackagingID { get; set; }
        public int PackageConditionID { get; set; }
        public int COO { get; set; }
        public DateTime Expiry { get; set; }
        public string MfrLotNum { get; set; }
        public bool IsDeleted { get; set; }
    }
}
