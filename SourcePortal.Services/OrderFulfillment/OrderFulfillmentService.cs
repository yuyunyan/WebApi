using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sourceportal.DB.OrderFillment;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses.OrderFulfillment;
using Newtonsoft.Json;
using Sourceportal.DB.PurchaseOrders;
using Sourceportal.Domain.Models.API.Requests.OrderFulfillment;
using Sourceportal.Domain.Models.API.Responses.Sync.SynchronousResponses;
using SourcePortal.Services.OrderFulfillment.InventoryAllocation;
using Sourceportal.Domain.Models.API.Requests.PurchaseOrders;
using Sourceportal.Domain.Models.DB.PurchaseOrders;
using SourcePortal.Services.Shared.Middleware;
using Sourceportal.Domain.Models.Middleware.OrderFulfillment;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using SourcePortal.Services.PurchaseOrders;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Domain.Models.Middleware.PurchaseOrder;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB.OrderFulfillment;
using Sourceportal.DB.Items;
using Sourceportal.DB.SalesOrders;
using Sourceportal.Domain.Models.DB.ItemStock;
using SourcePortal.Services.BOMs;
using Sourceportal.Domain.Models.API.Responses.BOMs;
using Sourceportal.Domain.Models.DB.SalesOrders;
using Sourceportal.DB.CommonData;
using Sourceportal.Domain.Models.Shared;

namespace SourcePortal.Services.OrderFulfillment
{
    public class OrderFulfillmentService:IOrderFulfillmentService
    {
        private readonly IOrderFillmentRepository _orderFillmentRepository;
        private readonly IInventoryAllocationSyncRequestCreator _iaSyncRequestCreator;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IMiddlewareService _middlewareService;
        private readonly IPoSyncRequestCreator _poSyncRequestCreator;
        private readonly IItemRepository _itemRepository;
        private readonly ISalesOrderRepository _soRepo;
        private readonly IBOMsService _bOMsService;
        private readonly ICommonDataRepository _commonDataRepository;

        public OrderFulfillmentService(IOrderFillmentRepository orderFillmentRepository, IInventoryAllocationSyncRequestCreator iaSyncRequestCreator, IPurchaseOrderRepository purchaseOrderRepository
            , IMiddlewareService middlewareService, IPoSyncRequestCreator poSyncRequestCreator, IItemRepository itemRepo, ISalesOrderRepository soRepo,IBOMsService bOMsService,
            ICommonDataRepository commonDataRepository)
        {
            _orderFillmentRepository = orderFillmentRepository;
            _iaSyncRequestCreator = iaSyncRequestCreator;
            _purchaseOrderRepository = purchaseOrderRepository;
            _middlewareService = middlewareService;
            _poSyncRequestCreator = poSyncRequestCreator;
            _itemRepository = itemRepo;
            _soRepo = soRepo;
            _bOMsService = bOMsService;
            _commonDataRepository = commonDataRepository;
        }

        public OrderFulfillmentListResponse RequestToPurchaseListGet(RequestToPurchaseListRequest searchFilter)
        {
            var dborderFulfillment = _orderFillmentRepository.GetRequestToPurchaseList(searchFilter);
            var list = new List<OrderFillmentResponse>();
            var totalCount = 0;
            foreach (var value in dborderFulfillment)
            {
                list.Add(new OrderFillmentResponse
                {
                    ItemId = (int)value.ItemId,
                    SoLineId = value.SoLineId,
                    SOVersionID = value.VersionID,
                    AccountID = value.AccountId,
                    OrderNo = value.SalesOrderId,
                    LineNum = value.LineNum,
                    Customer = value.AccountName,
                    PartNo = value.PartNumber,
                    Mfr = value.MfrName,
                    OrderQty = value.Qty,
                    AllocatedQty = value.AllocatedQty,
                    Price = value.Price,
                    Cost = value.Cost,
                    PackagingName = value.PackagingName,
                    CommodityName = value.CommodityName,
                    DateCode = value.DateCode,
                    ShipDate = value.ShipDate,
                    DueDate = value.DueDate,
                    ExternalID = value.ExternalID,
                    SalesPerson = CleanLists(value.Owner),
                    Comments = value.Comments,
                });
            }
            if (dborderFulfillment.Count() > 0)
            {
                totalCount = dborderFulfillment[0].TotalRows;
            }
            return new OrderFulfillmentListResponse { OrderFillment = list, TotalRowCount = totalCount };
        }

