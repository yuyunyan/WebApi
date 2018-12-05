using Sourceportal.DB.CommonData;
using Sourceportal.DB.Items;
using Sourceportal.DB.OrderFillment;
using Sourceportal.DB.QC;
using Sourceportal.DB.SalesOrders;
using Sourceportal.Domain.Models.API.Requests.ItemStock;
using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.Enums;
using Sourceportal.Domain.Models.Middleware.OrderFulfillment;
using Sourceportal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.Enum;

namespace SourcePortal.Services.OrderFulfillment.InventoryAllocation
{
    public class InventoryAllocationSyncRequestCreator : IInventoryAllocationSyncRequestCreator
    {
        private readonly IOrderFillmentRepository _inventoryRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ICommonDataRepository _commonDataRepository;
        private ISalesOrderRepository _salesOrderRepository;
        private readonly IInspectionRepository _iInspectionRepository;

        public InventoryAllocationSyncRequestCreator(IOrderFillmentRepository inventoryRepository, ISalesOrderRepository salesOrderRepository, IItemRepository itemRepository,
            ICommonDataRepository commonDataRepository, IInspectionRepository iInspectionRepository)
        {
            _inventoryRepository = inventoryRepository;
            _salesOrderRepository = salesOrderRepository;
            _itemRepository = itemRepository;
            _commonDataRepository = commonDataRepository;
            _iInspectionRepository = iInspectionRepository;
        }

        public MiddlewareSyncRequest<InventoryAllocateSync> Create(int soLineId, int stockId, int qty, bool isDeleted)
        {
            var syncRequest = new MiddlewareSyncRequest<InventoryAllocateSync>(
                stockId, 
                MiddlewareObjectTypes.InvAlloc.ToString(), 
                UserHelper.GetUserId(),
                (int)ObjectType.Inventory);
            var iaSync = InventoryAllocateSync(soLineId, stockId, qty, isDeleted);
            syncRequest.Data = iaSync;
            return syncRequest;
        }

        public MiddlewareSyncRequest<CreateStockRequestForSap> CreateForInspection(int inspectionId, int stockId, List<int> stocksOnInspection)
        {
            var syncRequest = new MiddlewareSyncRequest<CreateStockRequestForSap>(
                stockId, 
                MiddlewareObjectTypes.InvAlloc.ToString(), 
                UserHelper.GetUserId(),
                (int)ObjectType.Inventory);
            var iaSync = SyncStocksFromInspection(inspectionId, stockId, stocksOnInspection);
            syncRequest.Data = iaSync;
            return syncRequest;
        }

