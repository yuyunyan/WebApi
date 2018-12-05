using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.SalesOrders
{
    public class SalesOrderDetailsRequest
    {
        public int SalesOrderId { get; set; }
        public int VersionID { get; set; }
        public int AccountID { get; set; }
        public int ContactID { get; set; }
        public int ProjectID { get; set; }
        public int StatusID { get; set; }
        public int IncotermID { get; set; }
        public int PaymentTermID { get; set; }
        public string CurrencyID { get; set; }
        public int ShipLocationID { get; set; }
        public int ShippingMethodID { get; set; }
        public int ShipFromRegionID { get; set; }
        public int OrganizationID { get; set; }
        public int? DeliveryRuleID { get; set; }
        public int UltDestinationID { get; set; }
        public int FreightPaymentID { get; set; }
        public string FreightAccount { get; set; }
        public string OrderDate { get; set; }
        public string CustomerPo { get; set; }
        public string ExternalId { get; set; }
        public List<SalesOrderLinesSapData> Lines { get; set; }
        public List<SalesOrderItemsSapData> Items { get; set; }
        public string ShippingNotes { get; set; }
        public string QCNotes { get; set; }
        public int CarrierID { get; set; }
        public int CarrierMethodID { get; set; }
    }
}
