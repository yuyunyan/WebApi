using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.Middleware.QcInspection
{
    public class QcInspectionSync : MiddlewareSyncBase
    {
        public QcInspectionSync (int id, string externalId) :base(id, externalId)
        {
        }

        [DataMember(Name = "inspectionStatusExternalId")]
        public string InspectionStatusId { get; set; }

        [DataMember(Name = "qtyFailed")]
        public int QtyFailed { get; set; }

        [DataMember(Name = "completedBy")]
        public string CompletedBy { get; set; }

        [DataMember(Name = "inspectionQty")]
        public int InspectionQty { get; set; }

        [DataMember(Name = "document")]
        public QcInspectionDocument Document { get; set; }

        [DataMember(Name = "result")]
        public QcInspectionResult Result { get; set; }
    }

    public class QcInspectionDocument
    {
        [DataMember(Name = "objectId")]
        public int ObjectId { get; set; }

        [DataMember(Name = "documentExternalId")]
        public string DocumentExternalId { get; set; }

        [DataMember(Name = "documentName")]
        public string DocumentName { get; set; }

        [DataMember(Name = "documentUrl")]
        public string DocumentUrl { get; set; }
    }

    public class QcInspectionResult
    {
        [DataMember(Name = "resultId")]
        public int ResultId { get; set; }

        [DataMember(Name = "decisionCode")]
        public string DecisionCode { get; set; }

        [DataMember(Name = "acceptanceCode")]
        public string AcceptanceCode { get; set; }
    }
}
