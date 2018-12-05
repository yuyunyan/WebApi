using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.ErrorLog;
using Sourceportal.Domain.Models.DB.ErrorManagement;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.DB.ErrorManagementService
{
    public interface IErrorManagementRepository
    {
        int SaveLogDb(ExceptionLogSave excLogSave);
        List<ErrorLogDb> ErrorLogListGet(ErrorLogListRequest errorLogListRequest);
        ErrorLogDetailDb ErrorLogDetailGet(int errorId);
    }
}
