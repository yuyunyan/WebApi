using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.API.ErrorManagement
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exc = context.Exception;
            if (exc is GlobalApiException)
            {
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(exc.Message),
                    ReasonPhrase = !string.IsNullOrEmpty(exc.Source) && exc.Source == ApplicationType.Middleware.ToString() ? "Api Error" : "Db Error"
                };

                context.Result = new DatabaseErrorResult(context.Request, result);
            }
            else
            {
                base.Handle(context);
            }
        }

        public class DatabaseErrorResult : IHttpActionResult
        {
            private HttpResponseMessage _responseMessage;
            private HttpRequestMessage _request;

            public DatabaseErrorResult(HttpRequestMessage request, HttpResponseMessage responseMessage)
            {
                _request = request;
                _responseMessage = responseMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_responseMessage);
            }
        }
    }
}