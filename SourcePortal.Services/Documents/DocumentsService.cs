using Sourceportal.DB.Documents;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.Documents
{
    public class DocumentsService: IDocumentsService
    {
        private readonly IDocumentsRepository _documentsRepository;


        public DocumentsService(IDocumentsRepository DocumentsRepository)
        {
            _documentsRepository = DocumentsRepository;
        }

        public DocumentListResponse GetObjectDocuments(int objectId, ObjectType type, int? rowLimit, int? rowOffset, int DocumentTypeID, string sortCol, bool descSort)
        {
            var result = _documentsRepository.GetDocuments(type, objectId, rowLimit, rowOffset, DocumentTypeID, sortCol, descSort, false);
            var list = new List<DocumentResponse>();
            var rowCount = 0;
            foreach (var value in result)
            {
                list.Add(new DocumentResponse
                {
                    DocumentID = value.DocumentId,
                    ObjectID = value.ObjectID,
                    ObjectTypeID = value.ObjectTypeID,
                    DocName = value.DocName,
                    FileNameOriginal = value.FileNameOriginal,
                    FileNameStored = value.FileNameStored,
                    FolderPath = value.FolderPath,
                    FileMimeType = value.FileMimeType,
                    Created = value.Created
                });
            }
            if (result.Count() > 0)
            {
                rowCount = result[0].TotalRows;
            }
            return new DocumentListResponse { Documents = list, RowCount = rowCount };
        }
    }
}
