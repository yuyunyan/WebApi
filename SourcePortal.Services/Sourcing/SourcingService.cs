using System;
using System.Collections.Generic;
using System.Linq;
using Sourceportal.DB.Accounts;
using Sourceportal.DB.Items;
using Sourceportal.DB.Ownership;
using Sourceportal.DB.PurchaseOrders;
using Sourceportal.DB.Sourcing;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.API.Requests.PurchaseOrders;
using Sourceportal.Domain.Models.API.Requests.Sourcing;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.PurchaseOrders;
using Sourceportal.Domain.Models.API.Responses.Sourcing;

namespace SourcePortal.Services.Sourcing
{
    public class SourcingService : ISourcingService
    {
        private readonly ISourcingRepository _sourcingRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IOwnershipRepository _ownershipRepository;
        private readonly IItemRepository _itemRepository;

        public SourcingService(ISourcingRepository sourcingRepository, IAccountRepository accountRepository,
            IPurchaseOrderRepository purchaseOrderRepository, IOwnershipRepository ownershipRepository,
            IItemRepository itemRepository)
        {
            _sourcingRepository = sourcingRepository;
            _accountRepository = accountRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _ownershipRepository = ownershipRepository;
            _itemRepository = itemRepository;
        }

        
        public SourcingQuoteLinesListResponse GetSourcingQuoteLines(SourcingQuoteLinesFilter sourcingFilter)
        {
            var dbSourcingQuoteLines = _sourcingRepository.GetSourcingQuoteList(sourcingFilter);
            var list = new List<SourcingQuoteLinesResponse>();
            int TotalCount = 0;
            foreach (var dbSourcingQuoteLine in dbSourcingQuoteLines)
            {
                list.Add(new SourcingQuoteLinesResponse
                {
                    QuoteLineID = dbSourcingQuoteLine.QuoteLineID,
                    QuoteID = dbSourcingQuoteLine.QuoteID,
                    QuoteVersionID = dbSourcingQuoteLine.QuoteVersionID,
                    AccountID = dbSourcingQuoteLine.AccountID,
                    AccountName = dbSourcingQuoteLine.AccountName,
                    LineNum = dbSourcingQuoteLine.LineNum,
                    PartNumber = dbSourcingQuoteLine.PartNumber,
                    PartNumberStrip = dbSourcingQuoteLine.PartNumberStrip,
                    Manufacturer = dbSourcingQuoteLine.Manufacturer,
                    CommodityID = dbSourcingQuoteLine.CommodityID,
                    CommodityName = dbSourcingQuoteLine.CommodityName,
                    StatusID = dbSourcingQuoteLine.StatusID,
                    StatusName = dbSourcingQuoteLine.StatusName,
                    Qty = dbSourcingQuoteLine.Qty,
                    PackagingID = dbSourcingQuoteLine.PackagingID,
                    PackagingName = dbSourcingQuoteLine.PackagingName,
                    DateCode = dbSourcingQuoteLine.DateCode,
                    Comments = dbSourcingQuoteLine.Comments,
                    SourcesCount = dbSourcingQuoteLine.SourcesCount,
                    RFQCount = dbSourcingQuoteLine.RFQCount,
                    Price = dbSourcingQuoteLine.Price,
                    Cost = dbSourcingQuoteLine.Cost,
                    ItemID = dbSourcingQuoteLine.ItemID,
                    DueDate = dbSourcingQuoteLine.DueDate,
                    ShipDate = dbSourcingQuoteLine.ShipDate,
                    CustomerLine = dbSourcingQuoteLine.CustomerLine,
                    CustomerPartNumber = dbSourcingQuoteLine.CustomerPartNumber ,
                    ItemListLineID = dbSourcingQuoteLine.ItemListLineID,
                    QuoteTypeName = dbSourcingQuoteLine.TypeName,
                    Owners = dbSourcingQuoteLine.Owners

                });
            }
            if (dbSourcingQuoteLines.Count() > 0)
                TotalCount = dbSourcingQuoteLines[0].TotalRows;
            return new SourcingQuoteLinesListResponse { SourcingQuoteLinesList = list, TotalRows = TotalCount };
        }

