using Sourceportal.DB.QC;
using Sourceportal.Domain.Models.API.Responses.QC;
using System.Collections.Generic;
using System.Linq;
using Sourceportal.DB.Documents;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Items;
using Sourceportal.Domain.Models.API.Requests.QC;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Sync;
using SourcePortal.Services.Shared.Middleware;
using Sourceportal.Domain.Models.DB.QC;
using Sourceportal.DB.OrderFillment;
using Sourceportal.Domain.Models.API.Requests.ItemStock;
using Sourceportal.Domain.Models.DB.ItemStock;
using Sourceportal.Domain.Models.DB.OrderFulfillment;
using Sourceportal.Domain.Models.API.Responses.Sync.SynchronousResponses;
using Sourceportal.Domain.Models.Middleware.OrderFulfillment;
using SourcePortal.Services.OrderFulfillment.InventoryAllocation;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using SourcePortal.Services.BOMs;
using Newtonsoft.Json;
using System;

namespace SourcePortal.Services.QC
{
    public class InspectionService : IInspectionService
    {
        private readonly IInspectionRepository _iInspectionRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IQcInspectionSyncRequestCreator _qcInspectionSyncRequestCreator;
        private readonly IMiddlewareService _middlewareService;
        private readonly IDocumentsRepository _documentsRepository;
        private readonly IOrderFillmentRepository _orderFulfillmentRepository;
        private readonly IInventoryAllocationSyncRequestCreator _iaSyncRequestCreator;
       

        public InspectionService(IInspectionRepository iInspectionRepository, IChecklistRepository checklistRepository, IItemRepository itemRepository,
            IQcInspectionSyncRequestCreator qcInspectionSyncRequestCreator, IMiddlewareService middlewareService, IDocumentsRepository documentsRepository,
            IOrderFillmentRepository orderFulfillmentRepository, IInventoryAllocationSyncRequestCreator iaSyncRequestCreator)
        {
            _iInspectionRepository = iInspectionRepository;
            _checklistRepository = checklistRepository;
            _itemRepository = itemRepository;
            _qcInspectionSyncRequestCreator = qcInspectionSyncRequestCreator;
            _middlewareService = middlewareService;
            _documentsRepository = documentsRepository;
            _orderFulfillmentRepository = orderFulfillmentRepository;
            _iaSyncRequestCreator = iaSyncRequestCreator;
           
        }


        public InspectionDetailsResponse GetInspectionDetails(int inspectionID)
        {
            var inspectionDb = _iInspectionRepository.GetInspectionDetails(inspectionID);
            var po = new InspectionDetailsResponse
            {
                InspectionID = inspectionDb.InspectionID,
                InventoryID = inspectionDb.InventoryID,
                ItemID = inspectionDb.ItemID,
                QtyFailed = inspectionDb.QtyFailed,
                CompletedBy = inspectionDb.CompletedBy,
                CompletedDate = inspectionDb.CompletedDate,
                CompletedByUser = inspectionDb.CompletedByUser,
                CreatedBy = inspectionDb.CreatedBy,
                CreatedDate = inspectionDb.Created,
                POLineID = inspectionDb.POLineID,
                Qty = inspectionDb.Qty,
                DateCode = inspectionDb.DateCode,
                PackagingID = inspectionDb.PackagingID,
                CommodityID = inspectionDb.CommodityID,
                ItemStatusID = inspectionDb.ItemStatusID,
                PartNumber = inspectionDb.PartNumber,
                PartNumberStrip = inspectionDb.PartNumberStrip,
                mfrName = inspectionDb.MfrName,
                PartDescription = inspectionDb.PartDescription,
                WarehouseName = inspectionDb.WarehouseName,
                CustomerAccount = inspectionDb.CustomerAccount,
                VendorAccount = inspectionDb.VendorAccount,
                LotNumber = inspectionDb.LotNumber,
                QCNotes = inspectionDb.QCNotes,
                CustomerAccountID = inspectionDb.CustomerAccountID,
                VendorAccountID = inspectionDb.VendorAccountID,
                ItemQty = inspectionDb.ItemQty,
                UserID = inspectionDb.UserID,
                ExternalID = inspectionDb.ExternalID,
                SOExternalID = inspectionDb.SOExternalID,
                POExternalID = inspectionDb.POExternalID,
                InspectionStatusId = inspectionDb.InspectionStatusID,
                SalesOrderID = inspectionDb.SalesOrderID,
                SOVersionID = inspectionDb.SOVersionID,
                PurchaseOrderID = inspectionDb.PurchaseOrderID,
                POVersionID = inspectionDb.POVersionID,
                ResultID = inspectionDb.ResultID,
                InspectionTypeName = inspectionDb.InspectionTypeName,
                VendorType = inspectionDb.VendorType
            };

            return po;
        }

