using System.Linq;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using Sourceportal.Domain.Models.Middleware.Owners;
using SourcePortal.Services.Ownership;

namespace SourcePortal.Services.Shared.Middleware
{
    public class SyncOwnershipCreator : ISyncOwnershipCreator
    {
        private readonly IOwnershipService _ownershipService;

        public SyncOwnershipCreator(IOwnershipService ownershipService)
        {
            _ownershipService = ownershipService;
        }

        public SyncOwnership Create(int soId, ObjectType objectType)
        {
            var ownership = _ownershipService.GetOwnership(new GetOwnershipRequest
                {
                    ObjectID = soId,
                    ObjectTypeID = (int)objectType
            });

            var ownerShipByDescending = ownership.Owners.OrderByDescending(x => x.Percentage);

            var leadOwner = ownerShipByDescending.FirstOrDefault();
            var secondOwner = ownerShipByDescending.Skip(1).FirstOrDefault();

            return new SyncOwnership
            {
                LeadOwner = CreateSyncOwner(leadOwner),
                SecondOwner = secondOwner != null ? CreateSyncOwner(secondOwner) : null
            };
        }

        private static SyncOwner CreateSyncOwner(Owner owner)
        {
            return new SyncOwner
            {
                Id = owner.UserId,
                ExternalId = owner.ExternalID,
                Percentage = owner.Percentage,
                Name = owner.Name

            };
        }
    }
}
