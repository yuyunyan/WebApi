using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Items
{
    public class ItemSalesOrdersDb
    {
        public int SOLineID;
        public int SalesOrderID;
        public int VersionID;
        public int AccountID;
        public string AccountName;
        public int ContactID;
        public string FirstName;
        public string LastName;
        public string OrgName;
        public string OrderDate;
        public string ShipDate;
        public string DateCode;
        public int Qty;
        public string Price;
        public string Cost;
        public int PackagingID;
        public string PackagingName;
        public int PackageConditionID;
        public string ConditionName;
        public string Owners;
        public string SOExternalID;
        public int TotalRowCount;
    }
}