        public int DeleteItemStockOnInspection(int stockId)
        {
            //Delete stock
            ItemStockDB itemStockDb = new ItemStockDB()
            {
                ItemStockID = stockId,
                IsDeleted = true
            };
            var itemStockId = _orderFulfillmentRepository.SetItemStock(itemStockDb);

            if (itemStockId > 0)
            {

                //Delete Inventory
                List<ItemInventoryDb> inventoryList = _orderFulfillmentRepository.GetItemInventoryOnStock(itemStockId);
                foreach (ItemInventoryDb inv in inventoryList)
                {
                    inv.IsDeleted = true;
                    var inventoryId = _orderFulfillmentRepository.SetItemInventory(inv);
                }

                //Delete Breakdown lines
                bool linesDeleted = _iInspectionRepository.DeleteItemStockBreakdownLines(itemStockId);
            }
            return itemStockId;
        }
        public int SetItemStockOnInspection(SetItemStockRequest itemStock)
        {
            bool isEditing = (itemStock.StockID > 0);
            ItemStockDB itemStockDb = CreateItemStockDb(itemStock);
            var itemStockId = _orderFulfillmentRepository.SetItemStock(itemStockDb);
            //new stock ID, create inventory
            if(itemStockId > 0 && !isEditing)
            {
                ItemInventoryDb itemInventoryDb = new ItemInventoryDb();
                itemInventoryDb.StockID = itemStockId;
                itemInventoryDb.WarehouseBinID = itemStock.WarehouseBinID;
                itemInventoryDb.Qty = 0;
                var inventoryId = _orderFulfillmentRepository.SetItemInventory(itemInventoryDb);
            }
            //Update warehouse ID for each stock (should only be one)
            else if (isEditing)
            {
                List<ItemInventoryDb> inventoryList = _orderFulfillmentRepository.GetItemInventoryOnStock(itemStockId);
                foreach(ItemInventoryDb inv in inventoryList )
                {
                    inv.WarehouseBinID = itemStock.WarehouseBinID;
                    var inventoryId = _orderFulfillmentRepository.SetItemInventory(inv);
                }
            }
            return itemStockId;
        }

        public int SetItemStockBreakdownOnInspection(SetItemStockBreakdownRequest breakdown)
        {
            return _iInspectionRepository.SetItemStockBreakdown(breakdown);
        }

        private ItemStockDB CreateItemStockDb(SetItemStockRequest itemStock)
        {
            ItemStockDB itemStockDb = new ItemStockDB();
            itemStockDb.InspectionID = itemStock.InspectionID;
            itemStockDb.ItemStockID = itemStock.StockID;
            itemStockDb.IsRejected = itemStock.IsRejected;
            itemStockDb.InvStatusID = itemStock.InvStatusID;
            itemStockDb.StockDescription = itemStock.StockDescription;
            itemStockDb.ExternalID = itemStock.ExternalID;
            itemStockDb.PackagingID = itemStock.PackagingTypeID;
            itemStockDb.WarehouseBinID = itemStock.WarehouseBinID;
            itemStockDb.ReceivedDate = itemStock.ReceivedDate;
            itemStockDb.PackageConditionID = itemStock.PackageConditionID;
            itemStockDb.ItemID = itemStock.ItemID;
            itemStockDb.MfrLotNum = itemStock.MfrLotNum;
            itemStockDb.POLineID = itemStock.POLineID;
            itemStockDb.Expiry = itemStock.Expiry;
            itemStockDb.DateCode = itemStock.DateCode;
            itemStockDb.COO = itemStock.COO;
            return itemStockDb;
        }

