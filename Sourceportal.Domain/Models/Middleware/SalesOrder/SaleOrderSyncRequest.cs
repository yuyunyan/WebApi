namespace Sourceportal.Domain.Models.Middleware.SalesOrder
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SalesOrderSyncRequest : MiddlewareSyncRequest
    {
        public SalesOrderSyncRequest(int objectId, string objectType, int createdBy, int objectTypeId, string externalId)
            : base(objectId, objectType, createdBy, objectTypeId, externalId)
        { } 

        [DataMember(Name = "orgExternalId")]
        public string OrgExternalId { get; set; }

        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }

        [DataMember(Name = "customerPo")]
        public string CustomerPo { get; set; }

        [DataMember(Name = "incoTermExternalId")]
        public string IncoTermExternalId { get; set; }

        [DataMember(Name = "paymentTermExternalId")]
        public string PaymentTermExternalId { get; set; }

        [DataMember(Name = "currencyExternalId")]
        public string CurrencyExternalId { get; set; }

        [DataMember(Name = "orderDate")]        
        public string OrderDate { get; set; }

        [DataMember(Name = "accountExternalId")]
        public string AccountExternalId { get; set; }

        [DataMember(Name = "contactExternalId")]
        public string ContactExternalId { get; set; }

        [DataMember(Name = "ultDestinationId")]
        public string UltDestinationId { get; set; }

        [DataMember(Name = "freightAccount")]
        public string FreightAccount { get; set; }

        [DataMember(Name = "incotermLocation")]
        public string IncotermLocation { get; set; }       

        [DataMember(Name = "isSynchronous")]
        public bool IsSynchronous { get; set; }

        [DataMember(Name = "ownership")]
        public OwnerShip Ownership { get; set; }

        [DataMember(Name = "lines")]
        public List<Line> Lines { get; set; }        
    }
}
