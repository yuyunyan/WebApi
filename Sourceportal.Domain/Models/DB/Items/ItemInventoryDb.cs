using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Items
{
    public class ItemInventoryDb
    {
        public int WarehouseID;
        public string WarehouseName;
        public int AccountID;
        public string AccountName;
        public int OrigQty;
        public int Available;
        public int PurchaseOrderID;
        public int POVersionID;
        public string POExternalID;
        public string Buyers;
        public int StatusID;
        public string StatusName;
        public string Cost;
        public string Allocations;
        public string DateCode;
        public string PackagingName;
        public string ConditionName;
        public string ShipDate;
        public int TotalRows;
    }
}