        public OrderFulfillmentListResponse GetOrderFulfillmentList(OrderFulfillmentListSearchFilter searchFilter)
        {
            var dborderFulfillment = _orderFillmentRepository.GetOrderfullfillmentList(searchFilter);
            var list= new List<OrderFillmentResponse>();
            var totalCount = 0;
            foreach (var value in dborderFulfillment)
            {
                list.Add(new OrderFillmentResponse
                {
                    ItemId = (int)value.ItemId,
                    SoLineId = value.SoLineId,
                    SOVersionID = value.VersionID,
                    AccountID = value.AccountId,
                    OrderNo = value.SalesOrderId,
                    LineNum = value.LineNum,
                    Customer = value.AccountName,
                    PartNo = value.PartNumber,
                    Mfr = value.MfrName,
                    OrderQty = value.Qty,
                    Buyers = MapOFBuyerToJsonObject(value.Buyers),
                    AllocatedQty = value.AllocatedQty,
                    Price = value.Price,
                    Cost = value.Cost,
                    PackagingName = value.PackagingName,
                    CommodityName = value.CommodityName,
                    DateCode = value.DateCode,
                    ShipDate = value.ShipDate,
                    DueDate = value.DueDate,
                    SalesPerson = CleanLists(value.Owner),
                    ExternalID = value.ExternalID,
                    Comments = value.Comments,
                });
            }
            if (dborderFulfillment.Count() > 0)
            {
                totalCount = dborderFulfillment[0].TotalRowCount;
            }
            return new OrderFulfillmentListResponse {OrderFillment = list,TotalRowCount = totalCount};
        }

        public SOAllocationListResponse GetUnallocatedSOLines(UnallocatedSOLinesGetRequest unallocatedSoLinesGetRequest)
        {
            var response = new List<SOAllocationResponse>();
            var soLines = _orderFillmentRepository.GetUnallocatedSOLines(unallocatedSoLinesGetRequest);
            foreach (var soAllocationDb in soLines)
            {
                response.Add(new SOAllocationResponse
                {
                    AccountName = soAllocationDb.AccountName,
                    Allocated = soAllocationDb.Allocated,
                    DateCode = soAllocationDb.DateCode,
                    MfrName = soAllocationDb.MfrName,
                    Needed = soAllocationDb.Needed,
                    PartNumber = soAllocationDb.PartNumber,
                    Price = soAllocationDb.Price,
                    Qty = soAllocationDb.Qty,
                    SOLineID = soAllocationDb.SOLineID,
                    SalesOrderID = soAllocationDb.SalesOrderID,
                    SOVersionID = soAllocationDb.SOVersionID,
                    Sellers = CleanLists(soAllocationDb.Sellers),
                    ShipDate = soAllocationDb.ShipDate,
                    StatusName = soAllocationDb.StatusName,
                    LineNum = soAllocationDb.LineNum,
                    ExternalID = soAllocationDb.ExternalID
                });
            }
            return new SOAllocationListResponse
            {
                SOAllocations = response
            };
        }

        public List<OFGridExportLine> MapSOLinesToExport(List<OrderFillmentResponse> soLines)
        {
            var exportsLines = new List<OFGridExportLine>();
            foreach (var soLine in soLines)
            {
                exportsLines.Add(new OFGridExportLine
                {
                    DateCode = soLine.DateCode,
                    PartNumber = soLine.PartNo,
                    AllocatedQty = soLine.AllocatedQty,
                    Buyer = CreateBuyerNameString(soLine.Buyers),
                    Commodity = soLine.CommodityName,
                    ShipDate = soLine.ShipDate,
                    Customer = soLine.Customer,
                    Price = soLine.Price,
                    LineNumber = soLine.LineNum,
                    Mfr = soLine.Mfr,
                    OrderNumber = soLine.OrderNo,
                    OrderQty = soLine.OrderQty,
                    Package = soLine.PackagingName,
                    SalesPerson = soLine.SalesPerson
                });
            }
            return exportsLines;
        }

        private string CreateBuyerNameString(List<OFBuyerResponse> buyers)
        {
            return string.Join(", ", buyers.Select(x => x.BuyerName));
        }

        private List<OFBuyerResponse> MapOFBuyerToJsonObject(string buyerJson)
        {
            var responseList = new List<OFBuyerResponse>();

            if (buyerJson == null)
            {
                return responseList;
            }
            var buyerToObjectList = JsonConvert.DeserializeObject<List<OFBuyerMap>>(buyerJson);

            foreach (var ofBuyerMap in buyerToObjectList)
            {
                responseList.Add(new OFBuyerResponse
                {
                    BuyerName = ofBuyerMap.BuyerName,
                    SOLineID = ofBuyerMap.SOLineID
                });
            }

            return responseList;
        }

