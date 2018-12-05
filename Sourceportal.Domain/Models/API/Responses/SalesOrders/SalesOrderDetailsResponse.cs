using System;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.SalesOrders
{
   [DataContract]
   public class SalesOrderDetailsResponse : BaseResponse
    {
        [DataMember (Name = ("salesOrderId"))]
        public int SalesOrderId { get; set; }

        [DataMember(Name = ("versionId"))]
        public int VersionId { get; set; }

        [DataMember(Name = ("statusId"))]
        public int StatusId { get; set; }

        [DataMember(Name = ("statusName"))]
        public string StatusName { get; set; }

        [DataMember(Name = ("accountId"))]
        public int AccountId { get; set; }

        [DataMember(Name = ("accountName"))]
        public string AccountName { get; set; }

        [DataMember(Name = ("contactId"))]
        public int ContactId { get; set; }

        [DataMember(Name = ("projectId"))]
        public int ProjectId { get; set; }

        [DataMember(Name = ("incotermId"))]
        public int IncotermId { get; set; }

        [DataMember(Name = ("paymentTermId"))]
        public int PaymentTermId { get; set; }

        [DataMember(Name = ("currencyId"))]
        public string CurrencyId { get; set; }

        [DataMember(Name = ("shippingMethodId"))]
        public int ShippingMethodId { get; set; }

        [DataMember(Name = ("shipFromRegionId"))]
        public int ShipFromRegionID { get; set; }

        [DataMember(Name = ("organizationId"))]
        public int OrganizationId { get; set; }

        [DataMember(Name = ("organization"))]
        public SalesOrderOrganization Organization{ get; set; }

        [DataMember(Name = ("freightPaymentId"))]
        public int FreightPaymentId { get; set; }

        [DataMember(Name = ("freightAccount"))]
        public string FreightAccount { get; set; }

        [DataMember(Name = ("orderDate"))]
        public string OrderDate { get; set; }

        [DataMember(Name = ("contactName"))]
        public string ContactName { get; set; }

        [DataMember(Name = "deliveryRuleId")]
        public int? DeliveryRuleId { get; set; }

        [DataMember(Name = ("countryId"))]
        public int UltDestinationId { get; set; }

        [DataMember(Name = ("countryName"))]
        public string UltDestinationName { get; set; }

        [DataMember(Name = ("phone"))]
        public string Phone { get; set; }

        [DataMember(Name = ("email"))]
        public string Email { get; set; }

        [DataMember(Name = ("shipLocationId"))]
        public int ShipLocationId { get; set; }

        [DataMember(Name = ("shipLocationName"))]
        public string ShipLocationName { get; set; }

        [DataMember(Name = ("customerPo"))]
        public string CustomerPo { get; set; }

        [DataMember(Name = ("soCost"))]
        public string SOCost { get; set; }

        [DataMember(Name = ("soPrice"))]
        public string SOPrice { get; set; }

        [DataMember(Name = ("soProfit"))]
        public string SOProfit { get; set; }

        [DataMember(Name = ("soGpm"))]
        public string SOGPM { get; set; }
        
        [DataMember(Name = ("shippingNotes"))]
        public string ShippingNotes { get; set; }

        [DataMember(Name = ("qcNotes"))]
        public string QCNotes { get; set; }

        [DataMember(Name = ("carrierId"))]
        public int CarrierId { get; set; }

        [DataMember(Name = ("carrierName"))]
        public string CarrierName { get; set; }

        [DataMember(Name = ("carrierMethodId"))]
        public int CarrierMethodId { get; set; }

        [DataMember(Name = ("methodName"))]
        public string MethodName { get; set; }

        [DataMember(Name = ("userId"))]
        public int UserID { get; set; }

        [DataMember(Name = ("externalId"))]
        public string ExternalID { get; set; }
    }

    [DataContract]
    public class SalesOrderOrganization : BaseResponse
    {
        [DataMember(Name = ("organizationName"))]
        public string OrganizationName { get; set; }

        [DataMember(Name = ("address1"))]
        public string Address1 { get; set; }

        [DataMember(Name = ("address2"))]
        public string Address2 { get; set; }

        [DataMember(Name = ("address4"))]
        public string Address4 { get; set; }

        [DataMember(Name = ("houseNumber"))]
        public string HouseNumber { get; set; }

        [DataMember(Name = ("street"))]
        public string Street { get; set; }

        [DataMember(Name = ("city"))]
        public string City { get; set; }

        [DataMember(Name = ("stateCode"))]
        public string StateCode { get; set; }

        [DataMember(Name = ("stateName"))]
        public string StateName { get; set; }

        [DataMember(Name = ("postalCode"))]
        public string PostalCode { get; set; }

        [DataMember(Name = ("countryName"))]
        public string CountryName { get; set; }

        [DataMember(Name = ("officePhone"))]
        public string OfficePhone { get; set; }

        [DataMember(Name = ("mobilePhone"))]
        public string MobilePhone { get; set; }

        [DataMember(Name = ("fax"))]
        public string Fax { get; set; }

        [DataMember(Name = ("email"))]
        public string Email { get; set; }

        [DataMember(Name = ("bank"))]
        public SalesOrderOrganizationBank Bank { get; set; }
    }

    [DataContract]
    public class SalesOrderOrganizationBank : BaseResponse
    {
        [DataMember(Name = ("bankName"))]
        public string BankName { get; set; }

        [DataMember(Name = ("branchName"))]
        public string BranchName { get; set; }

        [DataMember(Name = ("usdAccount"))]
        public string USDAccount { get; set; }

        [DataMember(Name = ("eurAccount"))]
        public string EURAccount { get; set; }

        [DataMember(Name = ("swiftAccount"))]
        public string SwiftAccount { get; set; }

        [DataMember(Name = ("routingNumber"))]
        public string RoutingNumber { get; set; }
    }
        

}