        public ItemStockWithBreakdownsListResponse GetStockListForInspection(int inspectionId)
        {
            var stockListResponse = new ItemStockWithBreakdownsListResponse();
            stockListResponse.ItemStockList = new List<ItemStockResponse>();
            var stockIdsOnInspection = _orderFulfillmentRepository.GetInspectionItemStockList(inspectionId);

            foreach (var stockId in stockIdsOnInspection)
            {
                //var itemStockDb = _orderFulfillmentRepository.GetItemStock(inspectionId);
                if (stockId != null)
                {
                    var itemStockDetails = new ItemStockResponse();
                    itemStockDetails.ItemStockID = stockId.ItemStockID;
                    itemStockDetails.POLineID = stockId.POLineID;
                    itemStockDetails.ItemID = stockId.ItemID;
                    itemStockDetails.Qty = stockId.Qty;
                    itemStockDetails.IsRejected = stockId.IsRejected;
                    itemStockDetails.InvStatusID = stockId.InvStatusID;
                    itemStockDetails.ReceivedDate = stockId.ReceivedDate;
                    itemStockDetails.DateCode = stockId.DateCode;
                    itemStockDetails.MfrLotNum = stockId.MfrLotNum;
                    itemStockDetails.PackagingID = stockId.PackagingID;
                    itemStockDetails.PackageConditionID = stockId.PackageConditionID;
                    itemStockDetails.StockDescription = stockId.StockDescription;
                    itemStockDetails.InspectionWarehouseID = stockId.InspectionWarehouseID;
                    itemStockDetails.ExternalID = stockId.ExternalID;
                    itemStockDetails.IsDeleted = stockId.IsDeleted;
                    itemStockDetails.WarehouseBinID = stockId.WarehouseBinID;
                    itemStockDetails.WarehouseID = stockId.WarehouseID;
                    itemStockDetails.COO = stockId.COO;
                    itemStockDetails.Expiry = stockId.Expiry;
                    itemStockDetails.AcceptedBinID = stockId.AcceptedBinID;
                    itemStockDetails.AcceptedBinName = stockId.AcceptedBinName;
                    itemStockDetails.RejectedBinID = stockId.RejectedBinID;
                    itemStockDetails.RejectedBinName = stockId.RejectedBinName;

                    itemStockDetails.ItemStockBreakdownList = new List<ItemStockBreakdownResponse>();
                    var itemBreakdownListDb = _iInspectionRepository.GetItemStockBreakdownList(stockId.ItemStockID);
                    foreach (var breakdown in itemBreakdownListDb)
                    {
                        var itemBreakdownDetails = new ItemStockBreakdownResponse();
                        itemBreakdownDetails.BreakdownID = breakdown.BreakdownID;
                        itemBreakdownDetails.StockID = breakdown.StockID;
                        itemBreakdownDetails.IsDiscrepant = breakdown.IsDiscrepant;
                        itemBreakdownDetails.PackQty = breakdown.PackQty;
                        itemBreakdownDetails.NumPacks = breakdown.NumPacks;
                        itemBreakdownDetails.DateCode = breakdown.DateCode;
                        itemBreakdownDetails.PackagingID = breakdown.PackagingID;
                        itemBreakdownDetails.PackageConditionID = breakdown.PackageConditionID;
                        itemBreakdownDetails.COO = breakdown.COO;
                        itemBreakdownDetails.Expiry = breakdown.Expiry;
                        itemBreakdownDetails.MfrLotNum = breakdown.MfrLotNum;
                        itemBreakdownDetails.IsDeleted = breakdown.IsDeleted;

                        itemStockDetails.ItemStockBreakdownList.Add(itemBreakdownDetails);
                    }

                    stockListResponse.ItemStockList.Add(itemStockDetails);
                }
            }

            return stockListResponse;
        }

        public int? SetInspectionConclusion(InpsectionConclusionRequest request)
        {
            if(request.StockDetailsList != null && request.StockDetailsList.Count > 0)
            {
                foreach(var stock in request.StockDetailsList)
                {
                    if(stock.id > 0 && stock.warehouseBinId > 0)
                    {
                        var invOnStock = _orderFulfillmentRepository.GetItemInventoryOnStock(stock.id).First();
                        invOnStock.WarehouseBinID = stock.warehouseBinId;
                        _orderFulfillmentRepository.SetItemInventory(invOnStock);
                    }
                }
            }
            return _iInspectionRepository.SetInspectionConclusion(request);
        }