        public OrderFulfillmentAvailabilityListResponse GetOrderFulfillmentAvailability(int soLineId)
        {
            var dbofAvailability = _orderFillmentRepository.GetOrderFulfillmentAvailability(soLineId);
            var list = new List<OrderFulfillmentAvailabilityResponse>();
            var totalCount = 0;
            foreach (var value in dbofAvailability)
            {
                list.Add(new OrderFulfillmentAvailabilityResponse
                {
                    TypeName = value.Type,
                    ID = value.ID,
                    PartNumber = value.PartNumber,
                    Manufacturer = value.MfrName,
                    CommodityName = value.CommodityName,
                    Supplier = value.AccountName,
                    SupplierId = value.AccountID,
                    OriginalQty = value.OrigQty,
                    AvailableQty = value.Available,
                    Cost = value.Cost,
                    DateCode = value.DateCode,
                    PackagingName = value.PackagingName,
                    SalesOrderID = value.SalesOrderID,
                    SOVersionID = value.SOVersionID,
                    SOLineID = value.SOLineID,
                    PurchaseOrderID = value.PurchaseOrderID,
                    POVersionID = value.POVersionID,
                    LineNum = (value.LineNum + "." + value.LineRev),
                    ShipDate = value.ShipDate,
                    Buyers = value.Buyers != null? CleanLists(value.Buyers): "",
                    ConditionName = value.ConditionName,
                    InTransit = value.InTransit,
                    IsInspection = value.IsInspection,
                    Comments = value.Comments,
                    ItemID = value.ItemID,
                    WarehouseName = value.WarehouseName,
                    ExternalID = value.ExternalID,
                    Allocated=  _bOMsService.MapAllocationJsonToObject(value.Allocations)

                });
            }
            if (dbofAvailability.Count() > 0)
            {
                totalCount = dbofAvailability[0].TotalRows;
            }
            return new OrderFulfillmentAvailabilityListResponse { OrderFulfillmentAvailabilityList = list, TotalRows = totalCount };

        }
        public InventoryAllocateSyncResponse Sync(int soLineId, int stockId, int qty, bool isDeleted)
        {
            InventoryAllocateSyncResponse syncResponse = new InventoryAllocateSyncResponse();

            var syncRequest = _iaSyncRequestCreator.Create(soLineId, stockId, qty, isDeleted);
            syncResponse = _middlewareService.SynchronousSync<InventoryAllocateSync, InventoryAllocateSyncResponse>(syncRequest, "transactions/inventoryallocation");
            
            return syncResponse;
        }

        private static string CleanLists(string stringToClean)
        {
            return !string.IsNullOrEmpty(stringToClean) ? stringToClean.Trim().TrimEnd(',') : null;
        }

