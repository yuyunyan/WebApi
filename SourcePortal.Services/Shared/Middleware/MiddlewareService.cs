using System;
using System.Collections.Specialized;
using System.Drawing.Printing;
using System.Linq;
using Sourceportal.DB.Enum;
using Sourceportal.DB.User;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.Enums;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using SourcePortal.Services.ApiService;
using SourcePortal.Services.Ownership;

namespace SourcePortal.Services.Shared.Middleware
{
    public class MiddlewareService : IMiddlewareService
    {

        protected readonly IRestClient _restClient;
        protected readonly IOwnershipService _ownershipService;
        protected readonly IUserRepository _userRepository;

        public MiddlewareService(IRestClient restClient, IOwnershipService ownershipService, IUserRepository userRepository)
        {
            _restClient = restClient;
            _ownershipService = ownershipService;
            _userRepository = userRepository;
        }

        public SyncResponse Sync<T>(MiddlewareSyncRequest<T> rqeuest) where T: MiddlewareSyncBase
        {
            if (IsTherePendingTransactions(rqeuest.ObjectType, rqeuest.ObjectId, rqeuest.Data.ExternalId))
            {
                return new SyncResponse { ErrorMessage = "One or more transactions are pending for this object, please try again later" };
            }

            SetOwnersAndCreator(rqeuest);
            var middlewareSyncResponse = _restClient.Post<MiddlewareSyncRequest<T>, SyncResponse>("transactions/add", rqeuest);

            if (!string.IsNullOrEmpty(middlewareSyncResponse.ErrorMessage))
            {
                var errorMessage = string.Format("Middleware error occured: {0}", middlewareSyncResponse.ErrorMessage);
                throw new GlobalApiException(errorMessage, ApplicationType.Middleware.ToString());
            }
            return middlewareSyncResponse;
            
        }

        public TResponse SynchronousSync<T, TResponse>(MiddlewareSyncRequest<T> rqeuest, string path) where T : MiddlewareSyncBase where TResponse : BaseResponse
        {
            SetOwnersAndCreator(rqeuest);

            var middlewareSyncResponse = _restClient.Post<MiddlewareSyncRequest<T>, TResponse>(path, rqeuest);

            if (!string.IsNullOrEmpty(middlewareSyncResponse.ErrorMessage))
            {
                var errorMessage = string.Format("Middleware error occured: {0}", middlewareSyncResponse.ErrorMessage);
                //throw new GlobalApiException(errorMessage, ApplicationType.Middleware.ToString());
            }
            return middlewareSyncResponse;
        }

        public bool IsTherePendingTransactions(string objectType, int objectId, string externalId)
        {
            var response = _restClient.Get<bool>(
                "transaction/checkPending",
                new NameValueCollection
                {
                    {"objectType", objectType},
                    {"objectId", objectId.ToString()},
                    { "externalId", !string.IsNullOrEmpty(externalId)? externalId:"" }
                });

            return response;
            
        }

        private void SetOwnersAndCreator<T>(MiddlewareSyncRequest<T> request) where T : MiddlewareSyncBase
        {
            SetOwnersAndCreatorData(request);           
        }

        protected void SetOwnersAndCreatorData<T>(MiddlewareSyncRequest<T> request) where T : MiddlewareSyncBase
        {

            var ownership = _ownershipService.GetOwnership(new GetOwnershipRequest
            {
                ObjectTypeID = request.ObjectTypeId,
                ObjectID = request.ObjectId
            });

            var ownershipString = string.Join(",", ownership.Owners.Select(x => x.Name));
            ownershipString = ownershipString.TrimEnd(',');
            request.Owners = ownershipString;

            var requestCreator = _userRepository.GetUserData(request.CreatedBy);
            
            request.Creator = requestCreator != null ? requestCreator.FirstName + " " + requestCreator.LastName : "";
        }


        //New Methods
        public SyncResponse Sync<T>(T rqeuest, string path) where T : MiddlewareSyncRequest
        {
            if (IsTherePendingTransactions(rqeuest.ObjectType, rqeuest.ObjectId, rqeuest.ExternalId))
            {
                return new SyncResponse { ErrorMessage = "One or more transactions are pending for this object, please try again later" };
            }

            AddOwnershipAndCreatorInformation(rqeuest);
            var middlewareSyncResponse = _restClient.Post<T, SyncResponse>(path, rqeuest);

            if (!string.IsNullOrEmpty(middlewareSyncResponse.ErrorMessage))
            {
                var errorMessage = string.Format("Middleware error occured: {0}", middlewareSyncResponse.ErrorMessage);
                throw new GlobalApiException(errorMessage, ApplicationType.Middleware.ToString());
            }
            return middlewareSyncResponse;

        }

        protected void AddOwnershipAndCreatorInformation<T>(T request) where T : MiddlewareSyncRequest
        {

            var ownership = _ownershipService.GetOwnership(new GetOwnershipRequest
            {
                ObjectTypeID = request.ObjectTypeId,
                ObjectID = request.ObjectId
            });

            var ownershipString = string.Join(",", ownership.Owners.Select(x => x.Name));
            ownershipString = ownershipString.TrimEnd(',');
            request.Owners = ownershipString;

            var requestCreator = _userRepository.GetUserData(request.CreatedBy.Id);

            if (requestCreator != null)
            {
                request.CreatedBy.Name = requestCreator != null ? requestCreator.FirstName + " " + requestCreator.LastName : "";
                request.CreatedBy.Email = requestCreator.EmailAddress;
            }
        }

    }
}