        public SyncResponse SyncInspectionConclusion(int inspectionId)
        {
            _orderFulfillmentRepository.UpdateItemStock(inspectionId);
            SyncStocksOnInspectionWithSap(inspectionId);
            //Update QCInspections only if the first sync(stock) is successful
            _iInspectionRepository.UpdateInspectionCompletedFields(inspectionId);
            return Sync(inspectionId);
        }

        private void UpdateStocksFromSapOnInspection(CreateStockResponse sapStockResponse, int originalStockId)
        {
            try
            {
                //UPDATE NEW STOCKS
                int qtyOnNewStocks = 0;
                if (sapStockResponse.NewItemStocks != null)
                {
                    foreach (var newStock in sapStockResponse.NewItemStocks)
                    {
                        qtyOnNewStocks += newStock.Qty;
                        var stockDb = _orderFulfillmentRepository.GetItemStock(newStock.LocalId);
                        stockDb.ExternalID = newStock.ItemStockExternalId;

                        var inventoryOfStock = _orderFulfillmentRepository.GetItemInventoryOnStock(newStock.LocalId);
                        //should only be one here...
                        inventoryOfStock.First().Qty = newStock.Qty;

                        _orderFulfillmentRepository.SetItemInventory(inventoryOfStock.First());

                        _orderFulfillmentRepository.SetItemStock(stockDb);
                    }
                }

                //UPDATE ORIGINAL STOCK
                var inventoryOfOriginal = _orderFulfillmentRepository.GetItemInventoryOnStock(originalStockId);
                var qtyOnOriginal = _orderFulfillmentRepository.GetQtyOfInventoryOnStock(originalStockId);
                foreach (var inv in inventoryOfOriginal)
                {
                    //when user was updating original stock in the inspection, the first stock should have been the one that gets updated with details
                    if (inv.InventoryID == inventoryOfOriginal.First().InventoryID)
                    {
                        inv.Qty = qtyOnOriginal - qtyOnNewStocks;
                    }
                    else
                    {
                        inv.Qty = 0;
                        inv.IsDeleted = true;
                    }

                    _orderFulfillmentRepository.SetItemInventory(inv);
                }
            }
            catch (Exception e)
            {
                var errorMessage = string.Format("Qc Complete - Update DB after Stock Sync failed: {0}. Stack Trace {1}", e.Message, e.StackTrace);
                throw new GlobalApiException(errorMessage);
            }
        }

        private void SyncStocksOnInspectionWithSap(int inspectionId)
        {
            var stockIds = _iInspectionRepository.GetStockIdsOnInpsections(inspectionId);
            stockIds.Sort();
            var syncResponse = SyncStockOnInpsection(inspectionId, stockIds.First(), stockIds);

            if(string.IsNullOrEmpty(syncResponse.OriginalItemStockExternalId))
            {
                var errorMessage = string.Format("Qc Completion Stock Sync Failed: {0}", syncResponse.ErrorMessage);
                throw new GlobalApiException(errorMessage);
            }

            UpdateStocksFromSapOnInspection(syncResponse, stockIds.First());
        }

        public CreateStockResponse SyncStockOnInpsection(int inspectionId, int originalStockId, List<int> stocksOnInspection)
        {
            CreateStockResponse syncResponse = new CreateStockResponse();

            var syncRequest = _iaSyncRequestCreator.CreateForInspection(inspectionId, originalStockId, stocksOnInspection);
            syncResponse = _middlewareService.SynchronousSync<CreateStockRequestForSap, CreateStockResponse>(syncRequest, "transactions/qc-complete/stocks/set");

            if (!string.IsNullOrEmpty(syncResponse.ErrorMessage))
            {
                var errorMessage = string.Format("Qc Completion Stock Sync Failed: {0}", syncResponse.ErrorMessage);
                throw new GlobalApiException(errorMessage);
            }

            return syncResponse;
        }

