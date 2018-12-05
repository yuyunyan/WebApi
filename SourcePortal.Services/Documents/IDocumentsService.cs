using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.Documents
{
    public interface IDocumentsService
    {
        DocumentListResponse GetObjectDocuments(int objectId, ObjectType type, int? rowLimit, int? rowOffset, int documentTypeID, string sortCol, bool descSort);
    }
}
