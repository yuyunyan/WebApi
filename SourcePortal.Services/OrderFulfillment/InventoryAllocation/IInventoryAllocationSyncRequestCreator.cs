using Sourceportal.Domain.Models.API.Requests.ItemStock;
using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.OrderFulfillment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.OrderFulfillment.InventoryAllocation
{
    public interface IInventoryAllocationSyncRequestCreator
    {
        MiddlewareSyncRequest<InventoryAllocateSync> Create(int soLineId, int inventoryId, int qty, bool isDeleted);
        MiddlewareSyncRequest<CreateStockRequestForSap> CreateForInspection(int inspectionId, int originalStockId, List<int> stocksOnInspection);
    }
}
