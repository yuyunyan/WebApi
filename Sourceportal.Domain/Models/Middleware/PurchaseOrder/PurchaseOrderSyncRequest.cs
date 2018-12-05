namespace Sourceportal.Domain.Models.Middleware.PurchaseOrder
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    public class PurchaseOrderSyncRequest : MiddlewareSyncRequest
    {
        public PurchaseOrderSyncRequest(int objectId, string objectType, int createdBy, int objectTypeId, string externalId)
           : base(objectId, objectType, createdBy, objectTypeId, externalId)
        { }

        [DataMember(Name = "versionId")]
        public int VersionId { get; set; }

        [DataMember(Name = "accountExternalId")]
        public string AccountExternalId { get; set; }

        [DataMember(Name = "incoTermExternalId")]
        public string IncotermId { get; set; }

        [DataMember(Name = "paymentTermExternalId")]
        public string PaymentTermId { get; set; }

        [DataMember(Name = "currencyExternalId")]
        public string CurrencyId { get; set; }

        [DataMember(Name = "orgExternalId")]
        public string OrganizationId { get; set; }

        [DataMember(Name = "orderDate")]
        public string OrderDate { get; set; }

        [DataMember(Name = "toLocationExternalId")]
        public string ToLocationExternalId { get; set; }

        [DataMember(Name = "toLocationCity")]
        public string ToLocationCity { get; set; }

        [DataMember(Name = "ownership")]
        public OwnerShip Ownership { get; set; }

        [DataMember(Name = "lines")]
        public List<PurchaseOrderLineSync> Lines { get; set; }
    }
}
