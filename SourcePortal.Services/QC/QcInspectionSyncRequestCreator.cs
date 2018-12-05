using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.Documents;
using Sourceportal.DB.Enum;
using Sourceportal.DB.QC;
using Sourceportal.DB.User;
using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.Enums;
using Sourceportal.Domain.Models.Middleware.QcInspection;
using Sourceportal.Domain.Models.Middleware.SalesOrder;
using Sourceportal.Utilities;

namespace SourcePortal.Services.QC
{
    public class QcInspectionSyncRequestCreator : IQcInspectionSyncRequestCreator
    {
        private readonly IInspectionRepository _inspectionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentsRepository _documentsRepository;

        public QcInspectionSyncRequestCreator(IInspectionRepository inspectionRepository, IUserRepository userRepository, IDocumentsRepository documentsRepository)
        {
            _inspectionRepository = inspectionRepository;
           _userRepository = userRepository;
            _documentsRepository = documentsRepository;
        }

        public MiddlewareSyncRequest<QcInspectionSync> Create(int inspectionId)
        {
            var syncRequest = new MiddlewareSyncRequest<QcInspectionSync>(
                inspectionId, 
                MiddlewareObjectTypes.QcInspection.ToString(), 
                UserHelper.GetUserId(),
                (int)ObjectType.Inspection);
            var inspectionDetails = _inspectionRepository.GetInspectionDetails(inspectionId);
            var systemDocuments = _documentsRepository.GetDocuments(ObjectType.Inspection, inspectionId, null, null, (int)DocumentType.Text, null, false, true);
            var conclusion = _inspectionRepository.GetInspectionConclusion(inspectionId);

            var qcInspectionSync = new QcInspectionSync(inspectionId, inspectionDetails.ExternalID);
            qcInspectionSync.InspectionStatusId = _inspectionRepository.GetInspectionStatusExternalId(inspectionDetails.InspectionStatusID);
            qcInspectionSync.CompletedBy = _userRepository.GetUserData(UserHelper.GetUserId()).ExternalId; //inspectionDetails.CompletedBy != 0 ? _userRepository.GetUserData(inspectionDetails.CompletedBy).ExternalId: null;
            qcInspectionSync.QtyFailed = conclusion.QtyFailedTotal;
            qcInspectionSync.InspectionQty = inspectionDetails.InspectionQty;
            
            if (systemDocuments != null && systemDocuments.Count > 0)
            {
                var doc = systemDocuments.First();
                qcInspectionSync.Document = new QcInspectionDocument();
                qcInspectionSync.Document.ObjectId = doc.ObjectID;
                qcInspectionSync.Document.DocumentExternalId = doc.ExternalId;
                qcInspectionSync.Document.DocumentName = doc.DocName;
                
                //todo: needs to create the correct path when we store this document in the folder
                qcInspectionSync.Document.DocumentUrl = doc.FolderPath + "/"+ doc.FileNameStored;
            }

            if (inspectionDetails.ResultID != 0)
            {
                qcInspectionSync.Result = new QcInspectionResult();
                qcInspectionSync.Result.ResultId = inspectionDetails.ResultID;
                qcInspectionSync.Result.AcceptanceCode = inspectionDetails.AcceptanceCode;
                qcInspectionSync.Result.DecisionCode = inspectionDetails.DecisionCode;
            }

            syncRequest.Data = qcInspectionSync;
            return syncRequest;
        }
    }
}