        private CreateStockRequestForSap SyncStocksFromInspection(int inspectionId, int originalStockId, List<int> stocksOnInspection)
        {
            var csSync = new CreateStockRequestForSap(originalStockId, null);

            var originalStockDetails = _inventoryRepository.GetItemStock(originalStockId);

            csSync.OriginalItemStockExternalID = originalStockDetails.ExternalID;
            csSync.originalWarehouseExternalID = _inventoryRepository.GetWarehouseExternalIdFromBin(originalStockDetails.WarehouseBinID);
            csSync.qcBinExternalUUID = _inventoryRepository.GetQcBinUUIDFromWarehouseBin(originalStockDetails.WarehouseBinID);
            csSync.OriginalItemStock = new StockDetailsRequest();
            csSync.OriginalItemStock.BinExternalID = _inventoryRepository.GetWarehouseBinExternalId(originalStockDetails.WarehouseBinID);
            csSync.OriginalItemStock.ItemStockExternalID = originalStockDetails.ExternalID;
            csSync.OriginalItemStock.QtyToAllocate = originalStockDetails.Qty;
            csSync.OriginalItemStock.MaterialId = _itemRepository.GetItemExternalById(originalStockDetails.ItemID);
            csSync.OriginalItemStock.ExpirationDataTime = originalStockDetails.Expiry;
            csSync.OriginalItemStock.CountryOfOrigin = _commonDataRepository.GetCountry((int)originalStockDetails.COO).First().CountryCode2;
            csSync.OriginalItemStock.ManuDateCode = originalStockDetails.DateCode;
            csSync.OriginalItemStock.LocalId = originalStockId;

            int totalQtySplit = 0;

            if (stocksOnInspection != null && stocksOnInspection.Count > 0)
            {
                csSync.NewItemStocks = new List<StockDetailsRequest>();
                foreach (var newStock in stocksOnInspection)
                {
                    if (newStock != originalStockId)
                    {
                        var createNewStock = new StockDetailsRequest();
                        var newStockDetails = _inventoryRepository.GetItemStock(newStock);
                        var breakdownList = _iInspectionRepository.GetItemStockBreakdownList(newStock);
                        var breakdownQtyList = breakdownList != null ? breakdownList.Select(x => x.PackQty * x.NumPacks).ToList() : null;
                        int breakdownTotalQty = 0;
                        foreach (var breakdown in breakdownQtyList)
                        {
                            breakdownTotalQty += breakdown;
                            totalQtySplit += breakdown;
                        }
                        createNewStock.BinExternalID = _inventoryRepository.GetWarehouseBinExternalId(newStockDetails.WarehouseBinID);
                        createNewStock.QtyToAllocate = breakdownTotalQty;
                        createNewStock.MaterialId = _itemRepository.GetItemExternalById(newStockDetails.ItemID);
                        createNewStock.ExpirationDataTime = newStockDetails.Expiry;
                        createNewStock.CountryOfOrigin = _commonDataRepository.GetCountry((int)newStockDetails.COO).First().CountryCode2;
                        createNewStock.ManuDateCode = newStockDetails.DateCode;
                        //createNewStock.ROHS = newStockDetails.
                        createNewStock.LocalId = newStock;
                        //createNewStock.ProductSpecId = _purchasenewStockDetails.POLineID _salesOrderRepository.GetProductSpecId(soLineId)
                        csSync.NewItemStocks.Add(createNewStock);
                    }
                }
            }

            csSync.OriginalItemStock.QtyToAllocate -= totalQtySplit;

            return csSync;
        }

        private InventoryAllocateSync InventoryAllocateSync(int soLineId, int stockId, int qty, bool isDeleted)
        {
            var iaSync = new InventoryAllocateSync(stockId, null);

            iaSync.IdentifiedStockExternalId = _inventoryRepository.GetItemStock(stockId).ExternalID;
            iaSync.ProductSpecId = isDeleted ? null : _salesOrderRepository.GetProductSpecId(soLineId);
            iaSync.SplitLines = SplitLines(stockId, qty);

            return iaSync;
        }

        private List<SplitLineDetails> SplitLines(int stockId, int qty)
        {
            List<SplitLineDetails> lines = new List<SplitLineDetails>();
            var inventoryList = _inventoryRepository.GetItemInventoryOnStock(stockId);
            int qtyOnStock = _inventoryRepository.GetQtyOfInventoryOnStock(stockId);

            if(qtyOnStock == qty)
            {
                foreach(var inventory in inventoryList)
                {
                    var splitLine = new SplitLineDetails();
                    splitLine.Qty = inventory.Qty;
                    splitLine.BinExternalId = inventory.WarehouseBinExternalId;
                    splitLine.BinExternalUUID = inventory.WarehouseBinExternalUUID;
                    splitLine.WarehouseExternalId = inventory.WarehouseExternalId;
                    lines.Add(splitLine);
                }

                return lines;
            }


            int qtyOnSplitLines = 0;
            foreach(var inventory in inventoryList)
            {
                if(qtyOnSplitLines == qty)
                {
                    return lines;
                }

                var splitLine = new SplitLineDetails();

                if(inventory.Qty > (qty - qtyOnSplitLines))
                {
                    splitLine.Qty = (qty - qtyOnSplitLines);
                }
                else
                {
                    splitLine.Qty = inventory.Qty;
                }

                splitLine.BinExternalId = inventory.WarehouseBinExternalId;
                splitLine.BinExternalUUID = inventory.WarehouseBinExternalUUID;
                splitLine.WarehouseExternalId = inventory.WarehouseExternalId;

                qtyOnSplitLines += (int)splitLine.Qty;
                lines.Add(splitLine);
            }

            return lines;
        }


    }
}
