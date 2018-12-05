using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    [DataContract]
    public class PurchaseOrderListResponse : BaseResponse
    {
        [DataMember(Name = "purchaseOrders")]
        public IList<PurchaseOrderResponse> PurchaseOrders { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }

    [DataContract]
    public class PurchaseOrderResponse
    {

        [DataMember(Name = "purchaseOrderId")]
        public int PurchaseOrderID { get; set; }

        [DataMember(Name = "versionId")]    
        public int VersionID { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "contactId")]
        public int ContactID { get; set; }

        [DataMember(Name = "contactFirstName")]
        public string ContactFirstName { get; set; }

        [DataMember(Name = "contactLastName")]
        public string ContactLastName { get; set; }

        [DataMember(Name = "contactPhone")]
        public string ContactPhone;

        [DataMember(Name = "contactEmail")]
        public string ContactEmail;

        [DataMember(Name = "statusId")]
        public int StatusID;

        [DataMember(Name = "statusName")]
        public string StatusName;

        [DataMember(Name = "countryName")]
        public string CountryName;

        [DataMember(Name = "organizationId")]
        public int OrganizationID;

        [DataMember(Name = "organizationName")]
        public string OrganizationName;

        [DataMember(Name = "orderDate")]
        public string OrderDate;

        [DataMember(Name = "owners")]
        public string Owners;

        [DataMember(Name = "cost")]
        public string Cost;

        [DataMember(Name = "currencyId")]
        public string CurrencyID;

        [DataMember(Name = "billFromLocationId")]
        public int BillFromLocationID;

        [DataMember(Name = "billToLocationName")]
        public string BillToLocationName { get; set; }

        [DataMember(Name = "billToHouseNumber")]
        public string BillToHouseNumber { get; set; }

        [DataMember(Name = "billToCity")]
        public string BillToCity { get; set; }

        [DataMember(Name = "billToStateID")]
        public int BillToStateID { get; set; }

        [DataMember(Name = "billToStreet")]
        public string BillToStreet { get; set; }

        [DataMember(Name = "billToStateCode")]
        public string BillToStateCode { get; set; }

        [DataMember(Name = "billToPostalCode")]
        public string BillToPostalCode { get; set; }

        [DataMember(Name = "shipFromLocationId")]
        public int ShipFromLocationID;

        [DataMember(Name = "shipFromLocationName")]
        public string ShipFromLocationName { get; set; }

        [DataMember(Name = "shipFromHouseNumber")]
        public string ShipFromHouseNumber { get; set; }

        [DataMember(Name = "shipFromCity")]
        public string ShipFromCity { get; set; }

        [DataMember(Name = "shipFromStateID")]
        public int ShipFromStateID { get; set; }

        [DataMember(Name = "shipFromStreet")]
        public string ShipFromStreet { get; set; }

        [DataMember(Name = "shipFromStateCode")]
        public string ShipFromStateCode { get; set; }

        [DataMember(Name = "shipFromPostalCode")]
        public string ShipFromPostalCode { get; set; }

        [DataMember(Name = "shippingMethodId")]
        public int ShippingMethodID;

        [DataMember(Name = "paymentTermId")]
        public int PaymentTermID;

        [DataMember(Name = "incotermId")]
        public int IncotermID { get; set; }

        [DataMember(Name = "toWarehouseId")]
        public int ToWarehouseID { get; set; }

        [DataMember(Name = "poNotes")]
        public string PONotes { get; set; }

        [DataMember(Name = "userId")]
        public int? UserID { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

        [DataMember(Name = "hasPendingTransaction")]
        public bool HasPendingTransaction { get; set; }
    }
}
