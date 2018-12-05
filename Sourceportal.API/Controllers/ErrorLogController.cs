using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.ErrorLog;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.BOMs;
using Sourceportal.Domain.Models.API.Responses.ErrorLog;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using SourcePortal.Services;
using SourcePortal.Services.ErrorManagement;

namespace Sourceportal.API.Controllers
{
    public class ErrorLogController : ApiController
    {
        private readonly IErrorManagementService _errorManagementService;

        public ErrorLogController(IErrorManagementService errorManagementService)
        {
            _errorManagementService = errorManagementService;
        }

        [HttpPost]
        [Route("api/error-logging/logToDb")]
        public BaseResponse LogAngularErrorToDb(LogToDbRequest request)
        {
            var errorLogDto = new ExceptionDTO
            {
                ApplicationId = (int) ApplicationType.Angular,
                Exception = new Exception(request.ErrorMessage)
            };

            _errorManagementService.LoggingError(errorLogDto);
            return new BaseResponse
            {
                ErrorMessage = null,
                IsSuccess = true
            };
        }

        [HttpPost]
        [Route("api/error-logging/exception")]
        public ErrorLogResponse LogErrorToDb(LogToDbRequest request)
        {
            
            var errorId = _errorManagementService.SapExceptionLogAndEmail(request);
            
            return new ErrorLogResponse
            {
                ErrorID = errorId
            };
        }

        [HttpGet]
        [Route("api/error-logging/errorLogList")]
        public ErrorLogListResponse ErrorLogListGet([FromUri] ErrorLogListRequest errorLogListRequest)
        {
            return _errorManagementService.ErrorLogListGet(errorLogListRequest);
        }

        [HttpGet]
        [Route("api/error-logging/errorLogDetail")]
        public ErrorLogDetailResponse ErrorLogDetailGet(int errorId)
        {
            return _errorManagementService.ErrorLogDetailGet(errorId);
            
        }
    }
}