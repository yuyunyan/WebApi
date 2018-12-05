using System.Collections.Generic;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    public class SetContactOwnershipRequest
    {
        public int ContactId;
        public List<OwnerSetRequest> OwnerList;

    }
}
