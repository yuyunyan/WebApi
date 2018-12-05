using System.Collections.Generic;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.Documents;
using Sourceportal.Domain.Models.DB.documents;
using Sourceportal.Domain.Models.DB.shared;

namespace Sourceportal.DB.Documents
{
    public interface IDocumentsRepository
    {
        bool SaveDocument(DocumentRequest document);
        IList<DocumentDb> GetDocuments(ObjectType type, int objectId, int? rowLimit, int? rowOffset, int DocumentTypeID, string sortCol, bool descSort, bool isSystem);
        
        bool DeleteDocument(int documentId);
        bool SaveDocumentName(int documentId, string documentName);
        BaseDbResult SaveDocumentExternalId(int documentId, string externalId);


    }
}