        public SetOrderFulfillmentQtyResponse SetOrderFulfillmentQty(int soLineId, int id, string idType, int qty, bool isDeleted)
        {
            var response = new SetOrderFulfillmentQtyResponse();

            //check source of supply validation
            var currentSoSWhDetails = GetWarehouseSoSDetails(soLineId);
            if (currentSoSWhDetails != null && currentSoSWhDetails.Count() > 0)
            {
                var allocationSoSWhDetails = GetAllocationWarehouseDetails(id, idType);
                var salesOrderDb = _soRepo.GetSalesOrderFromLine(soLineId);
                var potentialSoS = GetSourceOfSupply(salesOrderDb, soLineId, allocationSoSWhDetails);
                var currentSoS = GetSourceOfSupply(salesOrderDb, soLineId, currentSoSWhDetails);

                if (currentSoS.Entity != potentialSoS.Entity)
                {
                    response.ErrorMessage = "Source Of Supply not compatable";
                    return response;
                }
            }
            try
            {
                if (idType.Equals("Inventory"))
                {

                    var syncWithSap = Sync(soLineId, id, qty, isDeleted);

                    if (!string.IsNullOrEmpty(syncWithSap.ErrorMessage))
                    {
                        response.ErrorMessage = "Allocation Failed! ";
                        response.ErrorMessage += syncWithSap.ErrorMessage;
                        return response;
                    }

                    var oldInv = new ItemInventoryDb();

                    //HANDLE UPDATING OF ITEMINVENTORY LINES
                    foreach (var line in syncWithSap.AllocatedInventoryList)
                    {
                        if (string.IsNullOrEmpty(line.NewStockExternalId))
                        {
                            response.ErrorMessage = "Sync Allocation with Sap did not return an error message, but returned null Stock External ID";
                            return response;
                        }

                        int checkStockId = _orderFillmentRepository.GetStockIDFromExternal(line.NewStockExternalId);
                        if (checkStockId < 1)
                        {
                            checkStockId = id;
                        }

                        //CREATE or UPDATE STOCK
                        var newStockDb = _orderFillmentRepository.GetItemStock(checkStockId);
                        newStockDb.ItemStockID = checkStockId == id ? 0 : checkStockId; //if checkStockId == id then it is new, so use 0 as id
                        newStockDb.ExternalID = line.NewStockExternalId;

                        //if this is a new stock, we want to set the clonedFromId to be the original stock, otherwise don't edit it
                        if (checkStockId == id)
                        {
                            newStockDb.ClonedFromID = id;
                        }

                        //set item stock
                        var newStockId = _orderFillmentRepository.SetItemStock(newStockDb);

                        //get the inventory from the old stock that we changed 
                        int binId = _orderFillmentRepository.GetWarehouseBinIdByExternalIdWarehouseExternalId(line.BinExternalId, line.WarehouseExternalId);
                        var oldInventory = _orderFillmentRepository.GetItemInventoryOnStock(id).Where(x => x.WarehouseBinID == binId).FirstOrDefault();
                        int oldInventoryId = oldInventory.InventoryID;
                        oldInv = oldInventory ?? throw new GlobalApiException("Could not find match on inventory for Warehouse Bin ID: " + binId);
                        var oldStockId = _orderFillmentRepository.GetStockIDFromExternal(line.OldStockExternalId);

                        //SET NEW ITEM INV DATA USING OLD INV AS BASE
                        oldInv.InventoryID = 0;
                        oldInv.StockID = newStockId;
                        oldInv.Qty = (int)line.NewQty;

                        var newInvId = _orderFillmentRepository.SetItemInventory(oldInv);
                        if (newInvId < 1)
                        {
                            var errorMessage = "Database error occured: Could not create new item inventory from allocation";
                            response.ErrorMessage = errorMessage;
                            return response;
                        }

                        //UPDATE OLD DATA AND SET BACK DATA WE CHANGED FROM SETTING THE NEW INV
                        oldInv.InventoryID = oldInventoryId;
                        oldInv.StockID = oldStockId;
                        oldInv.Qty = (int)line.RemainingQty;
                        if (oldInv.Qty == 0 || isDeleted)
                        {
                            oldInv.IsDeleted = true;
                        }

                        var oldInvId = _orderFillmentRepository.SetItemInventory(oldInv);
                        if (oldInvId < 1)
                        {
                            var errorMessage = "Database error occured: Could not update qty of old item inventory from allocation";
                            response.ErrorMessage = errorMessage;
                            return response;
                        }

                        int totalOldStockQty = _orderFillmentRepository.GetQtyOfInventoryOnStock(id);
                        //MAP SO AND NEW INVENTORY CREATED
                        if (!isDeleted)
                        {
                            var totalNewStockQty = _orderFillmentRepository.GetQtyOfInventoryOnStock(newStockId);
                            var dbSetOrderFulfillmentNewQty = _orderFillmentRepository.SetOrderFulfillmentQty(soLineId, newStockId, idType, totalNewStockQty, false);

                            if (totalOldStockQty == 0)
                            {
                                isDeleted = true;
                            }
                        }

                        //UPDATE QTY OF OLD INVENTORY ON MAPPING TABLE
                        var dbSetOrderFulfillmentRemainingQty = _orderFillmentRepository.UpdateExistingInventoryFulfillmentQty(id, totalOldStockQty, isDeleted);
                        response.SOLineID = soLineId;
                        response.Qty = qty;
                        response.ID = newStockId;
                    }

                }
                else if (idType.Equals("Purchase Order"))
                {

                    var searchFilter = new SearchFilter
                    {
                        RowLimit = 1,
                        RowOffset = 0,
                        PoLineId = id
                    };
                    var dbPOLine = _purchaseOrderRepository.GetPurchaseOrderLines(0, 0, searchFilter).First();
                    if (dbPOLine.Qty > qty && !isDeleted)
                    {
                        var updateOriginalPOLineReq = MapPODbToRequest(dbPOLine);
                        var originalQty = (int)updateOriginalPOLineReq.Qty;
                        updateOriginalPOLineReq.Qty = originalQty - qty;
                        _purchaseOrderRepository.SetPurchaseOrderLine(updateOriginalPOLineReq);
                        updateOriginalPOLineReq.POLineId = 0;
                        updateOriginalPOLineReq.Qty = qty;
                        updateOriginalPOLineReq.ClonedFromID = dbPOLine.ClonedFromID ?? dbPOLine.POLineId;
                        var newPo = _purchaseOrderRepository.SetPurchaseOrderLine(updateOriginalPOLineReq);

                        var dbSetOrderFulfillmentQty = _orderFillmentRepository.SetOrderFulfillmentQty(soLineId, newPo.POLineId, idType, qty, isDeleted);
                        response.SOLineID = dbSetOrderFulfillmentQty.SOLineID;
                        response.Qty = dbSetOrderFulfillmentQty.Qty;
                        response.ID = dbSetOrderFulfillmentQty.POLineID;
                    }
                    else if (dbPOLine.Qty == qty && !isDeleted)
                    {
                        //update original with is deleted
                        var updateOriginalPOLineReq = MapPODbToRequest(dbPOLine);
                        updateOriginalPOLineReq.IsDeleted = true;
                        updateOriginalPOLineReq.Qty = 0;
                        _purchaseOrderRepository.SetPurchaseOrderLine(updateOriginalPOLineReq);

                        //create new
                        updateOriginalPOLineReq.IsDeleted = false;
                        updateOriginalPOLineReq.POLineId = 0;
                        updateOriginalPOLineReq.Qty = qty;
                        updateOriginalPOLineReq.ClonedFromID = dbPOLine.ClonedFromID ?? dbPOLine.POLineId;
                        var newPo = _purchaseOrderRepository.SetPurchaseOrderLine(updateOriginalPOLineReq);

                        var dbSetOrderFulfillmentQty = _orderFillmentRepository.SetOrderFulfillmentQty(soLineId, newPo.POLineId, idType, qty, isDeleted);
                        response.SOLineID = dbSetOrderFulfillmentQty.SOLineID;
                        response.Qty = dbSetOrderFulfillmentQty.Qty;
                        response.ID = dbSetOrderFulfillmentQty.POLineID;
                    }
                    else if (isDeleted)
                    {
                        var updateOriginalPOLineReq = MapPODbToRequest(dbPOLine);
                        updateOriginalPOLineReq.IsDeleted = true;
                        _purchaseOrderRepository.SetPurchaseOrderLine(updateOriginalPOLineReq);
                        updateOriginalPOLineReq.ClonedFromID = dbPOLine.POLineId;
                        updateOriginalPOLineReq.POLineId = 0;
                        updateOriginalPOLineReq.Qty = qty;
                        var newPo = _purchaseOrderRepository.SetPurchaseOrderLine(updateOriginalPOLineReq);

                        var dbSetOrderFulfillmentQty = _orderFillmentRepository.SetOrderFulfillmentQty(soLineId, id, idType, qty, isDeleted);
                        response.SOLineID = soLineId;
                        response.Qty = qty;
                        response.ID = newPo.POLineId;
                    }
                    else
                    {
                        var dbSetOrderFulfillmentQty = _orderFillmentRepository.SetOrderFulfillmentQty(soLineId, id, idType, qty, isDeleted);
                        response.SOLineID = dbSetOrderFulfillmentQty.SOLineID;
                        response.Qty = dbSetOrderFulfillmentQty.Qty;
                        response.ID = dbSetOrderFulfillmentQty.POLineID;
                    }

                    var syncRequest = PoSync(id, soLineId);
                }
            }
            catch (Exception e)
            {
                throw new GlobalApiException(e.Message + ": " + e.StackTrace);
            }

            return response;
        }

