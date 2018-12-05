using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Sourceportal.DB.ErrorManagementService;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using SourcePortal.Services.ErrorManagement;
using StructureMap;

namespace Sourceportal.API.ErrorManagement
{
    public class GlobalHttpActionSelector : ApiControllerActionSelector
    {
        public readonly IErrorManagementService _errorManagementService;
        public GlobalHttpActionSelector()
        {
            var container = new Container(x =>
            {
                x.For<IErrorManagementService>().Use<ErrorManagementService>();
                x.For<IErrorManagementRepository>().Use<ErrorManagementRepository>();
            });
            _errorManagementService = container.GetInstance<IErrorManagementService>();
        }

        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            HttpActionDescriptor descriptor = null;
            try
            {
                descriptor = base.SelectAction(controllerContext);
            }
            catch (HttpResponseException exc)
            {
                var exceptionDto = new ExceptionDTO()
                {
                    Exception = exc,
                    Request = controllerContext.Request
                };
                _errorManagementService.LoggingError(exceptionDto);
                var errorMessage = string.Format("Request to Uri: '{0}' does not match any route.", controllerContext.Request.RequestUri);
                throw new GlobalApiException(errorMessage);
            }

            return descriptor;
        }
    }
}