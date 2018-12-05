using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.DB.ErrorManagementService;
using Sourceportal.Domain.Models.API.Requests.ErrorLog;
using Sourceportal.Domain.Models.API.Responses.ErrorLog;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;

namespace SourcePortal.Services.ErrorManagement
{
    public class ErrorManagementService : IErrorManagementService
    {
        private readonly IErrorManagementRepository _errorManagementRepository;
        public ErrorManagementService(IErrorManagementRepository errorManagementRepository)
        {
            _errorManagementRepository = errorManagementRepository;
        }

        public void LoggingError(ExceptionDTO exceptionDto)
        {
            if (!(exceptionDto.Exception is HttpResponseException))
            {
                var httpRequestInputStream = HttpContext.Current.Request.InputStream;
                var postData = new StreamReader(httpRequestInputStream).ReadToEnd();
                //SaveLogLocal(exceptionDto, postData);
                SaveLogDb(exceptionDto, postData);
            }
        }

        public ErrorLogListResponse ErrorLogListGet(ErrorLogListRequest errorLogListRequest)
        {
            var errorLogListDb = _errorManagementRepository.ErrorLogListGet(errorLogListRequest);
            var errorLogList = new List<ErrorLogResponse>();
            foreach (var errorLogDb in errorLogListDb)
            {
                var errorLogResponse = new ErrorLogResponse
                {
                    Application = errorLogDb.Application,
                    ErrorID = errorLogDb.ErrorID,
                    ErrorMessage = errorLogDb.ErrorMessage,
                    ExceptionType = errorLogDb.ExceptionType,
                    Timestamp = errorLogDb.Timestamp,
                    URL = errorLogDb.URL,
                    User = $"{errorLogDb.FirstName} {errorLogDb.LastName}",
                    
                };
                errorLogList.Add(errorLogResponse);
            }
            int rowCount = 0;

            if (errorLogListDb.Count > 0)
            {
                rowCount = errorLogListDb[0].TotalRows;
            }
            return new ErrorLogListResponse
            {
                ErrorLogList = errorLogList,
                TotalRowCount = rowCount,
                IsSuccess = true
            };
        }

        public ErrorLogDetailResponse ErrorLogDetailGet(int errorId)
        {
            var errorDetailDb = _errorManagementRepository.ErrorLogDetailGet(errorId);
            return new ErrorLogDetailResponse
            {
                Application = errorDetailDb.Application,
                ErrorMessage = errorDetailDb.ErrorMessage,
                ExceptionType = errorDetailDb.ExceptionType,
                ErrorID = errorDetailDb.ErrorID,
                InnerExceptionMessage = errorDetailDb.InnerExceptionMessage,
                PostData = errorDetailDb.PostData,
                StackTrace = errorDetailDb.StackTrace,
                Timestamp = errorDetailDb.Timestamp,
                URL = errorDetailDb.URL,
                User = $"{errorDetailDb.FirstName} {errorDetailDb.LastName}"
            };
        }

        private void SaveLogLocal(ExceptionDTO exceptionDto, string postData)
        {
            var relativePath = "~/Documents/ErrorLog.log";
            var filePath = HttpContext.Current.Server.MapPath(relativePath);
            var sw = new StreamWriter(filePath, false);
            sw.WriteLine("TimeStamp: {0}", DateTime.Now);
            sw.WriteLine("URL: {0}", exceptionDto.Request?.RequestUri.ToString() ?? "");
            sw.WriteLine("PostedData:");
            sw.WriteLine("{0}", postData);
            sw.WriteLine("UserID: {0}", UserHelper.GetUserId());
            sw.WriteLine("Error Message: {0}", exceptionDto.Exception.Message);
            sw.WriteLine("StackTrace: {0}", exceptionDto.Exception.StackTrace);
            sw.WriteLine("Exception Type: {0}", exceptionDto.Exception.GetType());
            if (exceptionDto.Exception.InnerException != null)
            {
                sw.WriteLine("InnerException:");
                sw.WriteLine("InnerException Message: {0}",exceptionDto.Exception.InnerException.Message);
            }
            sw.WriteLine("Application: API");
            
            sw.Close();
        }

        private void SaveLogDb(ExceptionDTO exceptionDto, string postData)
        {
            var exc = exceptionDto.Exception;
            var type = (exc.GetType().ToString() == "System.Exception") ? "Angular Error" : exc.GetType().ToString();
            var applicationId = exceptionDto.ApplicationId ?? (int) ApplicationType.WebApi;
            var url = exceptionDto.Request?.RequestUri.ToString();
            var innerException = exc.InnerException != null ? exc.InnerException.Message : null;

            SaveException(exc.Message, type, postData, exc.StackTrace, url, applicationId, innerException);
            
        }

        private ExceptionDto SaveException(string errorMessage, string exceptionType, string postData, string stackTrace,
            string url, int applicationId, string innerException)
        {
            var exceptionLogSave = new ExceptionLogSave
            {
                ErrorMessage = errorMessage,
                ExceptionType = exceptionType,
                PostData = postData,
                StackTrace = stackTrace,
                UserId = UserHelper.GetUserId(),
                Url = url,
                ApplicationId = applicationId,
                TimeStamp = DateTime.UtcNow,
                InnerException = innerException
            };
            var errorId = _errorManagementRepository.SaveLogDb(exceptionLogSave);
            
            return new ExceptionDto{ErrorId = errorId, Exception = exceptionLogSave};

        }

        public int SapExceptionLogAndEmail(LogToDbRequest logToDbRequest)
        {
            ApplicationType applicationType;
            Enum.TryParse(logToDbRequest.Application, out applicationType);

            var exceptionDto = SaveException(logToDbRequest.ErrorMessage, logToDbRequest.ExceptionType, logToDbRequest.PostData, 
                logToDbRequest.StackTrace, logToDbRequest.Url, (int)applicationType, logToDbRequest.InnerException);

            SendSapErrorEmail(exceptionDto.Exception, exceptionDto.ErrorId);

            return exceptionDto.ErrorId;
        }

        private void SendSapErrorEmail(ExceptionLogSave exceptionLogSave, int errorId)
        {
            var errorsEmail = ConfigurationManager.AppSettings["SapErrorsRecipientEmail"];

            if (!string.IsNullOrEmpty(errorsEmail))
            {
                var env = ConfigurationManager.AppSettings["Env"];
                string body = JsonConvert.SerializeObject(exceptionLogSave);
                string subject = "SAP Error Occured in "+ env + " : Error Id " + errorId;
                Mail.EmailService.SendEmail("noreply-sap-error@sourceability.com", null, errorsEmail, subject, body);
            }
        }

        private class ExceptionDto
        {
            public int ErrorId { get; set; }
            public ExceptionLogSave Exception { get; set; }
        }

    }
}
