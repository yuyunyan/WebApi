using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.DB.Accounts;

namespace Sourceportal.DB.Ownership
{
    public interface IOwnershipRepository
    {
        IList<OwnerDb> GetObjectOwnership(GetOwnershipRequest getOwnershipRequest);
        IList<OwnerDb> SetObjectOwnership(SetOwnershipRequest setOwnershipRequest);
    }
}
