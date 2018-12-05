using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Configuration;
using Sourceportal.DB.Documents;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.Documents;
using Sourceportal.Domain.Models.API.Responses.Images;
using Sourceportal.Domain.Models.Shared;
using File = Sourceportal.Domain.Models.API.Responses.Images.File;
namespace SourcePortal.Services.Images
{
    public class FileService : IFileService
    {
        private readonly IDocumentsRepository _documentsRepository;
        private static readonly string ImagesDirectoryName = WebConfigurationManager.AppSettings["DocumentsFolder"];

        public FileService(IDocumentsRepository documentsRepository)
        {
            _documentsRepository = documentsRepository;
        }

        public bool SaveFile(string relativePath, ObjectType objectType, int objectId, bool imagesOnly = false)
        {
            var httpRequest = HttpContext.Current.Request;
            var status = true;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string fileName in httpRequest.Files.Keys)
                {
                    try
                    {
                        var file = httpRequest.Files[fileName];
                        string folderPath = "";
                        bool isImage = IsImageType(file);
                        var fileNameToSave = CreateFileNameToSave(file, isImage);
                        SaveFileToFolder(relativePath, fileNameToSave, file, isImage);
                        SaveFiletoDatabase(relativePath, fileNameToSave, file, objectType, objectId);
                    }
                    catch (Exception ex)
                    {
                        status = false;
                    }
                }
            }
            return status;
        }

        public bool IsImageType(HttpPostedFile file)
        {
            return file.ContentType.Contains("image");
        }

        public bool RequestContainsImages()
        {

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string fileName in httpRequest.Files.Keys)
                {
                    try
                    {
                        var file = httpRequest.Files[fileName];
                        if (!IsImageType(file))
                            return false;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }

            }
            else
                return false;
            //Success
            return true;
        }
        public bool CopyDocument(string oldPath, string newFileName, ref string outputPath)
        {
            var fullOldDirectoryPath = HttpContext.Current.Server.MapPath(oldPath);
            var fullNewDirectoryPath = HttpContext.Current.Server.MapPath("~/" + ImagesDirectoryName + "/Downloads/" + newFileName);

            if (System.IO.File.Exists(fullOldDirectoryPath))
            {
                System.IO.File.Copy(fullOldDirectoryPath, fullNewDirectoryPath, true);
                if (System.IO.File.Exists(fullNewDirectoryPath))
                {
                    outputPath = fullNewDirectoryPath;
                    return true;
                }
                
            }
            return false;
        }
        public FileResponse GetFiles(string relativePath, ObjectType objectType, int objectId, int DocumentTypeID)
        {
            var documentDb = _documentsRepository.GetDocuments(objectType, objectId, 999999999, null, DocumentTypeID, null, false, false);

            var files = new List<File>();
            foreach (var document in documentDb)
            {
                files.Add(new File
                {
                    Id = document.DocumentId,
                    Url = GenerateFullAnswerImagePath(document.FolderPath, document.FileNameStored, relativePath)
                });
            }
            return new FileResponse { Files = files };
        }

        private static void SaveFileToFolder(string directoryToSave, string fileNameToSave, HttpPostedFile file, bool forcePng = false)
        {
            var fullRelativePath = "~/" + ImagesDirectoryName + "/" + directoryToSave;
            
            var fullDirectoryPath = HttpContext.Current.Server.MapPath(fullRelativePath);
            
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            var filePath = HttpContext.Current.Server.MapPath(fullRelativePath + "/" + fileNameToSave);
            if (forcePng)
            {
                //Save original as temporary file
                string tmpFilePath = HttpContext.Current.Server.MapPath(fullRelativePath + "/" + "delme_" + fileNameToSave);
                file.SaveAs(tmpFilePath);

                //Save new file from temp file
                System.Drawing.Image tmpImage = System.Drawing.Bitmap.FromFile(tmpFilePath);
                tmpImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                
                //Delete old file
                tmpImage.Dispose();
                System.IO.File.Delete(tmpFilePath);

            }
            else
            {
                file.SaveAs(filePath);
            }
        }

        private void SaveFiletoDatabase(string folderPath, string fileName, HttpPostedFile file, ObjectType objectType, int objectId)
        {
            var documentRequest = new DocumentRequest
            {
                DocName = "",
                DoucmentID = 0,
                FileMimeType = file.ContentType,
                FileNameOriginal = file.FileName,
                FileNameStored = fileName,
                FolderPath = folderPath,
                ObjectID = objectId,
                ObjectTypeID = (int)objectType
            };
            _documentsRepository.SaveDocument(documentRequest);
        }

        private static string CreateFileNameToSave(HttpPostedFile file, bool isImage = false)
        {
            var guidFileName = Guid.NewGuid();
            var origFileName = file.FileName;
            var origExtention =  origFileName.Split('.')[1];
            var fileNameToSave = guidFileName + "." ;
            
            //force PNG extension
            if(isImage)
                fileNameToSave = fileNameToSave + "png";
            else
                fileNameToSave = fileNameToSave + origExtention;

            return fileNameToSave;
        }

        public string GenerateFullAnswerImagePath(string folderPath, string imageFileNameStored, string relativePath)
        {
            //return ImagesDirectoryName + "/" + relativePath + "/" + imageFileNameStored;
            return ImagesDirectoryName + "/" + folderPath + "/" + imageFileNameStored;
        }

        public string GenerateFullAnswerSaveFolderPath(string imageFileNameStored, string relativePath)
        {
            return ImagesDirectoryName + "/" + relativePath + "/" + imageFileNameStored;
        }
        public bool DeleteDocument(int imageId)
        {
            return _documentsRepository.DeleteDocument(imageId);
        }

        public bool SaveDocumentName(int documentID, string documentName)
        {
            return _documentsRepository.SaveDocumentName(documentID, documentName);
        }
    }
}