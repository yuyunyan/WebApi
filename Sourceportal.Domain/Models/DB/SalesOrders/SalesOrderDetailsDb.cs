using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.SalesOrders
{
    public class SalesOrderDetailsDb
    {
        public int SalesOrderId { get; set; }
        public int VersionId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int ContactId { get; set; }
        public int ProjectId { get; set; }
        public int IncotermId { get; set; }
        public int PaymentTermId { get; set; }
        public string CurrencyId { get; set; }
        public int ShippingMethodId { get; set; }
        public int ShipFromRegionID { get; set; }
        public int OrganizationId { get; set; }
        public int FreightPaymentId { get; set; }
        public string FreightAccount { get; set; }
        public DateTime OrderDate { get; set; }
        public string ContactName { get; set; }
        public string OfficePhone { get; set; }
        public string Email { get; set; }
        public int ShipLocationId { get; set; }
        public string ShipLocationName { get; set; }
        public string CustomerPo { get; set; }
        public int? DeliveryRuleId { get; set; }
        public int UltDestinationId { get; set; }
        public string UltDestinationName { get; set; }
        public string SOPrice { get; set; }
        public string SOCost { get; set; }
        public string SOProfit { get; set;}
        public string SOGPM { get; set; }
        public string IncotermLocation { get; set; }
        public string ExternalId { get; set; }
        public string ShippingNotes { get; set; }
        public string QCNotes { get; set; }
        public int CarrierId { get; set; }
        public string CarrierName { get; set; }
        public int CarrierMethodId { get; set; }
        public string MethodName { get; set; }
        public int UserID { get; set; }
    }

    public class SalesOrderOrganizationDb
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address4 { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string OfficePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string USDAccount { get; set; }
        public string EURAccount { get; set; }
        public string SwiftAccount { get; set; }
        public string RoutingNumber { get; set; }
    }

}
