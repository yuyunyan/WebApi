using System;
using System.Collections.Generic;
using System.Linq;
using Sourceportal.DB.Comments;
using Sourceportal.DB.Enum;
using Sourceportal.DB.PurchaseOrders;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.API.Requests.PurchaseOrders;
using Sourceportal.Domain.Models.API.Responses.PurchaseOrders;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Domain.Models.DB.PurchaseOrders;
using SourcePortal.Services.Shared.Middleware;
using ExtraListResponse = Sourceportal.Domain.Models.API.Responses.PurchaseOrders.ExtraListResponse;
using SourcePortal.Services.Items;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.DB.Accounts;
using Sourceportal.DB.CommonData;
using SourcePortal.Services.Ownership;
using Sourceportal.DB.User;
using System.Globalization;
using Sourceportal.DB.OrderFillment;
using Sourceportal.DB.SalesOrders;
using Sourceportal.Domain.Models.Middleware.Enums;
using Sourceportal.Domain.Models.Shared;
using Sourceportal.Domain.Models.Middleware.PurchaseOrder;
using Sourceportal.Domain.Models.Middleware;

namespace SourcePortal.Services.PurchaseOrders
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ICommentRepository _commentRepository;        
        private readonly IMiddlewareService _middlewareService;
        private readonly IItemService _itemService;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommonDataRepository _commonDataRepo;
        private readonly IOwnershipService _ownershipService;
        private readonly IUserRepository _userRepo;
        private readonly IOrderFillmentRepository _ofRepo;
        private readonly ISalesOrderRepository _soRepo;
        private readonly IPurchaseOrderMiddlewareClient _middlewareClient;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository, ICommentRepository commentRepository, 
            IPoSyncRequestCreator poSyncRequestCreator, IMiddlewareService middlewareService, IItemService itemService, IAccountRepository accountRepo,
            ICommonDataRepository commonRepo, IOwnershipService ownershipService, IUserRepository userRepo, IOrderFillmentRepository ofRepo, ISalesOrderRepository soRepo, IPurchaseOrderMiddlewareClient middlewareClient)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _commentRepository = commentRepository;            
            _middlewareService = middlewareService;
            _itemService = itemService;
            _accountRepository = accountRepo;
            _commonDataRepo = commonRepo;
            _ownershipService = ownershipService;
            _userRepo = userRepo;
            _ofRepo = ofRepo;
            _soRepo = soRepo;

            _middlewareClient = middlewareClient;
        }

        public CurrencyListResponse GetCurrencies()
        {
            var response = new CurrencyListResponse();
           var currenciesDb = _purchaseOrderRepository.GetCurrencies();
            var currencies = new List<CurrencyResponse>();

            foreach(var currency in currenciesDb)
            {
                var c = new CurrencyResponse
                {
                    CurrencyID = currency.CurrencyID,
                    Name = currency.CurrencyName,
                    ExternalID = currency.ExternalID
                };

                currencies.Add(c);
            }

            response.Currencies = currencies;
            response.IsSuccess = true;

            return response;
        }
        
        public PurchaseOrderDetailsSetResponse SetPurchaseOrderDetails(SetPurchaseOrderDetailsRequest setPoRequest)
        {
            var setPoDetailsDb = _purchaseOrderRepository.SetPurchaseOrderDetails(setPoRequest);

            if (string.IsNullOrEmpty(setPoDetailsDb.ExternalId))
            {
                _purchaseOrderRepository.UpdateWarehouseOnPurchaseOrderLines(setPoDetailsDb.ToWarehouseID, setPoDetailsDb.PurchaseOrderID);
            }

            var response = new PurchaseOrderDetailsSetResponse();
            response.PurchaseOrderId= setPoDetailsDb.PurchaseOrderID;
            response.VersionId = setPoDetailsDb.VersionID;
            return response;
        }

        public PurchaseOrderResponse GetPurchaseOrderDetails(int purchaseOrderId, int versionId, bool checkForPendingTransactions)
        {
            var purchaseOrdersDb = _purchaseOrderRepository.GetPurchaseOrderDetails(purchaseOrderId, versionId);
            var findPendingTransaction = checkForPendingTransactions 
                                         && _middlewareService.IsTherePendingTransactions(MiddlewareObjectTypes.PurchaseOrder.ToString(), purchaseOrderId, null);

            var po = new PurchaseOrderResponse
            {
                PurchaseOrderID = purchaseOrdersDb.PurchaseOrderID,
                VersionID = purchaseOrdersDb.VersionID,
                AccountID = purchaseOrdersDb.AccountID,
                AccountName = purchaseOrdersDb.AccountName,
                ContactID = purchaseOrdersDb.ContactID,
                ContactFirstName = purchaseOrdersDb.ContactFirstName,
                ContactLastName = purchaseOrdersDb.ContactLastName,
                ContactPhone = purchaseOrdersDb.OfficePhone,
                ContactEmail = purchaseOrdersDb.Email,
                StatusID = purchaseOrdersDb.StatusID,
                StatusName = purchaseOrdersDb.StatusName,
                CountryName = purchaseOrdersDb.CountryName,
                OrganizationID = purchaseOrdersDb.OrganizationID,
                OrganizationName = purchaseOrdersDb.OrganizationName,
                OrderDate = purchaseOrdersDb.OrderDate,
                Cost = purchaseOrdersDb.Cost,
                CurrencyID = purchaseOrdersDb.CurrencyID,
                BillFromLocationID = purchaseOrdersDb.BillFromLocationID,
                BillToLocationName = purchaseOrdersDb.BillToLocationName,
                BillToHouseNumber = purchaseOrdersDb.BillToHouseNumber,
                BillToCity = purchaseOrdersDb.BillToCity,
                BillToStreet = purchaseOrdersDb.BillToStreet,
                BillToStateCode = purchaseOrdersDb.BillToStateCode,
                BillToPostalCode = purchaseOrdersDb.BillToPostalCode,
                ShipFromLocationID = purchaseOrdersDb.ShipFromLocationID,
                ShipFromLocationName = purchaseOrdersDb.ShipFromLocationName,
                ShipFromHouseNumber = purchaseOrdersDb.ShipFromHouseNumber,
                ShipFromCity = purchaseOrdersDb.ShipFromCity,
                ShipFromStreet = purchaseOrdersDb.ShipFromStreet,
                ShipFromStateCode = purchaseOrdersDb.ShipFromStateCode,
                ShipFromPostalCode = purchaseOrdersDb.ShipFromPostalCode,
                ShippingMethodID = purchaseOrdersDb.ShippingMethodID,
                PaymentTermID = purchaseOrdersDb.PaymentTermID,
                IncotermID = purchaseOrdersDb.IncotermID,
                ToWarehouseID = purchaseOrdersDb.ToWarehouseID,
                Owners = purchaseOrdersDb.Owners,
                PONotes = purchaseOrdersDb.PONotes,
                ExternalID = purchaseOrdersDb.ExternalId,
                UserID = UserHelper.GetUserId(),
                HasPendingTransaction = findPendingTransaction
            };

            return po;
        }
        public PurchaseOrderListResponse GetPurchaseOrderList(SearchFilter searchFilter)
        {
            var response = new PurchaseOrderListResponse();

            var purchaseOrdersDb = _purchaseOrderRepository.GetPurchaseOrderList(searchFilter);

            var purchaseOrders = new List<PurchaseOrderResponse>();

            int TotalCount = 0;

            foreach (var purchaseOrder in purchaseOrdersDb)
            {
                var po = new PurchaseOrderResponse
                {
                    PurchaseOrderID = purchaseOrder.PurchaseOrderID,
                    VersionID = purchaseOrder.VersionID,
                    AccountID = purchaseOrder.AccountID,
                    AccountName = purchaseOrder.AccountName,
                    ContactID = purchaseOrder.ContactID,
                    ContactFirstName = purchaseOrder.ContactFirstName,
                    ContactLastName = purchaseOrder.ContactLastName,
                    StatusID = purchaseOrder.StatusID,
                    StatusName = purchaseOrder.StatusName,
                    CountryName = purchaseOrder.CountryName,
                    OrganizationID = purchaseOrder.OrganizationID,
                    OrganizationName = purchaseOrder.OrganizationName,
                    OrderDate = purchaseOrder.OrderDate,
                    Owners = purchaseOrder.Owners,
                    ExternalID = purchaseOrder.ExternalId
                };

                purchaseOrders.Add(po);
            }

            if (purchaseOrdersDb.Count() > 0)
            {
                TotalCount = purchaseOrdersDb[0].TotalRows;
            }

            response.PurchaseOrders = purchaseOrders;
            response.TotalRowCount = TotalCount;
            response.IsSuccess = true;

            return response;
        }

        public GetPurchaseOrderLinesResponse GetPurchaseOrderLines(int poId, int poVersionId, SearchFilter searchFilter)
        {
            var poLines = new GetPurchaseOrderLinesResponse();
            var response = new List<PurchaseOrderLineDetail>();

            var dbPoLines = _purchaseOrderRepository.GetPurchaseOrderLines(poId, poVersionId, searchFilter);

            foreach (var line in dbPoLines)
            {
                response.Add(CreatePurchaseOrderLine(line));
            }

            poLines.POLinesResponse = response;

            return poLines;
        }

        private static PurchaseOrderLineDetail CreatePurchaseOrderLine(PurchaseOrderLinesDb line)
        {
            return new PurchaseOrderLineDetail
            {
                POLineId = line.POLineId,
                StatusId = line.StatusId,
                StatusName = line.StatusName,
                StatusIsCanceled = line.StatusIsCanceled,
                LineNum = (line.LineNum + "." + line.LineRev),
                VendorLine = line.VendorLine,
                ItemId = line.ItemId,
                PartNumber = line.PartNumber,
                CommodityName = line.CommodityName,
                Qty = line.Qty,
                Cost = line.Cost,
                DateCode = line.DateCode,
                PackingId = line.PackagingId,
                PackagingName = line.PackagingName,
                ConditionID = line.PackageConditionID,
                ConditionName = line.ConditionName,
                DueDate = line.DueDate?.ToString("MM/dd/yyyy"),
                PromisedDate = line.PromisedDate?.ToString("MM/dd/yyyy"),
                MfrName = line.MfrName,
                TotalRows = line.TotalRows,
                Comments = line.Comments,
                IsSpecBuy =  line.IsSpecBuy,
                SpecBuyForAccountID = line.SpecBuyForAccountID,
                SpecBuyForUserID = line.SpecBuyForUserId,
                SpecBuyReason = line.SpecBuyReason,
                AllocatedQty = line.AllocatedQty,
                AllocatedSalesOrderId = line.SalesOrderID,
                AllocatedSalesOrderVersionId = line.SOVersionID,
                ExternalID = line.ExternalID
            };
        }

        public PurchaseOrderLineDetail SetPurchaseOrderLine(SetPurchaseOrderLineRequest setPurchaseOrderLineRequest)
        {
            var dbPurchaseOrder = _purchaseOrderRepository.GetPurchaseOrderDetails(setPurchaseOrderLineRequest.PurchaseOrderId, (int)setPurchaseOrderLineRequest.POVersionId);

            SetWarehouseIdWhenNonSyncedPoOrNewLines(dbPurchaseOrder, setPurchaseOrderLineRequest);

            var dbSetPurchaseOrderLine = _purchaseOrderRepository.SetPurchaseOrderLine(setPurchaseOrderLineRequest);
            var response = CreatePurchaseOrderLine(dbSetPurchaseOrderLine);
            
            return response;
        }

        public PurchaseOrderLinesDeleteResponse DeletePurchaseOrderLines(List<int> poLineIds)
        {
            var dbDeletePurchaseOrderLines = _purchaseOrderRepository.DeletePurchaseOrderLines(poLineIds);
            var response = new PurchaseOrderLinesDeleteResponse();

            if (dbDeletePurchaseOrderLines != 0)
            {
                response.IsSuccess = true;
            }

            return response;
        }

        public PurchaseOrderExtraResponse GetPurchaseOrderExtra(int poId, int poVersionId, int rowOffset, int rowLimit)
        {
            var dbPurchaseOrderExtra = _purchaseOrderRepository.GetPurchaseOrderExtras(poId, poVersionId, rowOffset, rowLimit);
            var response = new List<ExtraListResponse>();

            foreach (var value in dbPurchaseOrderExtra)
            {
                response.Add(new ExtraListResponse
                {
                    POExtraId = value.POExtraID,
                    LineNum = value.LineNum,
                    RefLineNum = value.RefLineNum,
                    ItemExtraId = value.ItemExtraId,
                    ExtraName = value.ExtraName,
                    ExtraDescription = value.ExtraDescription,
                    Note = value.Note,
                    Qty = value.Qty,
                    Cost = value.Cost,
                    StatusId = value.StatusId,
                    StatusName = value.StatusName,
                    Comments = value.Comments,
                    PrintOnPO = value.PrintOnPO,
                    TotalRows = value.TotalRows

                });
            }
            return new PurchaseOrderExtraResponse() { ExtraListResponse = response };
        }

        public SetPurchaseOrderExtraResponse SetPurchaseOrderExtra(SetPurchaseOrderExtraRequest setPurchaseOrderExtraRequest)
        {
            var dbSetPurchaseOrderExtra = _purchaseOrderRepository.SetPurchaseOrderExtra(setPurchaseOrderExtraRequest);
            var response = new SetPurchaseOrderExtraResponse();

            response.POExtraId = dbSetPurchaseOrderExtra.POExtraID;
            response.LineNum = dbSetPurchaseOrderExtra.LineNum;

            return response;
        }

        public PurchaseOrderExtraDeleteResponse DeletePurchaseOrderExtras(List<int> poExtraIds)
        {
            var dbDeletePurchaseOrderExtras = _purchaseOrderRepository.DeletePurchaseOrderExtras(poExtraIds);
            var response = new PurchaseOrderExtraDeleteResponse();

			
            if (dbDeletePurchaseOrderExtras != 0)
            {
                response.IsSuccess = true;
            }

            return response;
        }

        public PurchaseOrderDetailsSetResponse PurchaseOrderFromFlaggedSet(
            SetPurchaseItemsFlaggedRequest setPurchaseItemsFlaggedRequest)
        {
            var poDetailRequest = setPurchaseItemsFlaggedRequest.PurchaseOrderDetails;
            poDetailRequest.CurrencyId = "USD";
            poDetailRequest.OrderDate = DateTime.Today.ToString("MM/dd/yyyy");

            var poDetailDb = _purchaseOrderRepository.SetPurchaseOrderDetails(poDetailRequest);

            var poId = poDetailDb.PurchaseOrderID;
            var poVersionId = poDetailDb.VersionID;

            var purchaseOrderLines = setPurchaseItemsFlaggedRequest.PurchaseOrderLines;
            var poLines = _purchaseOrderRepository.SetPoLines(poId, poVersionId, purchaseOrderLines);

            var commentId = _commentRepository.SetComment(new SetCommentRequest
            {
                ObjectID = poId,
                ObjectTypeID = (int) ObjectType.Purchaseorder,
                Comment = setPurchaseItemsFlaggedRequest.Comment,
                CommentTypeID = (int)CommentType.SalesPurchasing
            }).CommentID;

            return new PurchaseOrderDetailsSetResponse
            {
                PurchaseOrderId = poId,
                VersionId = poVersionId,
                IsSuccess = (purchaseOrderLines.Count == poLines.Count) && (setPurchaseItemsFlaggedRequest.Comment == null || commentId > 0)
            };
        }

        public ItemMfrResponse GetManufactuerItem(int itemId)
        {
            var mfrName = _purchaseOrderRepository.GetManufactuerItem(itemId);
            return new ItemMfrResponse
            {
                IsSuccess = true,
                MfrName = mfrName
            };
        }


        public SyncResponse Sync(int poId, int poVersionId)
        {
            return _middlewareClient.Sync(poId, poVersionId);
        }

        public void SetPurchaseOrderSapData(SetPurchaseOrderSapDataRequest request)
        {
            _purchaseOrderRepository.SetExternalId(request.ObjectId, request.ExternalId);

            foreach(var item in request.Items)
            {
                _itemService.SetItemExternalId(new SetExternalIdRequest { ObjectId = item.Id, ExternalId = item.ExternalId });
            }
        }

        public BaseResponse HandlingIncomingPurchaseOrderSapUpdate(PurchaseOrderIncomingSapResponse request)
        {
            var poDb = _purchaseOrderRepository.GetPurchaseOrderFromExternal(request.ExternalId);
            var isNewPO = poDb == null || poDb.PurchaseOrderID < 1;
            List<string> allocationErrors = new List<string>();

            int checkAccountId = _accountRepository.GetAccountIdByExternal(request.AccountExternalId);
            var account = _accountRepository.GetAccountBasicDetails(checkAccountId);
            if (account.IsSourceability)
            {
                return new BaseResponse(); //we dont want to process sales orders that have this type of account
            }

            bool checkLines = true;
            foreach(var line in request.Lines)
            {
                if(line.ProductType == "material")
                {
                    checkLines = false;
                }
            }

            if (checkLines)
            {
                return new BaseResponse();
            }

            var savePO = SetIncomingPurchaseOrder(request, poDb);

            if(savePO != null && savePO.PurchaseOrderID > 0)
            {
                List<int> sapLineIds = new List<int>();

                foreach(var line in request.Lines)
                {
                    if(line.ProductType == "service")
                    {
                        continue;
                    }

                    var savePOLine = SetIncomingPurchaseOrderLine(request.ExternalId, line, savePO, isNewPO);

                    if (savePOLine == null || savePOLine.POLineId < 1)
                    {
                        return new BaseResponse { ErrorMessage = "Could not create / update PO Line with LineNum " + line.LineNum };
                    }
                    else
                    {
                        //check for anymore existing allocations if line is deleted and delete it
                        if (line.IsDeleted)
                        {
                            var allocation = _purchaseOrderRepository.GetPOAllocationFromLine(savePOLine.POLineId);

                            if(allocation != null)
                                _ofRepo.SetOrderFulfillmentQty(allocation.SOLineID, allocation.POLineID, "Purchase Order", allocation.Qty, true);

                            continue;
                        }

                        if (!string.IsNullOrEmpty(savePOLine.Error))
                        {
                            allocationErrors.Add(savePOLine.Error);
                        }

                        sapLineIds.Add(savePOLine.POLineId);
                    }
                }

                var allLines = _purchaseOrderRepository.GetPurchaseOrderLines(savePO.PurchaseOrderID, savePO.VersionID, new SearchFilter { RowLimit = 999999 });
                List<int> deleteLines = new List<int>();
                foreach(var line in allLines)
                {
                    if (!sapLineIds.Contains(line.POLineId))
                    {
                        deleteLines.Add(line.POLineId);
                    }
                }

                var deletedLinesCount = deleteLines.Count > 0 ? _purchaseOrderRepository.DeletePurchaseOrderLines(deleteLines) : 0;

                if(deletedLinesCount != deleteLines.Count)
                {
                    return new BaseResponse { ErrorMessage = "Could not delete all lines from Incoming Purchase Order" };
                }
            }
            else
            {
                return new BaseResponse { ErrorMessage = "Could not create new PO from ExternalID " + request.ExternalId };
            }

            BaseResponse res = new BaseResponse();
            if (allocationErrors.Count > 0)
            {
                res.ErrorMessage = String.Join(", ", allocationErrors);
            }
            return res;
        }

        private PurchaseOrderDb SetIncomingPurchaseOrder(PurchaseOrderIncomingSapResponse sapPO, PurchaseOrderDb poDb)
        {
            SetPurchaseOrderDetailsRequest po = new SetPurchaseOrderDetailsRequest();

            po.PurchaseOrderId = poDb != null && poDb.PurchaseOrderID > 0 ? poDb.PurchaseOrderID : 0;
            po.VersionId = poDb != null && poDb.VersionID > 0 ? poDb.VersionID : 1;
            po.ExternalId = sapPO.ExternalId;
            po.AccountId = _accountRepository.GetAccountIdByExternal(sapPO.AccountExternalId);
            po.OrganizationId = sapPO.OrgExternalId != null ? _commonDataRepo.GetAllOrganizations().First(x => x.ExternalID == sapPO.OrgExternalId).OrganizationID : 0;
            po.IncotermId = sapPO.IncotermExternalId != null ? _commonDataRepo.GetAllIncoterms().First(x => x.ExternalID == sapPO.IncotermExternalId).IncotermID : 0;
            po.PaymentTermId = sapPO.PaymentTermExternalId != null ? _commonDataRepo.GetPaymentTermIdByExternal(sapPO.PaymentTermExternalId) : 0;
            po.CurrencyId = sapPO.CurrencyExternalId != null ? _commonDataRepo.GetCurrencyIdByExternal(sapPO.CurrencyExternalId) : null;
            po.OrderDate = sapPO.OrderDate != new DateTime() ? sapPO.OrderDate.ToString("d", DateTimeFormatInfo.InvariantInfo) : null;
            //TODO: ShippingMethod

            return _purchaseOrderRepository.SetPurchaseOrderDetails(po);
        }

        private PurchaseOrderLinesDb SetIncomingPurchaseOrderLine(string externalId, PurchaseOrderLineDetails sapLine, PurchaseOrderDb parentPO, bool isNewPO)
        {
            int poLineId = _purchaseOrderRepository.GetPoLineIdFromExternal(externalId, sapLine.LineNum);
            SetPurchaseOrderLineRequest poLine = CreatePurchaseOrderLineForIncoming(poLineId, parentPO.PurchaseOrderID, parentPO.VersionID, sapLine);

            var result = _purchaseOrderRepository.SetPurchaseOrderLine(poLine);

            if(result != null && result.POLineId > 0)
            {
                var existingAlloc = _purchaseOrderRepository.GetPOAllocationFromLine(result.POLineId);
                var existingProductSpec = existingAlloc != null && existingAlloc.SOLineID > 0 ? _soRepo.GetProductSpecId(existingAlloc.SOLineID) : null;
                var sapAlloc = _purchaseOrderRepository.GetPOAllocationFromProductSpec(sapLine.ProductSpec, result.POLineId);
                var sapSOLineID = _soRepo.GetSoLineIdFromProductSpec(sapLine.ProductSpec);

                if (sapSOLineID < 1 && !string.IsNullOrEmpty(sapLine.ProductSpec))
                {
                    result.Error = "-Allocation Error- Product Spec from Sap does not exist: " + sapLine.ProductSpec;
                    return result;
                }

                if(existingAlloc == null && string.IsNullOrEmpty(sapLine.ProductSpec))
                {
                    //do nothing
                    return result;
                }

                if (NoAllocationAndSapProdSpecExists(sapLine, existingAlloc) && !sapLine.IsDeleted)
                {
                    //create new allocation
                    _ofRepo.SetOrderFulfillmentQty(sapSOLineID, result.POLineId, "Purchase Order", result.Qty, false);
                }
                else if ((existingAlloc != null && existingAlloc.SOLineID > 0) && string.IsNullOrEmpty(sapLine.ProductSpec))
                {
                    //remove allocation
                    _ofRepo.SetOrderFulfillmentQty(existingAlloc.SOLineID, result.POLineId, "Purchase Order", 0, true);
                }
                else if (!string.IsNullOrEmpty(existingProductSpec) && existingProductSpec != sapLine.ProductSpec)
                {
                    //update allocation with new so line id
                    _ofRepo.SetOrderFulfillmentQty(existingAlloc.SOLineID, result.POLineId, "Purchase Order", 0, true);
                    _ofRepo.SetOrderFulfillmentQty(sapSOLineID, result.POLineId, "Purchase Order", result.Qty, sapLine.IsDeleted);
                }
                else if (!string.IsNullOrEmpty(existingProductSpec) && existingProductSpec == sapLine.ProductSpec)
                {
                    //update qty on allocation
                    _ofRepo.SetOrderFulfillmentQty(existingAlloc.SOLineID, result.POLineId, "Purchase Order", result.Qty, sapLine.IsDeleted);
                }

            }

            return result;
        }

        private static bool NoAllocationAndSapProdSpecExists(PurchaseOrderLineDetails sapLine, PurchaseOrderSOAllocation existingAlloc)
        {
            return (existingAlloc == null || existingAlloc.SOLineID < 1) && !string.IsNullOrEmpty(sapLine.ProductSpec);
        }

        private SetPurchaseOrderLineRequest CreatePurchaseOrderLineForIncoming(int poLineId, int poId, int versionId, PurchaseOrderLineDetails sapLine)
        {
            int lineNum, lineRev;
            SetPurchaseOrderLineRequest poLine = new SetPurchaseOrderLineRequest();

            if (sapLine.LineNum.Contains("."))
            {
                lineNum = Int32.Parse(sapLine.LineNum.Split('.').First().Trim());
                lineRev = Int32.Parse(sapLine.LineNum.Split('.').Last().Trim());
            }
            else
            {
                lineNum = Int32.Parse(sapLine.LineNum);
                lineRev = 0;
            }

            poLine.POLineId = poLineId;
            poLine.PurchaseOrderId = poId;
            poLine.POVersionId = versionId;
            poLine.LineNum = lineNum;
            poLine.LineRev = lineRev;
            poLine.ItemId = _itemService.GetItemIdFromExternal(sapLine.ItemExternalId);
            poLine.Qty = sapLine.Qty;
            poLine.Cost = (double)sapLine.Cost;
            poLine.DateCode = sapLine.DateCode;
            poLine.PackagingId = _commonDataRepo.GetPackagingIdFromExternal(sapLine.PackagingExternalId);
            poLine.PackageConditionID = _commonDataRepo.GetPackageConditionIdFromExternal(sapLine.PackageConditionExternalId);
            poLine.DueDate = sapLine.DueDate.ToString("d", DateTimeFormatInfo.InvariantInfo);
            poLine.IsSpecBuy = sapLine.IsSpecBuy;
            poLine.SpecBuyReason = sapLine.SpecBuyReason;
            poLine.ToWarehouseID = sapLine.ToWarehouseExternalId != null ? 
                _commonDataRepo.GetWarehouses(null).Where(x => x.ExternalID == sapLine.ToWarehouseExternalId).FirstOrDefault().WarehouseID : 0;
            poLine.IsDeleted = sapLine.IsDeleted;

            return poLine;
        }

        //Do not change the ToWarehouseID on already synced lines 
        private int SetWarehouseIdWhenNonSyncedPoOrNewLines(PurchaseOrderDb dbPurchaseOrder, SetPurchaseOrderLineRequest setPurchaseOrderLineRequest)
        {
            if (string.IsNullOrEmpty(dbPurchaseOrder.ExternalId) || setPurchaseOrderLineRequest.POLineId < 1)
            {
                setPurchaseOrderLineRequest.ToWarehouseID = dbPurchaseOrder.ToWarehouseID;
            }
            return setPurchaseOrderLineRequest.ToWarehouseID;
        }

        public string ValidateSync(int purchaseOrderId, int versionId)
        {
            SearchFilter searchfilter = new SearchFilter
            {
                SearchString = "",
                RowOffset = 0,
                RowLimit = 100000,
                SortCol = "",
                DescSort = false
            };

            var poDetails = GetPurchaseOrderDetails(purchaseOrderId, versionId,false);
            var poLines = GetPurchaseOrderLines(purchaseOrderId, versionId, searchfilter);

            if (string.IsNullOrEmpty(poDetails.ExternalID))
            {
                if (poLines.POLinesResponse.Count > 0)
                {
                    return PurchaseOrderSyncStatuses.Valid.GetEnumDescription();
                }
                else
                {
                    return PurchaseOrderSyncStatuses.NewPoNoLines.GetEnumDescription();
                }
            }
            else
            {
                if (poLines.POLinesResponse.Count > 0)
                {
                    return PurchaseOrderSyncStatuses.Valid.GetEnumDescription();
                }
                else
                {
                    return PurchaseOrderSyncStatuses.SyncedPoNoLines.GetEnumDescription();
                }
            }
        }

        public PurchaseOrderLineHistoryResponse GetPurchaseOrderByAccountId(int accountId,int? contactId=null)
        {
            var poLinesDb = _purchaseOrderRepository.GetPurchaseOrderByAccountId(accountId,contactId);
            var response = new List<PurchaseOrderLineResponse>();
            var poLineResponse = new PurchaseOrderLineHistoryResponse();

            foreach (var value in poLinesDb)
            {
                response.Add(new PurchaseOrderLineResponse
                {
                    POLineId = value.POLineId,
                    LineNum = value.LineNum,
                    LineRev = value.LineRev,
                    OrderDate = value.OrderDate,
                    PartNumber = value.PartNumber,
                    ItemID = value.ItemID,
                    POExternalID = value.POExternalID,
                    MfrName = value.MfrName,
                    Qty = value.Qty,
                    Price = value.Price,
                    Cost = value.Cost,
                    GPM = value.GPM,
                    DateCode = value.DateCode,
                    Packaging = value.PackagingName,
                    PurchaseOrderID = value.PurchaseOrderID,
                    VersionID = value.VersionID,
                    AccountId = value.AccountId,
                    AccountName = value.AccountName,
                    ContactId = value.ContactId,
                    FirstName = value.FirstName,
                    LastName = value.LastName,
                    StatusId = value.StatusId,
                    StatusName = value.StatusName,
                    WareHouseName = value.WareHouseName,
                    FullName = FullNameFormatter.FormatFullName(value.FirstName,value.LastName),
                    Owners = CleanLists(value.Owners)
                });

            }
            poLineResponse.PurchaseOrderLineList = response;
            return poLineResponse;
        }
        private static string CleanLists(string stringToClean)
        {
            return !string.IsNullOrEmpty(stringToClean) ? stringToClean.Trim().TrimEnd(',') : null;
        }
    }
}
