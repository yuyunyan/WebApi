using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.Ownership;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using Sourceportal.Domain.Models.API.Responses.Ownership;

namespace SourcePortal.Services.Ownership
{
    public class OwnershipService : IOwnershipService
    {
        private readonly IOwnershipRepository _ownershipRepository;

        public OwnershipService(IOwnershipRepository ownershipRepository)
        {
            _ownershipRepository = ownershipRepository;
        }

        public GetOwnershipResponse GetOwnership(GetOwnershipRequest getOwnershipRequest)
        {
            var response = new GetOwnershipResponse();

            var dbOwners = _ownershipRepository.GetObjectOwnership(getOwnershipRequest);
            var owners = new List<Owner>();

            foreach (var o in dbOwners)
            {
                owners.Add(new Owner
                {
                    Name = o.OwnerFirstName + " " + o.OwnerLastName,
                    OwnerImageURL = o.OwnerImageURL,
                    UserId = o.OwnerId,
                    Percentage = o.Percent,
                    ExternalID = o.ExternalID
                });
            }

            response.ObjectID = getOwnershipRequest.ObjectID;
            response.ObjectTypeID = getOwnershipRequest.ObjectTypeID;
            response.Owners = owners;

            return response;
        }

        public SetOwnershipResponse SetOwnership(SetOwnershipRequest setOwnershipRequest)
        {
            var response = new SetOwnershipResponse();
            var dbOwners = _ownershipRepository.SetObjectOwnership(setOwnershipRequest);
            var owners = new List<Owner>();

            foreach (var o in dbOwners)
            {
                owners.Add(new Owner
                {
                    Name = o.OwnerFirstName + " " + o.OwnerLastName,
                    UserId = o.OwnerId,
                    Percentage = o.Percent
                });
            }
            response.ObjectID = setOwnershipRequest.ObjectID;
            response.ObjectTypeID = setOwnershipRequest.ObjectTypeID;
            response.Owners = owners;
            response.IsSuccess = true;

            return response;
        }
    }
}
