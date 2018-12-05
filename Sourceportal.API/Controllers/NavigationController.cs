using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Sourceportal.Domain.Models.API.Responses.Security;
using SourcePortal.Services.Navigation;

namespace Sourceportal.API.Controllers
{
    public class NavigationController : ApiController
    {
        private readonly INavigationService _navigationService;

        public NavigationController(INavigationService NavigationService)
        {
            _navigationService = NavigationService;
        }

        [Authorize]
        [Route("api/navigations/getNavList")]
        [HttpGet]
        public NavigationsGetResponse NavigationListGet()
        {
            return _navigationService.NavigationListGet();
        }

        [Authorize]
        [Route("api/navigations/getUserGeneralSecurities")]
        [HttpGet]
        public GeneralSecurityGetResponse GeneralSecuritiesGetResponse()
        {
            return _navigationService.GeneralSecuritiesGet();
        }

        [Authorize]
        [Route("api/navigations/getUserObjectSecurities")]
        [HttpGet]
        public UserObjectSecurityGetResponse UserObjectSecurityGet(int objectId, int objectTypeId)
        {
            return _navigationService.UserObjectSecurityGet(objectId, objectTypeId);
        }

        [Authorize]
        [Route("api/navigations/getUserObjectLevelSecurities")]
        [HttpGet]
        public bool checkUserObjectSecurity(int objectId, int objectTypeId)
        {
            return _navigationService.UserObjectLevelSecurityGet(objectId, objectTypeId);
        }
    }
}