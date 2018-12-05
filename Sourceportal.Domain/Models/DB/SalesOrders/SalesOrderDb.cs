using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.SalesOrders
{
    public class SalesOrderDb
    {
        public int SalesOrderID { get; set; }
        public int VersionID { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int ContactID { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string CountryName { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string OrderDate { get; set; }
        public string Owners { get; set; }
        public int TotalRows { get; set; }
        public string IncotermLocation { get; set; }
        public string ExternalID { get; set; }
        public int? ShipFromRegionID { get; set; }
    }
}
