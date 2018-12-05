using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.Items
{
    public interface IItemSyncRequestCreator
    {
        MiddlewareSyncRequest<ItemSync> Create(int itemId);
    }
}
