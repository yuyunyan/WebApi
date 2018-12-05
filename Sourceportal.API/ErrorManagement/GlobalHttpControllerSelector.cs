using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Sourceportal.DB.ErrorManagementService;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using SourcePortal.Services.ErrorManagement;
using StructureMap;

namespace Sourceportal.API.ErrorManagement
{
    public class GlobalHttpControllerSelector : DefaultHttpControllerSelector
    {
        public readonly IErrorManagementService _errorManagementService;

        public GlobalHttpControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            var container = new Container(x =>
            {
                x.For<IErrorManagementService>().Use<ErrorManagementService>();
                x.For<IErrorManagementRepository>().Use<ErrorManagementRepository>();
            });
            _errorManagementService = container.GetInstance<IErrorManagementService>();
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor descriptor = null;
            try
            {
                descriptor = base.SelectController(request);
            }
            catch (HttpResponseException exc)
            {
                var exceptionDto = new ExceptionDTO()
                {
                    Exception = exc,
                    Request = request
                };
                _errorManagementService.LoggingError(exceptionDto);
                var errorMessage = string.Format("Request to Uri: '{0}' does not match any route.", request.RequestUri);
                throw new GlobalApiException(errorMessage);
            }
            return descriptor;
        }
    }
}