        public SourcingStatusesListResponse GetSourcingStatuses()
        {
            var dbSourcingStatuses = _sourcingRepository.GetSourcingStatuses();
            var list = new List<SourcingStatusesResponse>();
            foreach (var dbSourcingStatus in dbSourcingStatuses)
            {
                list.Add(new SourcingStatusesResponse
                {
                    StatusID = dbSourcingStatus.StatusID,
                    StatusName = dbSourcingStatus.StatusName,
                    IsDefault = dbSourcingStatus.IsDefault,

                });
            }
            return new SourcingStatusesListResponse { SourcingStatusesList = list};
        }

        public SourcingRouteStatusesResponse GetRouteSatatuses()
        {
            var routeStatusDbs = _sourcingRepository.GetRouteStatuses();
            var routeStatuses = new List<RouteStatusesResponse>();
            foreach (var routeStatusDb in routeStatusDbs)
            {
                routeStatuses.Add(new RouteStatusesResponse
                {
                    RouteStatusID = routeStatusDb.RouteStatusID,
                    StatusName = routeStatusDb.StatusName,
                    IsComplete = routeStatusDb.IsComplete,
                    IsDefault = routeStatusDb.IsDefault,
                    CountQuoteLines = routeStatusDb.CountQuoteLines
                });
            }
            return new SourcingRouteStatusesResponse
            {
                RouteStatuses = routeStatuses
            };
        }

        public BaseResponse SetBuyerRoutes(SetBuyerRouteRequest setBuyerRouteRequest)
        {
            var rowCount = _sourcingRepository.SetBuyerRoute(setBuyerRouteRequest);
            return new BaseResponse
            {
                IsSuccess = rowCount == setBuyerRouteRequest.QuoteLines.Count
            };
        }

        public QuoteLineBuyersGetResponse QuoteLineBuyersGet(int quoteLineId)
        {
            var buyerNameDbs = _sourcingRepository.QuoteLineRouteBuyersGet(quoteLineId);
            var buyerNames = buyerNameDbs.Select(buyerNameDb => buyerNameDb.BuyerName).ToList();
            return new QuoteLineBuyersGetResponse
            {
                BuyerNames = buyerNames
            };
        }

        public SourceTypesListResponse GetSourceTypes()
        {
            var dbSourceTypes = _sourcingRepository.GetSourceTypes();
            var list = new List<SourceTypesResponse>();
            foreach (var dbSourceType in dbSourceTypes)
            {
                list.Add(new SourceTypesResponse
                {
                    TypeName = dbSourceType.TypeName,
                    SourceTypeID = dbSourceType.SourceTypeID,

                });
            }
            return new SourceTypesListResponse { SourcingStatusesList = list };
        }

        public List<SourceGridExportLine> MapSourceLinesToExport(List<SourceResposne> sourceLines)
        {
            var exportsLines = new List<SourceGridExportLine>();
            foreach (var soLine in sourceLines)
            {
                exportsLines.Add(new SourceGridExportLine
                {
                    DateCode = soLine.DateCode,
                    PartNumber = soLine.PartNumber,
                    Buyer = soLine.CreatedBy,
                    Commodity = soLine.CommodityName,
                    Date = soLine.Created,
                    Mfr = soLine.Manufacturer,
                    Packaging = soLine.PackagingName,
                    Type = soLine.TypeName,
                    LeadTimeDays = soLine.LeadTimeDays,
                    Cost = soLine.Cost,
                    Rating = soLine.Rating,
                    MOQ = soLine.MOQ,
                    Quantity = soLine.Qty,
                    Supplier = soLine.Supplier
                });
            }
            return exportsLines;
        }

