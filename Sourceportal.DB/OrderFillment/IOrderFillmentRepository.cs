using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.DB.OrderFulfillment;
using Sourceportal.Domain.Models.API.Requests.OrderFulfillment;
using Sourceportal.Domain.Models.DB.ItemStock;

namespace Sourceportal.DB.OrderFillment
{
   public interface IOrderFillmentRepository
    {
        IList<OFListDb> GetOrderfullfillmentList(OrderFulfillmentListSearchFilter searchFilter);
        int GetSoLineIdOnStock(int stockId);

        IList<OFAvailabilityDb> GetOrderFulfillmentAvailability(int soLineId);

        SetOrderFulfillmentQtyDb SetOrderFulfillmentQty(int soLineId, int id, string idType, int qty, bool isDeleted);
        SetOrderFulfillmentQtyDb UpdateExistingInventoryFulfillmentQty(int id, int qty, bool isDeleted);
        ItemInventoryDb GetItemInventoryFromBinExternal(string stockExternalId, string warehouseBinExternalId);
        ItemInventoryDb GetItemInventoryFromCompoundSapKey(string stockExternalId, string warehouseBinExternalId, bool isInspection, bool isRestricted);
        int GetQtyOfInventoryOnStock(int stockId);
        int GetStockIDFromExternal(string externalId);
        string GetWarehouseBinExternalId(int warehouseBinId);

        string GetWarehouseExternalId(int warehouseBinId);

        string GetWarehouseExternalIdFromBin(int warehouseBinId);

        string GetQcBinUUIDFromWarehouseBin(int warehouseBinId);

        IList<SOAllocationDb> GetUnallocatedSOLines(UnallocatedSOLinesGetRequest unallocatedSoLinesGetRequest);

        int SetItemInventory(ItemInventoryDb ii);
        int GetQtyAllocatedForLine(int soLineId);
        int GetStockQtyAllocated(int stockId);

        int SetItemStock(ItemStockDB ist);
        ItemStockDB GetItemStock(int stockId);
        List<ItemStockDB> GetInspectionItemStockList(int inspectionId);
        ItemInventoryDb GetItemInventory(int itemInventoryId);

        List<ItemInventoryDb> GetItemInventoryOnStock(int stockId);

        IList<OFListDb> GetRequestToPurchaseList(RequestToPurchaseListRequest searchFilter);

        int GetWarehouseBinIdByExternalUUID(string externalId);
        int GetWarehouseBinIdByExternalID(string externalId);
        List<SoSWarehouseDetailsDB> GetWarehouseSoSDetails(int soLineId);
        List<SoSWarehouseDetailsDB> GetStockWarehouseSoSDetails(int stockId);
        List<SoSWarehouseDetailsDB> GetPoWarehouseSoSDetails(int poLineId);
        int GetWarehouseBinIdByExternalIdWarehouseExternalId(string externalId, string warehouseExternalId);
        int GetInvStatusIdFromExternal(string externalId);
        int GetInTransitBinByWarehouseExternalUUID(string externalUUID);
        void UpdateItemStock(int inspecitonId);
    }
}
