using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.PurchaseOrders
{
    public class PurchaseOrderDb
    {
        public int PurchaseOrderID { get; set; }
        public int VersionID { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int ContactID { get; set; }
        public int ShippingMethodID { get; set; }
        public int PaymentTermID { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string OfficePhone { get; set; }
        public string Email { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string CountryName { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string OrderDate { get; set; }
        public string Owners { get; set; }
        public string Cost { get; set; }
        public string CurrencyID { get; set; }
        public int BillFromLocationID { get; set; }
        public string BillToLocationName { get; set; }
        public string BillToHouseNumber { get; set; }
        public string BillToCity { get; set; }
        public int BillToStateID { get; set; }
        public string BillToStreet { get; set; }
        public string BillToStateCode { get; set; }
        public string BillToPostalCode { get; set; }
        public int IncotermID { get; set; }
        public int ShipFromLocationID { get; set; }
        public string ShipFromLocationName { get; set; }
        public string ShipFromHouseNumber { get; set; }
        public string ShipFromCity { get; set; }
        public int ShipFromStateID { get; set; }
        public string ShipFromStreet { get; set; }
        public string ShipFromStateCode { get; set; }
        public string ShipFromPostalCode { get; set; }
        public int ToWarehouseID { get; set; }
        public int TotalRows { get; set; }
        public string ExternalId { get; set; }
        public string PONotes { get; set; }
    }
}
