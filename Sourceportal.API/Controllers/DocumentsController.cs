using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.Documents;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Documents;
using Sourceportal.Domain.Models.Shared;
using SourcePortal.Services.Documents;
using SourcePortal.Services.Images;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Sourceportal.API.Controllers
{
    public class DocumentsController : ApiController
    {
        private readonly IDocumentsService _documentsService;
        private readonly IFileService _fileService;
        public DocumentsController(IDocumentsService documentsService, IFileService fileService)
        {
            _documentsService = documentsService;
            _fileService = fileService;

        }

        [Authorize]
        [HttpGet]
        [Route("api/documents/getObjectDocuments")]
        public DocumentListResponse GetObjectDocuments(int objectId, int objectTypeId, int? rowLimit, int rowOffset, string sortCol, bool descSort)
        {
            ObjectType objectType = (ObjectType)objectTypeId;
            //int excludeImages = (int)DocumentType.Other + (int)DocumentType.Spreadsheet + (int)DocumentType.Text;
            var response = _documentsService.GetObjectDocuments(objectId, objectType, rowLimit, rowOffset, 0, sortCol, descSort);
            return _documentsService.GetObjectDocuments(objectId, objectType, rowLimit, rowOffset, 0, sortCol, descSort);
        }

        [Authorize]
        [HttpPost]
        [Route("api/documents/saveDocuments")]
        public BaseResponse SaveDocuments(int objectId, int objectTypeId)
        {
            var path = string.Format(DocumentPaths.UploadedDocuments, objectTypeId, objectId );
            string errorMsg = null;
            try
            {
                var status = _fileService.SaveFile(path, (ObjectType)objectTypeId, objectId);
                return new BaseResponse { IsSuccess = true, ErrorMessage = status ? null : "Document save success" };
            }
            catch(Exception ex)
            {
                return new BaseResponse { IsSuccess = false, ErrorMessage = true ? ex.Message : "Document save failed" };
            }

        }

        [Authorize]
        [HttpGet]
        [Route("api/documents/downloadDocument")]
        public DocumentDownloadResponse DownloadDocument(string path, string filename)
        {
            string newPath = "";
            bool success = _fileService.CopyDocument(path, filename, ref newPath);

            return new DocumentDownloadResponse() { DownloadName = filename, Success = success };
        }

        [Authorize]
        [HttpPost]
        [Route("api/documents/deleteDocument")]
        public bool DeleteDocument(int documentId)
        {
            return _fileService.DeleteDocument(documentId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/documents/saveDocumentName")]
        public bool SaveDocumentName(int documentId, string documentName)
        {
            return _fileService.SaveDocumentName(documentId, documentName);
        }

    }
}
