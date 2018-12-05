using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses.Images;

namespace SourcePortal.Services.Images
{
    public interface IFileService
    {
        bool SaveFile(string relativePath, ObjectType objectype, int objectId, bool imageOnly = false);
        string GenerateFullAnswerImagePath(string folderPath, string imageFileNameStored, string relativePath);
        string GenerateFullAnswerSaveFolderPath(string imageFileNameStored, string relativePath);
        bool DeleteDocument(int imageId);
        bool SaveDocumentName(int documentId, string documentName);
        FileResponse GetFiles(string relativePath, ObjectType objectType, int objectId, int DocumentTypeID);
        bool CopyDocument(string oldPath, string fileName, ref string outputPath);
        bool RequestContainsImages();
    }
}