        private void NormalizeStockInventoryOnInspection(int inspectionId)
        {
            var stockIds = _iInspectionRepository.GetStockIdsOnInpsections(inspectionId);

            foreach(var id in stockIds)
            {
                var breakdownList = _iInspectionRepository.GetItemStockBreakdownList(id);
                var breakdownQtyList = breakdownList != null ? breakdownList.Select(x => x.PackQty * x.NumPacks).ToList() : null;
                var breakdownTotalQty = breakdownQtyList != null ? breakdownQtyList.Aggregate((a, b) => b + a) : 0;

                var stockTotalQty = _orderFulfillmentRepository.GetQtyOfInventoryOnStock(id);

                if(stockTotalQty != breakdownTotalQty) //indication that new stocks have been created from the original stock
                {
                    var inventoryList = _orderFulfillmentRepository.GetItemInventoryOnStock(id);

                    if(stockTotalQty < breakdownTotalQty)// for secondary/newly-created down stocks, might as well check if stockTotalQty is 0?
                    {
                        var inventory = inventoryList.First();
                        inventory.Qty += breakdownTotalQty - stockTotalQty; //at this point isn't stockTotalQty always 0 thus no need of deduction. 
                        _orderFulfillmentRepository.SetItemInventory(inventory);
                    }
                    else if(stockTotalQty > breakdownTotalQty) //for the first/original stock
                    {
                        int qtyDelta = stockTotalQty - breakdownTotalQty;

                        foreach(var inventory in inventoryList)
                        {
                            if(qtyDelta == 0)// don't need this, if exit from the else condtion
                            {
                                break;
                            }

                            if(qtyDelta > inventory.Qty)//Inventory NOT enough to give away the qty
                            {
                                inventory.Qty = 0;
                                qtyDelta = qtyDelta - inventory.Qty;
                                _orderFulfillmentRepository.SetItemInventory(inventory);
                            }
                            else //The Inventory can give away the total that were taken away
                            {
                                inventory.Qty = inventory.Qty - qtyDelta;
                                qtyDelta = 0; //instead of setting this here, u could get out of the loop(after saving inventory)?
                                _orderFulfillmentRepository.SetItemInventory(inventory);
                            }
                        }

                    }
                }

            }

        }

        public InspectionConclusionResponse GetInspectionConclusion(int inspectionId)
        {
            InspectionConclusionResponse conclusionResponse;
            var inspectionDb = _iInspectionRepository.GetInspectionConclusion(inspectionId);

            if (inspectionDb == null)
            {
                conclusionResponse = new InspectionConclusionResponse();
            }
            else
            {
                conclusionResponse = new InspectionConclusionResponse
                {
                    InventoryID = inspectionDb.InventoryID,
                    LotTotal = inspectionDb.LotTotal,
                    QtyPassed = inspectionDb.QtyPassed,
                    QtyFailedTotal = inspectionDb.QtyFailedTotal,
                    Introduction = inspectionDb.Introduction,
                    TestResults = inspectionDb.TestResults,
                    Conclusion = inspectionDb.Conclusion,
                    InspectionQty = inspectionDb.InspectionQty
                };
            }

            return conclusionResponse;
        }

        public InspectionCheckListsResponse GetInspectionCheckLists(int inspectionId)
        {
            var response = new InspectionCheckListsResponse();
            response.CheckLists = new List<InspectionCheckList>();
            var checkListsdb = _iInspectionRepository.GetCheckListsForInspectionWithQuestions(inspectionId);

            foreach (var checklistDb in checkListsdb)
            {
                var checkList = new InspectionCheckList
                {
                    Name = checklistDb.ChecklistName,
                    Id = checklistDb.ChecklistId,
                    AddedByUser = checklistDb.AddedByUser,
                    Questions = new List<InspectionQuestion>()
                };

                foreach (var question in checklistDb.Questions)
                {
                    checkList.Questions.Add(new InspectionQuestion
                    {
                        Id = question.QuestionId,
                        AnswerId = question.AnswerId,
                        Answer = question.Answer,
                        AnswerTypeId = question.AnswerTypeId,
                        Comments = question.Note,
                        Inspected = question.CompletedDate != null,
                        Number = question.SortOrder,
                        QtyFailed = question.QtyFailed,
                        ShowQtyFailed = question.ShowQtyFailed,
                        SubText = question.QuestionSubText,
                        Text = question.QuestionText,
                        ImageCount = question.ImageCount,
                        CanComment = question.CanComment,
                        CompletedDate = question.CompletedDate,
                        RequiresPicture = question.RequiresPicture
                    });
                }
                response.CheckLists.Add(checkList);
            }

            return response;
        }

