using System;
using System.Collections.Generic;
using Dapper;
using Sourceportal.Domain.Models.API.Requests.Documents;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Sourceportal.DB.Enum;
using Sourceportal.DB.QC;
using Sourceportal.Domain.Models.API.Responses.Images;
using Sourceportal.Domain.Models.DB.documents;
using Sourceportal.Domain.Models.DB.shared;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.DB.Documents
{
    public class DocumentsRepository : IDocumentsRepository
    {
        
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;
        public bool SaveDocument(DocumentRequest document)
        {
            int ret = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@DocumentID", document.DoucmentID, direction: ParameterDirection.InputOutput);
                param.Add("@ObjectTypeID", document.ObjectTypeID);
                param.Add("@ObjectID", document.ObjectID);
                param.Add("@DocName", document.DocName);
                param.Add("@FileNameOriginal", document.FileNameOriginal);
                param.Add("@FileNameStored", document.FileNameStored);
                param.Add("@FileMimeType", document.FileMimeType);
                param.Add("@FolderPath", document.FolderPath);
                param.Add("@IsDeleted", document.IsDeleted);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                con.Query("uspDocumentSet", param, commandType: CommandType.StoredProcedure);
                
                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", DocumentDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }
            return true;
        }
        
        public IList<DocumentDb> GetDocuments(ObjectType type, int objectId, int? rowLimit, int? rowOffset, int DocumentTypeID, string sortCol, bool descSort, bool isSystem)
        {
            IList<DocumentDb> documents;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ObjectTypeID", (int)type);
                param.Add("@ObjectID", objectId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                if (rowOffset > 0)
                    param.Add("@RowOffset", rowOffset);
                if (rowLimit > 0)
                    param.Add("@RowLimit", rowLimit);
                if (DocumentTypeID > 0)
                    param.Add("@DocumentTypeID", DocumentTypeID);

                param.Add("@SortBy", sortCol);
                param.Add("@DescSort", descSort);
                param.Add("@IsSystem", isSystem);

                documents = con.Query<DocumentDb>("uspDocumentsGet", param, commandType: CommandType.StoredProcedure).ToList();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)   
                {
                    var errorMessage = string.Format("Database error occured: {0}", DocumentDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }

            return documents;
        }

        public bool DeleteDocument(int documentId)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
               
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@DocumentID", documentId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                con.Query("uspDocumentDelete", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", DocumentDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
                return true;
            }
        }

        public bool SaveDocumentName(int documentId, string documentName)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@DocumentID", documentId);
                param.Add("@DocName", documentName);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                con.Query("uspDocumentSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", DocumentDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
                return true;
            }
        }
        
        public BaseDbResult SaveDocumentExternalId(int documentId, string externalId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DocumentID", documentId);
            param.Add("@ExternalID", externalId);

            return DbCommonFunctions.ExecuteStoreProcedure(param, "uspDocumentSet", DocumentDbErrors.ErrorCodes);
        }


        
    }
}