        public PoAllocateSyncResponse PoSync(int poLineId, int soLineId)
        {
            var purchaseOrder = _purchaseOrderRepository.GetPurchaseOrderDetailsFromLineId(poLineId);
            var syncRequest = _poSyncRequestCreator.Create(purchaseOrder.PurchaseOrderID, purchaseOrder.VersionID, soLineId, poLineId);
            var response = _middlewareService.SynchronousSync<PurchaseOrderSync, PoAllocateSyncResponse>(syncRequest, "transactions/poallocation");

            if (string.IsNullOrEmpty(purchaseOrder.ExternalId))
            {
                if (!string.IsNullOrEmpty(response.ExternalId))
                {
                    _purchaseOrderRepository.SetExternalId(purchaseOrder.PurchaseOrderID, response.ExternalId);
                }
            }

            return response;
        }

        private SetPurchaseOrderLineRequest MapPODbToRequest (PurchaseOrderLinesDb dbPoLine)
        {
            var req = new SetPurchaseOrderLineRequest
            {
                Cost = dbPoLine.Cost,
                DateCode = dbPoLine.DateCode,
                DueDate = dbPoLine.DueDate.ToString(),
                Qty = dbPoLine.Qty,
                POVersionId = dbPoLine.POVersionID,
                PurchaseOrderId = dbPoLine.PurchaseOrderID,
                POLineId = dbPoLine.POLineId,
                IsSpecBuy = dbPoLine.IsSpecBuy,
                ItemId = dbPoLine.ItemId,
                PackageConditionID = dbPoLine.PackageConditionID,
                PackagingId = dbPoLine.PackagingId,
                SpecBuyForAccountID = dbPoLine.SpecBuyForAccountID,
                SpecBuyForUserID = dbPoLine.SpecBuyForUserId,
                SpecBuyReason = dbPoLine.SpecBuyReason,
                StatusId = dbPoLine.StatusId,
                VendorLine = dbPoLine.VendorLine,
                ToWarehouseID = dbPoLine.ToWarehouseID

            };
            return req;
        }