        public InspectionGridResponse GetInspections(string searchString,int rowOffset, int rowLimit, string sortCol, bool descSort)
        {
            var response = new InspectionGridResponse();
            response.InspectionList = new List<InspectionGridItem>();
            var inspetionListDb = _iInspectionRepository.GetInspectionList(searchString,rowOffset, rowLimit, sortCol, descSort);

            foreach (var inspectionDb in inspetionListDb)
            {
                var inspection = new InspectionGridItem
                {
                    InspectionId = inspectionDb.InspectionId,
                    Customers = MapCustomersJsonToObject(inspectionDb.Customers),
                    SalesOrders= MapSalesOrderJsonToObject(inspectionDb.Customers),
                    ItemId = inspectionDb.ItemId,
                    PoNumber = inspectionDb.PoNumber,
                    ReceivedDate = inspectionDb.ReceivedDate != null ? inspectionDb.ReceivedDate.Value.ToString("MM/dd/yyyy") : null,
                    ShipDate = inspectionDb.ShipDate != null ? inspectionDb.ShipDate.Value.ToString("MM/dd/yyyy") : null,
                    InventoryID = inspectionDb.InventoryID,
                    StatusName = inspectionDb.StatusName,
                    InspectionTypeID = inspectionDb.InspectionTypeID,
                    InspectionTypeName = inspectionDb.InspectionTypeName,
                    Supplier = inspectionDb.Supplier,
                    LotNumber = inspectionDb.LotNumber,
                    WarehouseID = inspectionDb.WarehouseID,
                    WarehouseName = inspectionDb.WarehouseName,
                    StockExternalID = inspectionDb.StockExternalID,
                    POVersionID = inspectionDb.POVersionID,
                    AccountID = inspectionDb.AccountID,
                    POExternalID = inspectionDb.POExternalID
                };
                response.InspectionList.Add(inspection);
            }
            response.RowCount = inspetionListDb.Count > 0 ? inspetionListDb.First().RowCount : 0;
            return response;
        }

        private List<InspectionCustomersJsonResponse> MapCustomersJsonToObject(string customersJson)
        {
            var responseList = new List<InspectionCustomersJsonResponse>();
            if (customersJson == null)
            {
                return responseList;
            }

            var customersToObjectList = JsonConvert.DeserializeObject<List<InspectionCustomersJsonResponse>>(customersJson);
            return  customersToObjectList.Select(x => new InspectionCustomersJsonResponse { AccountName = x.AccountName }).ToList();
        }

        private List<InspectionSalesOrdersResponse> MapSalesOrderJsonToObject(string customersJson)
        {
            var responseList = new List<InspectionSalesOrdersResponse>();
            if (customersJson == null)
            {
                return responseList;
            }

            var customersToObjectList = JsonConvert.DeserializeObject<List<InspectionSalesOrdersResponse>>(customersJson);
            foreach (var customerMap in customersToObjectList)
            {
                responseList.Add(new InspectionSalesOrdersResponse
                {
                   SalesOrderID=customerMap.SalesOrderID,
                   ExternalID= customerMap.ExternalID
                });

            }
            return responseList;
        }

        public BaseResponse SetInspection(SetInspectionFromSapRequest request)
        {
            var itemStock = _orderFulfillmentRepository.GetStockIDFromExternal(request.StockExternalId);
            if (itemStock < 1)
            {
                return new BaseResponse { ErrorMessage = "No matching itemStock" };
            }

            var inspectionExists = _iInspectionRepository.InspectionExistsByExternalId(request.ExternalId);
            if (!inspectionExists)
            {
                var response = _iInspectionRepository.CreateInspection(request, itemStock);
                return new BaseResponse { ErrorMessage = response.ErrorMessage };
            }
            else
            {
                var response = _iInspectionRepository.UpdateInspection(request);
                if (!string.IsNullOrEmpty(request.DocumentExternalId))
                {
                    var documents = _documentsRepository.GetDocuments(ObjectType.Inspection, request.ObjectId, null, null, (int)DocumentType.Text, null, false, true);
                    if (documents != null && documents.Count > 0)
                    {
                        var doc = documents.First();
                        _documentsRepository.SaveDocumentExternalId(doc.DocumentId, request.DocumentExternalId);
                    }
                }
                return new BaseResponse { ErrorMessage = response.ErrorMessage };
            }

        }

