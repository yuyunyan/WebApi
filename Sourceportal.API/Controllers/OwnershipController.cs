using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Routing;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.API.Responses.Ownership;
using SourcePortal.Services.Ownership;

namespace Sourceportal.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OwnershipController : ApiController
    {
        private readonly IOwnershipService _ownershipService;

        public OwnershipController(IOwnershipService ownershipService)
        {
            _ownershipService = ownershipService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/ownership/getObjectOwnership")]
        public GetOwnershipResponse GetObjectOwnership(GetOwnershipRequest getOwnershipRequest)
        {
            return _ownershipService.GetOwnership(getOwnershipRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/ownership/setObjectOwnership")]
        public SetOwnershipResponse SetObjectOwnership(SetOwnershipRequest setOwnershipRequest)
        {
            return _ownershipService.SetOwnership(setOwnershipRequest);
        }
    }
}
