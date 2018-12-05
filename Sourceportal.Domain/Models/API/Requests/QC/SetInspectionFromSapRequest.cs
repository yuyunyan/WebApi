using System.Runtime.Serialization;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;

namespace Sourceportal.Domain.Models.API.Requests.QC
{
    [DataContract]
    public class SetInspectionFromSapRequest : SetExternalIdRequest
    {
        [DataMember(Name = "stockExternalId")]
        public string StockExternalId { get; set; }


        [DataMember(Name = "inspectionQty")]
        public int InspectionQty { get; set; }

        [DataMember(Name = "inspectionStatusExternalId")]
        public string InspectionStatusExternalId { get; set; }

        [DataMember(Name = "documentExternalId")]
        public string DocumentExternalId { get; set; }

        [DataMember(Name = "inspectionTypeExternalId")]
        public string InspectionTypeExternalId { get; set; }
    }
}