        public BaseResponse CompleteInspectionFromSap(SetInspectionFromSapRequest request)
        {
            var itemStock = _orderFulfillmentRepository.GetStockIDFromExternal(request.StockExternalId);
            if (itemStock < 1)
            {
                return new BaseResponse { ErrorMessage = "No matching itemStock" };
            }

            var itemInventory = _orderFulfillmentRepository.GetItemInventoryOnStock(itemStock);
            if(itemInventory.Count == 0)
            {
                return new BaseResponse { ErrorMessage = "No inventory on stock for this inspection." };
            }

            foreach(var inv in itemInventory)
            {
                inv.IsInspection = false;
                _orderFulfillmentRepository.SetItemInventory(inv);
            }

            return new BaseResponse();
        }

        public SyncResponse Sync(int inspectionId)
        {
            var syncRequest = _qcInspectionSyncRequestCreator.Create(inspectionId);
            return _middlewareService.Sync(syncRequest);
        }

        public BaseResponse SaveAnswer(SaveAnswerRequest saveAnswerRequest)
        {
            _iInspectionRepository.SaveAnswer(saveAnswerRequest);
            return new BaseResponse();
        }

        public InspectionCheckListsResponse GetAvailableCheckLists(int inspectionId)
        {
            var response = new InspectionCheckListsResponse();
            response.CheckLists = new List<InspectionCheckList>();

            var checkListsOfInspection = _iInspectionRepository.GetCheckListsWithQuestionsAndAnswers(inspectionId);
            var allChecklists = _iInspectionRepository.GetCheckLists();

            var inspectionChecklistIds = checkListsOfInspection.Select(x => x.ChecklistId).ToList();
            var difference = allChecklists.Where(x => !inspectionChecklistIds.Contains(x.ChecklistId));

            foreach (var cklistDb in difference)
            {
                var checkList = new InspectionCheckList
                {
                    Name = cklistDb.ChecklistName,
                    Id = cklistDb.ChecklistId
                };
                response.CheckLists.Add(checkList);
            }
            return response;
        }

        public QCanswersResponse SaveChecklistForInpection(InsertChecklistForInspectionRequest questionAnswersInsertRequest)
        {
            var questions = _iInspectionRepository.GetQuestionForCheckList(questionAnswersInsertRequest.CheckListID);
            var response = new QCanswersResponse();

            foreach (var questionDb in questions)
            {
                var questionList = new InsertQuestionsToAnswersRequest
                {
                    QuestionID = questionDb.QuestionID,
                    QuestionVersionID = questionDb.VersionID,
                    ChecklistID = questionDb.ChecklistID,
                    InspectionID = questionAnswersInsertRequest.InspectionID

                };
                var answers = _iInspectionRepository.SaveChecklistForInpection(questionList);
                if (answers.RowCount > 0)
                {
                    response.IsSuccess = true;
                    response.AddedByUser = answers.AddedByUser;
                }
            }

            return response;
        }

        public BaseResponse DeleteChecklistForInpection(InsertChecklistForInspectionRequest questionAnswersInsertRequest)
        {
            var questions = _iInspectionRepository.GetQuestionForCheckList(questionAnswersInsertRequest.CheckListID);
            var response = new BaseResponse();

            foreach (var questionDb in questions)
            {
                var questionList = new InsertQuestionsToAnswersRequest
                {
                    QuestionID = questionDb.QuestionID,
                    QuestionVersionID = questionDb.VersionID,
                    ChecklistID = questionDb.ChecklistID,
                    InspectionID = questionAnswersInsertRequest.InspectionID

                };
                var answers = _iInspectionRepository.DeleteChecklistForInpection(questionList);
                if (answers > 0)
                {
                    response.IsSuccess = true;
                }
            }

            return response;
        }

        public QCResultsResponse GetQCResults()
        {
            var results = _iInspectionRepository.GetQCResults();
            var response = new List<QCResult>();

            foreach (var resultDb in results)
            {
                response.Add(new QCResult
                {
                    ResultID = resultDb.ResultID,
                    ResultName = resultDb.ResultName
                });
            }

            return new QCResultsResponse { Results = response };
        }

        public BaseResponse UpdateInspectionResult(InpsectionConclusionRequest inpsectionResultRequest)
        {
            var updateResultDb = _iInspectionRepository.UpdateInspectionResult(inpsectionResultRequest);
            var response = new BaseResponse();

            if (updateResultDb > 0)
            {
                response.IsSuccess = true;
            }
            else
            {
                response.ErrorMessage = "Update has failed";
            }
            return response;
        }

    }

    }
