using System.Collections.Specialized;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.SAP_API.Responses;

namespace SourcePortal.Services.ApiService
{
    public interface IRestClient
    {
        TResult Post<T, TResult>(string path, T objectToPost) where TResult : BaseResponse;
        TResponse Get<TResponse>(string path, NameValueCollection queryParams);
    }
}