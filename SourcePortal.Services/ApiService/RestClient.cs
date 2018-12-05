using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Sourceportal.Domain.Models.API.Responses;

namespace SourcePortal.Services.ApiService
{
    public class RestClient : IRestClient
    {
        private static readonly string MiddlewareUrl = ConfigurationManager.AppSettings["MiddlewareApiUrl"];

        public TResult Post<TRequestType, TResult>(string path, TRequestType objectToPost) where TResult:BaseResponse
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(MiddlewareUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //var authorizationHeaderKey = "Authorization";
            //var authorizationHeaderValue = HttpContext.Current.Request.Headers[authorizationHeaderKey];
            //client.DefaultRequestHeaders.Add(authorizationHeaderKey, authorizationHeaderValue);
            
            // List data response.
            HttpResponseMessage response = client.PostAsJsonAsync(path, objectToPost).Result;  // Blocking call!

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    var responseObject = response.Content.ReadAsAsync<TResult>().Result;
                    return responseObject;
                }

                return ReturnError<TResult>(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return ReturnError<TResult>(ex.Message);
            }
        }

        public TResponse Get<TResponse>(string path, NameValueCollection queryParams)
        {
            HttpClient client = new HttpClient();

            var url = path + ToQueryString(queryParams);
         
            client.BaseAddress = new Uri(MiddlewareUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            // List data response.
            HttpResponseMessage response = client.GetAsync(url).Result;  // Blocking call!
   
            // Parse the response body. Blocking!
            var responseObject = response.Content.ReadAsAsync<TResponse>().Result;
            return responseObject;
        }

        private static TResult ReturnError<TResult>(string message) where TResult : BaseResponse
        {
            Type objectType = typeof(TResult);
            var instance = (TResult) Activator.CreateInstance(objectType);
            instance.ErrorMessage = message;
            return instance;
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                    from value in nvc.GetValues(key)
                    select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }
}

}
