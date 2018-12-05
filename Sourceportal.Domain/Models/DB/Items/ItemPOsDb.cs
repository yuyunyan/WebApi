using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Items
{
    public class ItemPOsDb
    {
        public int POLineID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int VersionID { get; set; }
        public string POExternalID { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OrganizationID { get; set; }
        public string OrgName { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string OrderDate { get; set; }
        public string DueDate { get; set; }
        public int Qty { get; set; }
        public decimal Cost { get; set; }
        public string DateCode { get; set; }
        public int PackagingID { get; set; }
        public string PackagingName { get; set; }
        public int PackageConditionID { get; set; }
        public string ConditionName { get; set; }
        public string Owners { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int TotalRowCount { get; set; }
    }
}
