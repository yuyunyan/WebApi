using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Sourceportal.DB.Enum;
using Sourceportal.DB.ErrorManagementService;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using SourcePortal.Services.ErrorManagement;
using StructureMap;

namespace Sourceportal.API.ErrorManagement
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        public readonly IErrorManagementService _errorManagementService;

        public GlobalExceptionLogger()
        {
            var container = new Container(x =>
            {
                x.For<IErrorManagementService>().Use<ErrorManagementService>();
                x.For<IErrorManagementRepository>().Use<ErrorManagementRepository>();
            });
            _errorManagementService = container.GetInstance<IErrorManagementService>();
        }
        
        public override void Log(ExceptionLoggerContext context)
        {
            ApplicationType applicationType;
            var sourceProvided = Enum.TryParse(context.Exception.Source, out applicationType);

            var exceptionDto = new ExceptionDTO()
            {
                Exception = context.Exception,
                Request = context.Request,
                ApplicationId = sourceProvided ? (int)applicationType : default(int?)
            };
            _errorManagementService.LoggingError(exceptionDto);
        }
    }
}