        public List<RTPSourceExportLine> MapRTPSourceLinesToExport(List<SourceResposne> sourceLines, float soPrice)
        {
            var exportsLines = new List<RTPSourceExportLine>();
            foreach (var soLine in sourceLines)
            {
                string ageString;
                if (soLine.AgeInDays > 6)
                {
                    var week = soLine.AgeInDays / 7.0;
                    ageString = " (" + (week.ToString("0.0")) + "w)";
                }
                else
                {
                    ageString = " (" + soLine.AgeInDays + "d)";
                }

                string leadTimeString;
                if (soLine.LeadTimeDays % 7 == 0)
                {
                    leadTimeString = soLine.LeadTimeDays / 7 + " w";
                }
                else
                {
                    leadTimeString = soLine.LeadTimeDays + " d";
                }
                exportsLines.Add(new RTPSourceExportLine
                {
                    DateCode = soLine.DateCode,
                    PartNumber = soLine.PartNumber,
                    Buyer = soLine.CreatedBy,
                    Commodity = soLine.CommodityName,
                    Date = DateTime.Parse(soLine.Created).Date.ToString("MM/dd/yy") + ageString,
                    Mfr = soLine.Manufacturer,
                    Packaging = soLine.PackagingName,
                    Type = soLine.TypeName,
                    LeadTimeDays = leadTimeString,
                    Cost = soLine.Cost,
                    Rating = soLine.Rating,
                    MOQ = soLine.MOQ,
                    Margin = soPrice != 0? (soPrice - (float) soLine.Cost)/soPrice: 0,
                    RTPQty = soLine.RTPQty ?? 0,
                    SourceQty = soLine.Qty,
                    Vendor = soLine.Supplier,
                });
            }
            return exportsLines;
        }

        public SourceListResponse GetSourceList(int itemId, string partNumber, int objectId, int objectTypeId , bool showAll,bool showInventory)
        {
            var dbSourceList = _sourcingRepository.GetSourceList(itemId, partNumber, objectId, objectTypeId , showAll, showInventory);
            var response = new List<SourceResposne>();
            foreach (var value in dbSourceList)
            {
                response.Add(new SourceResposne
                {
                    PackagingName = value.PackagingName,
                    ItemId = value.ItemId,
                    PartNumber = value.PartNumber,
                    SourceId = value.SourceId,
                    SourceTypeId = value.SourceTypeId,
                    TypeName = value.TypeName,
                    TypeRank = value.TypeRank,
                    AccountId = value.AccountId,
                    ContactId = value.ContactId,
                    ContactName = value.ContactName,
                    Manufacturer = value.Manufacturer,
                    CommodityId = value.CommodityId,
                    CommodityName = value.CommodityName,
                    Qty = value.Qty,
                    DateCode = value.DateCode,
                    Cost = value.Cost,
                    PackagingId = value.PackagingId,
                    PackagingConditionID = value.PackageConditionID,
                    MOQ = value.MOQ,
                    SPQ = value.SPQ,
                    LeadTimeDays = value.LeadTimeDays,
                    ValidForHours = value.ValidForHours,
                    Supplier = value.AccountName,
                    IsMatched = value.IsMatch,
                    IsJoined = value.IsJoined,
                    showCheckmark = value.IsConfirmed,
                    Comments = value.Comments,
                    AgeInDays = value.AgeInDays,
                    Created = value.Created,
                    BuyerID = value.BuyerID,
                    CreatedBy = value.CreatedBy,
                    RTPQty = value.RTPQty,
                    Rating = value.Rating
                });

            }
            return new SourceListResponse { SourceResponse = response };
        }

        public SetSourceResponse SetSource(SetSourceRequest setSourceRequest)
        {
            var dbSetSource = _sourcingRepository.SetSource(setSourceRequest);
            var response = new SetSourceResponse();
            response.SourceId = dbSetSource.SourceId;

            return response;
        }

        public BaseResponse SetSourceStatus(SetSourceStatus setSourceStatus)
        {
            var success = _sourcingRepository.SetSourceStatus(setSourceStatus);
            return new BaseResponse
            {
                ErrorMessage = !success ? "Something went wrong" : null
            };
        }