        public BaseResponse HandleInboundDelivery(InboundDeliverySapRequest request)
        {
            foreach (var item in request.InboundDeliveryItems)
            {
                //check if all inventory is restricted before we create
                bool checkInventory = true;
                foreach(var inventory in item.InventoryList)
                {
                    if (!inventory.IsRestricted)
                    {
                        checkInventory = false; //if there are any that arent restricted, we want to process this
                    }
                }

                if (checkInventory)
                {
                    continue;
                }

                ItemStockDB itemStock = new ItemStockDB();

                int poLineId = _purchaseOrderRepository.GetPoLineIdFromExternal(item.PoExternalId, item.PoLineNum);
                int itemId = _itemRepository.GetItemByExternalId(item.ItemExternalId);
                int invStatusId = _orderFillmentRepository.GetInvStatusIdFromExternal(item.StockStatusExternalId);
                int stockId = _orderFillmentRepository.GetStockIDFromExternal(item.IdentifiedStockExternalId);

                if (poLineId > 0)
                {
                    itemStock.POLineID = poLineId;
                    itemStock.ItemID = itemId;
                    itemStock.ExternalID = item.IdentifiedStockExternalId;
                    itemStock.ReceivedDate = item.RecvDate;
                    itemStock.InvStatusID = invStatusId;
                    itemStock.ItemStockID = stockId > 0 ? stockId : 0;


                    var stockSet = _orderFillmentRepository.SetItemStock(itemStock);

                    if(stockSet > 0)
                    {
                        int totalQty = 0;
                        //create inventory lines
                        foreach (var sapInv in item.InventoryList)
                        {
                            //check isRestricted. if true we want to ignore
                            if (sapInv.IsRestricted)
                            {
                                continue;
                            }

                            ItemInventoryDb inventory = new ItemInventoryDb();

                            if(sapInv.WarehouseBinID != null)
                            {
                                inventory.WarehouseBinID = _orderFillmentRepository.GetWarehouseBinIdByExternalUUID(sapInv.WarehouseBinID);
                            }
                            else
                            {
                                inventory.WarehouseBinID = _orderFillmentRepository.GetInTransitBinByWarehouseExternalUUID(sapInv.WarehouseUUID); 
                            }

                            
                            inventory.Qty = sapInv.Qty;
                            inventory.StockID = stockSet;
                            inventory.IsInspection = sapInv.IsInspection;

                            int invSet = _orderFillmentRepository.SetItemInventory(inventory);

                            totalQty += inventory.Qty;
                        }
                        
                        //map allocation if there is a product spec
                        if (item.ProductSpec != null)
                        {
                            int soLineId = _soRepo.GetSoLineIdFromProductSpec(item.ProductSpec);
                            var mapInv = _orderFillmentRepository.SetOrderFulfillmentQty(soLineId, stockSet, "Inventory", totalQty, false);

                            if (mapInv == null)
                            {
                                return new BaseResponse
                                {
                                    IsSuccess = false,
                                    ErrorMessage = string.Format("Could not map Stock ID {0} to SO Line ID {1} based on request details", stockSet, soLineId)
                                };
                            }
                        }
                    }
                    else
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            ErrorMessage = string.Format("Could not create Inventory based on request details")
                        };
                    }
                }
                else
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = string.Format("Could not find POLineID corresponding with Purchase Order ExternalId {0}, and Line Number {1}", item.PoExternalId, item.PoLineNum)
                    };
                }
            }

            return new BaseResponse { IsSuccess = true };
        }


        public BaseResponse HandleLogisticsExecution(LogisticsExecutionSapRequest request)
        {
            int oldStockId = _orderFillmentRepository.GetStockIDFromExternal(request.OldInventory.IdentifiedStockExternalId);
            int changedStockId = _orderFillmentRepository.GetStockIDFromExternal(request.ChangedInventory.IdentifiedStockExternalId);
            var oldStock = _orderFillmentRepository.GetItemStock(oldStockId);
            var changedStock = _orderFillmentRepository.GetItemStock(changedStockId);

            //handle fringe error cases
            if(oldStockId == 0)
            {
                return new BaseResponse { ErrorMessage = "Identified stock does not exist in our database: " + request.OldInventory.IdentifiedStockExternalId };
            }

            var sapOldInventory = request.OldInventory.InventoryList.First();
            var sapChangedInventory = request.ChangedInventory.InventoryList.First();

            var oldInventory = _orderFillmentRepository.GetItemInventoryFromCompoundSapKey(request.OldInventory.IdentifiedStockExternalId,
                sapOldInventory.WarehouseBinID, sapOldInventory.IsInspection, sapOldInventory.IsRestricted);
            var changedInventory = _orderFillmentRepository.GetItemInventoryFromCompoundSapKey(request.ChangedInventory.IdentifiedStockExternalId,
                sapChangedInventory.WarehouseBinID, sapChangedInventory.IsInspection, sapChangedInventory.IsRestricted);

            if(oldInventory == null || oldInventory.InventoryID == 0)
            {
                return new BaseResponse { ErrorMessage = "No matching original inventory exists in our database for Item Stock: " + request.OldInventory.IdentifiedStockExternalId };
            }

            oldInventory.Qty -= sapOldInventory.Qty;
            _orderFillmentRepository.SetItemInventory(oldInventory);

            if(changedStock == null || changedStock.ItemStockID == 0)
            {
                //create stock based on details from original
                oldStock.ItemStockID = 0;
                oldStock.ExternalID = request.ChangedInventory.IdentifiedStockExternalId;
                oldStock.ReceivedDate = request.ChangedInventory.RecvDate;
                changedStockId = _orderFillmentRepository.SetItemStock(oldStock);
            }

            if (changedInventory == null || changedInventory.InventoryID == 0)
            {
                var newInventory = new ItemInventoryDb();
                newInventory.Qty = sapChangedInventory.Qty;
                newInventory.IsInspection = sapChangedInventory.IsInspection;
                newInventory.WarehouseBinID = _orderFillmentRepository.GetWarehouseBinIdByExternalID(sapChangedInventory.WarehouseBinID);
                newInventory.IsDeleted = sapChangedInventory.IsRestricted;
                newInventory.StockID = changedStockId;
                _orderFillmentRepository.SetItemInventory(newInventory);
            }
            else
            {
                changedInventory.Qty += sapChangedInventory.Qty;
                _orderFillmentRepository.SetItemInventory(changedInventory);
            }

            return new BaseResponse { IsSuccess = true };
        }

        public BaseResponse HandleProductionLot(ProductionLotSapRequest request)
        {
            var oldItemStockId = _orderFillmentRepository.GetStockIDFromExternal(request.OldIdentifiedStockExternalId);
            var updatedItemStockId = _orderFillmentRepository.GetStockIDFromExternal(request.NewIdentifiedStockExternalId);

            if(oldItemStockId == 0)
            {
                return new BaseResponse { ErrorMessage = "Identified stock does not exist in our database: " + request.OldIdentifiedStockExternalId };
            }

            //update old stock
            var oldStockInventoryList = _orderFillmentRepository.GetItemInventoryOnStock(oldItemStockId);
            var oldInventoryToUpdate = oldStockInventoryList.Where(x => x.WarehouseBinExternalId == request.WarehouseBinExternalId).FirstOrDefault();

            if (oldInventoryToUpdate == null)
            {
                return new BaseResponse { ErrorMessage = "Old Identified stock does not have existing inventory in WarehouseBin: " + request.WarehouseBinExternalId };
            }

            oldInventoryToUpdate.Qty -= request.Qty;
            _orderFillmentRepository.SetItemInventory(oldInventoryToUpdate);

            //update new / updated stock
            if(updatedItemStockId == 0)
            {
                //create new stock
                var newItemStock = new ItemStockDB();
                var oldItemStock = _orderFillmentRepository.GetItemStock(oldItemStockId);
                newItemStock.POLineID = oldItemStock.POLineID;
                newItemStock.ItemID = _itemRepository.GetItemByExternalId(request.ItemExternalId);
                newItemStock.ExternalID = request.NewIdentifiedStockExternalId;
                newItemStock.ReceivedDate = request.RecvDate;
                newItemStock.DateCode = request.DateCode;
                newItemStock.COO = _commonDataRepository.GetCountryByExternal(request.COO);
                //newItemStock.ROHS = request.ROHS;
                updatedItemStockId = _orderFillmentRepository.SetItemStock(newItemStock);

                //create new inventory on stock
                var newItemInventory = new ItemInventoryDb();
                newItemInventory.StockID = updatedItemStockId;
                newItemInventory.Qty = request.Qty;
                newItemInventory.WarehouseBinID = _orderFillmentRepository.GetWarehouseBinIdByExternalID(request.WarehouseBinExternalId);
                _orderFillmentRepository.SetItemInventory(newItemInventory);
            }
            else
            {
                var updatedStockInventoryList = _orderFillmentRepository.GetItemInventoryOnStock(updatedItemStockId);
                var updatedInventoryToUpdate = updatedStockInventoryList.Where(x => x.WarehouseBinExternalId == request.WarehouseBinExternalId).FirstOrDefault();

                if(updatedInventoryToUpdate == null)
                {
                    //create new inventory                
                    var newItemInventory = new ItemInventoryDb();
                    newItemInventory.StockID = updatedItemStockId;
                    newItemInventory.Qty = request.Qty;
                    newItemInventory.WarehouseBinID = _orderFillmentRepository.GetWarehouseBinIdByExternalID(request.WarehouseBinExternalId);
                    _orderFillmentRepository.SetItemInventory(newItemInventory);
                }
                else
                {
                    //update existing inventory
                    updatedInventoryToUpdate.Qty += request.Qty;
                    _orderFillmentRepository.SetItemInventory(updatedInventoryToUpdate);
                }
            }

            //check product spec and update both
            if(request.ProductSpec != null)
            {
                //reduce qty of old stock's allocation
                int oldStockLineId = _orderFillmentRepository.GetSoLineIdOnStock(oldItemStockId);
                int allocatedOnStock = _orderFillmentRepository.GetStockQtyAllocated(oldItemStockId);
                _orderFillmentRepository.SetOrderFulfillmentQty(oldStockLineId, oldItemStockId, "Inventory", allocatedOnStock - request.Qty, false);

                //allocate new / updated stock's qty
                var soLines = _soRepo.GetSoLinesFromProductSpec(request.ProductSpec);
                soLines.Sort((x, y) => x.DueDate.CompareTo(y.DueDate));
                int qtyCounter = request.Qty;

                foreach (var line in soLines)
                {
                    if(qtyCounter == 0)
                    {
                        break;
                    }

                    int allocatedQty = _orderFillmentRepository.GetQtyAllocatedForLine(line.SOLineId);
                    int remainingQty = line.Qty - allocatedQty;

                    if(remainingQty >= qtyCounter)
                    {
                        _orderFillmentRepository.SetOrderFulfillmentQty(line.SOLineId, updatedItemStockId, "Inventory", qtyCounter, false);
                        qtyCounter = 0;
                    }
                    else
                    {
                        _orderFillmentRepository.SetOrderFulfillmentQty(line.SOLineId, updatedItemStockId, "Inventory", remainingQty, false);
                        qtyCounter -= remainingQty;
                    }

                }
            }

            return new BaseResponse { IsSuccess = true };
        }

        public List<SoSWarehouseDetailsDB> GetWarehouseSoSDetails(int soLineId)
        {
            return _orderFillmentRepository.GetWarehouseSoSDetails(soLineId);
        }

        private List<SoSWarehouseDetailsDB> GetAllocationWarehouseDetails(int allocationId, string idType)
        {
            if(idType.Equals("Purchase Order"))
            {
                return _orderFillmentRepository.GetPoWarehouseSoSDetails(allocationId);
            }
            else if (idType.Equals("Inventory"))
            {
                return _orderFillmentRepository.GetStockWarehouseSoSDetails(allocationId);
            }

            return null;
        }

        public SourceOfSupply GetSourceOfSupply(SalesOrderDetailsDb salesOrderDb, int soLineId, List<SoSWarehouseDetailsDB> sosWarehouseDetails)
        {
            int seller = _soRepo.GetSalesOrderOrganization(salesOrderDb.SalesOrderId, salesOrderDb.VersionId).OrganizationID;

            var warehouseSoSDetails = sosWarehouseDetails;//_orderFillmentRepository.GetWarehouseSoSDetails(soLineId);

            if (warehouseSoSDetails == null || (warehouseSoSDetails != null && warehouseSoSDetails.Count() == 0))
            {
                return null;
            }

            int owner = warehouseSoSDetails != null ? warehouseSoSDetails.First().OrganizationID : 0;
            int? soShipFromRequirement = salesOrderDb.ShipFromRegionID > 0 ? (int?)salesOrderDb.ShipFromRegionID : null;
            int? whLocation = warehouseSoSDetails != null ? (int?)warehouseSoSDetails.First().ShipFromRegionID : null;
            bool isExternal = false;
            string entity = null;

            if (seller != owner && (soShipFromRequirement == null || soShipFromRequirement == whLocation))
            {
                isExternal = true;
            }

            if (!isExternal)
            {
                var warehouseList = _commonDataRepository.GetWarehouses(seller).ToList();

                if(soShipFromRequirement != null)
                {
                    entity = warehouseList.Where(x => soShipFromRequirement == x.ShipFromRegionID).First().ExternalID;
                }
                else
                {
                    entity = warehouseList.Where(x => whLocation == x.ShipFromRegionID).First().ExternalID;
                }
            }
            else
            {
                entity = _commonDataRepository.GetAllOrganizations().Where(x => x.OrganizationID == owner).First().ExternalID;
            }

            return new SourceOfSupply { Entity = entity, IsExternal = isExternal};
        }       

    }
}
