using Sourceportal.Domain.Models.API.Requests.ErrorLog;
using Sourceportal.Domain.Models.API.Responses.ErrorLog;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace SourcePortal.Services.ErrorManagement
{
    public interface IErrorManagementService
    {
        void LoggingError(ExceptionDTO exceptionDto);
        ErrorLogListResponse ErrorLogListGet(ErrorLogListRequest errorLogListRequest);
        ErrorLogDetailResponse ErrorLogDetailGet(int errorId);

        int SapExceptionLogAndEmail(LogToDbRequest logToDbRequest);
    }
}
