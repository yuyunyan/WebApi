using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.API.Responses.Ownership;

namespace SourcePortal.Services.Ownership
{
    public interface IOwnershipService
    {
        GetOwnershipResponse GetOwnership(GetOwnershipRequest getOwnershipRequest);
        SetOwnershipResponse SetOwnership(SetOwnershipRequest setOwnershipRequest);
    }
}
