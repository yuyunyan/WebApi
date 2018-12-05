using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Sourceportal.Utilities;

namespace Sourceportal.API.App_Start
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var url = actionExecutedContext.Request;
            var requestUser = UserHelper.GetUserId();
            var exc = actionExecutedContext.Exception;
            //if (exc is InvalidOperationException)
            //{
            //    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            //}
            base.OnException(actionExecutedContext);
            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //{
            //    Content = new StringContent(actionExecutedContext.Exception.Message),
            //    ReasonPhrase = "Exception"
            //});
        }
    }
}