        public SourceCommentUIDResponse GetSourceCommentUID(SourceCommentUIDRequest sourceCommentUidRequest)
        {
            var dbCommentUId = _sourcingRepository.GetSourceCommentUID(sourceCommentUidRequest);
            var response = new SourceCommentUIDResponse();
            response.CommentUID = dbCommentUId.CommentUID;

            return response;
        }
        public PurchaseOrderDetailsSetResponse SourceToPurchaseOrder(SourceToPORequest request)
        {
            try
            {
                var basicDetailsDb = _accountRepository.GetAccountBasicDetails(request.AccountID);
                var poCreateRequest = new SetPurchaseOrderDetailsRequest()
                {
                    AccountId = request.AccountID,
                    ContactId = request.ContactID,
                    CurrencyId = basicDetailsDb.CurrencyId,
                    PaymentTermId = request.PaymentTermID,
                    FromLocationId = request.ShipFrom,
                    StatusId = 29,
                    IncotermId = request.IncotermID,
                    IsDeleted = false,
                    ToWarehouseID = request.ShipTo,
                    OrganizationId = basicDetailsDb.OrganizationId,
                    OrderDate = DateTime.Now.ToString()
                };
                var ihsItemDict = new Dictionary<long, int>();
                var poDetailDb = _purchaseOrderRepository.SetPurchaseOrderDetails(poCreateRequest);
                var sourceDbs = _sourcingRepository.GetSourceByIds(request.LinesToCopy);

                foreach (var requestLine in request.LinesToCopy)
                {
                    var sourceDb = sourceDbs.First(x => x.SourceId == requestLine.SourceID);

                    if (requestLine.IsIhs)
                    {
                        if (ihsItemDict.ContainsKey(requestLine.ItemId))//key is the ihs item id, and value is local db item id
                        {
                            sourceDb.ItemId = ihsItemDict[requestLine.ItemId];
                        }
                        else
                        {
                            var itemDb = _itemRepository.CreateIhsItemInDb(requestLine.ItemId);
                            ihsItemDict.Add(requestLine.ItemId, itemDb.ItemID);
                            sourceDb.ItemId = itemDb.ItemID; //change source's item id to local item id
                        }
                    }
                    var poLineReq = new SetPurchaseOrderLineRequest
                    {
                        Qty = requestLine.Quantity,
                        DateCode = sourceDb.DateCode,
                        Cost = (double)sourceDb.Cost,
                        IsDeleted = false,
                        ItemId = requestLine.ItemId,
                        POVersionId = 1,
                        PurchaseOrderId = poDetailDb.PurchaseOrderID,
                        PackagingId = sourceDb.PackagingId,
                        StatusId = 32,
                        PackageConditionID = sourceDb.PackageConditionID
                    };
                    _purchaseOrderRepository.SetPurchaseOrderLine(poLineReq);
                }

                var ownerList = new List<OwnerSetRequest>
               {
                   new OwnerSetRequest
                   {
                       Percentage = 100,
                       UserID = request.BuyerID
                   }
               };
                _ownershipRepository.SetObjectOwnership(new SetOwnershipRequest
                {
                    ObjectID = poDetailDb.PurchaseOrderID,
                    ObjectTypeID = 22,
                    OwnerList = ownerList
                });
                return new PurchaseOrderDetailsSetResponse()
                {
                    IsSuccess = poDetailDb.PurchaseOrderID > 0,
                    PurchaseOrderId = poDetailDb.PurchaseOrderID,
                    VersionId = poDetailDb.VersionID
                };
            }
            catch (Exception e)
            {
                return new PurchaseOrderDetailsSetResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e + "\n" + request,
                    PurchaseOrderId = 0,

                    VersionId = 0
                };
            }
        }

        public SourceHistoryResponse GetSourceLineByAccountId(int accountId,int? contactId=null)
        {
            var sourceLineDb = _sourcingRepository.GetSourceLineByAccountId(accountId, contactId);
            var response = new List<SourceLineResponse>();
            var sourceLineResponse = new SourceHistoryResponse();

            foreach (var value in sourceLineDb)
            {
                response.Add(new SourceLineResponse
                {
                    TypeName= value.TypeName,
                    PartNumber = value.PartNumber,
                    ItemID = value.ItemID,
                    Manufacturer = value.Manufacturer,
                    Qty = value.Qty,
                    Cost = value.Cost,
                    Created= value.Created,
                    DateCode = value.DateCode,
                    Packaging = value.PackagingName,
                    AccountId = value.AccountId,
                    AccountName = value.AccountName,
                    ContactId = value.ContactId,
                    StatusId = value.StatusId,
                    StatusName = value.StatusName,
                    LeadTimeDays= value.LeadTimeDays,
                    ContactName = value.ContactName,
                    CreatedBy = value.CreatedBy                   
                });

            }
            sourceLineResponse.SourceLineList = response;
            return sourceLineResponse;
        }
    }

}
