using Sourceportal.DB.Items;
using Sourceportal.Domain.Models.API.Requests.Items;
using Sourceportal.Domain.Models.API.Responses.Items;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.Shared;
using Sourceportal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.API.Responses.Sync;
using SourcePortal.Services.Shared.Middleware;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Sourceportal.Domain.Models.DB.Items;
using SourcePortal.Services.BOMs;

namespace SourcePortal.Services.Items
{
    public class ItemService : IItemService
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;
        private readonly ItemRepository _itemRepository;
        private readonly IItemSyncRequestCreator _itemSyncRequestCreator;
        private readonly IMiddlewareService _middlewareService;
        private readonly IItemsIhsRepository _itemsIhsRepository;
        private readonly IBOMsService _bOMsService;



        public ItemService(ItemRepository itemRepository, IItemsIhsRepository itemsIhsRepository, IItemSyncRequestCreator itemSyncRequestCreator, IMiddlewareService middlewareService, IBOMsService bOMsService)
        {
            _itemRepository = itemRepository;
            _itemsIhsRepository = itemsIhsRepository;
            _itemSyncRequestCreator = itemSyncRequestCreator;
            _middlewareService = middlewareService;
            _bOMsService = bOMsService;
        }

        public int GetItemIdFromExternal(string externalId)
        {
            return _itemRepository.GetItemIdFromExternal(externalId);
        }

        public ItemCommodityListResponse GetItemCommodityList()
        {
            var coms = _itemRepository.GetItemCommodityList();
            var list = new List<ItemCommodityResponse>();

            foreach (var com in coms)
            {
                list.Add(new ItemCommodityResponse
                {
                    CommodityID = com.CommodityID,
                    CommodityName = com.CommodityName,
                    ItemGroupID = com.ItemGroupID,
                });
            }
            return new ItemCommodityListResponse { Commodities = list };
        }

        public ItemGroupListResponse GetItemGroupList()
        {
            var groups = _itemRepository.GetItemGroupList();
            var list = new List<ItemGroupResponse>();

            foreach (var group in groups)
            {
                list.Add(new ItemGroupResponse
                {
                    ItemGroupID = group.ItemGroupID,
                    GroupName = group.GroupName,
                    Code = group.Code
                });
            }
            return new ItemGroupListResponse { ItemGroups = list };
        }

        public ManufacturerListResponse GetManufacturerList(string searchText)
        {
            var dbMfrs = _itemRepository.GetManufacturerList(searchText);
            var list = new List<ManufacturerResponse>();

            foreach (var dbMfr in dbMfrs)
            {
                list.Add(new ManufacturerResponse
                {
                    MfrId = dbMfr.MfrID,
                    MfrName = dbMfr.MfrName,
                    Code = dbMfr.Code,
                    MfrURL = dbMfr.MfrURL
                });
            }

            return new ManufacturerListResponse { Manufacturers = list };
        }

        public ItemExtraListResponse GetItemExtraList()
        {
            var itemExtras = _itemRepository.GetItemExtraList();
            var list = new List<ItemExtraResponse>();

            foreach (var itemExtra in itemExtras)
            {
                list.Add(new ItemExtraResponse
                {
                    ItemExtraId = itemExtra.ItemExtraId,
                    ExtraDescription = itemExtra.ExtraDescription,
                    ExtraName = itemExtra.ExtraName
                });
            }

            return new ItemExtraListResponse { ItemExtras = list };

        }

        public ItemResponse GetItemDetails(int ItemID)
        {
            var dbItem = _itemRepository.GetItemDetails(ItemID);
            ItemResponse details = new ItemResponse
            {
                ItemID = dbItem.ItemID,
                ManufacturerID = dbItem.MfrID,
                ManufacturerName = dbItem.MfrName,
                ManufacturerPartNumber = dbItem.PartNumber,
                CommodityName = dbItem.CommodityName,
                CommodityID = dbItem.CommodityID,
                GroupName = dbItem.GroupName,
                GroupID = dbItem.ItemGroupID,
                Eccn = dbItem.ECCN,
                Eurohs = dbItem.Eurohs,
                Cnrohs = dbItem.Cnrohs,
                Status = dbItem.Status,
                StatusID = dbItem.ItemStatusID,
                Description = dbItem.PartDescription,
                HTS = dbItem.HTS,
                MSL = dbItem.MSL,
                Weight = dbItem.WeightG,
                Length = dbItem.LengthCM,
                Width = dbItem.WidthCM,
                Depth = dbItem.DepthCM
            };

            return details;
        }

        public ItemPurchaseOrdersListResponse GetItemPurchaseOrders(ItemPOsListGetRequest itemPOsListGetRequest)
        {
            var dbPOs = _itemRepository.GetItemPOList(itemPOsListGetRequest);
            var list = new List<ItemPurchaseOrderResponse>();
            int TotalCount = 0;

            foreach (var dbPO in dbPOs)
            {
                list.Add(new ItemPurchaseOrderResponse
                {
                    PurchaseOrderID = dbPO.PurchaseOrderID,
                    VersionID = dbPO.VersionID,
                    POExternalID = dbPO.POExternalID,
                    AccountID = dbPO.AccountID,
                    AccountName = dbPO.AccountName,
                    ContactID = dbPO.ContactID,
                    FirstName = dbPO.FirstName,
                    LastName = dbPO.LastName,
                    OrganizationID = dbPO.OrganizationID,
                    OrgName = dbPO.OrgName,
                    StatusID = dbPO.StatusID,
                    StatusName = dbPO.StatusName,
                    OrderDate = dbPO.OrderDate,
                    DueDate = dbPO.DueDate,
                    Qty = dbPO.Qty,
                    Cost = dbPO.Cost,
                    DateCode = dbPO.DateCode,
                    PackagingID = dbPO.PackagingID,
                    PackagingName = dbPO.PackagingName,
                    PackageConditionID = dbPO.PackageConditionID,
                    ConditionName = dbPO.ConditionName,
                    Owners = dbPO.Owners,
                    WarehouseID = dbPO.WarehouseID,
                    WarehouseName = dbPO.WarehouseName
                });
            }
            if (dbPOs.Count() > 0)
                TotalCount = dbPOs[0].TotalRowCount;
            return new ItemPurchaseOrdersListResponse { ItemPOs = list, TotalRowCount = TotalCount };
        }

        public ItemListResponse GetItemList(SearchFilter searchfilter)
        {
            var dbItems = _itemRepository.GetItemList(searchfilter);
            var list = new List<ItemResponse>();
            int TotalCount = 0;
            foreach (var dbItem in dbItems)
            {
                list.Add(new ItemResponse
                {
                    ItemID = dbItem.ItemID,
                    ManufacturerName = dbItem.MfrName,
                    ManufacturerPartNumber = dbItem.PartNumber,
                    CommodityID = dbItem.CommodityID,
                    CommodityName = dbItem.CommodityName,
                    Status = dbItem.Status,
                    Description = dbItem.PartDescription,
                });
            }
            if (dbItems.Count() > 0)
                TotalCount = dbItems[0].TotalRows;
            return new ItemListResponse { Items = list, TotalRowCount = TotalCount };
        }

        public IList<ItemStatus> GetItemStatuses()
        {
            return _itemRepository.GetItemStatuses();
        }

        public void DeleteItem(int itemId)
        {
            _itemRepository.DeleteItem(itemId);
        }

        public SetItemDetailsResponse SetItemDetails(SetItemDetailsRequest itemdetails)
        {
            var dbSetItemDetails = _itemRepository.SetItemDetails(itemdetails);
            var response = new SetItemDetailsResponse();

            response.ItemID = dbSetItemDetails.ItemID;
            return response;
        }

        public ItemsResponse GetItems()
        {
            var dbGetItems = _itemRepository.GetItems();
            var list = new List<Sourceportal.Domain.Models.API.Responses.Items.Items>();
            foreach (var item in dbGetItems)
            {
                list.Add(new Sourceportal.Domain.Models.API.Responses.Items.Items
                {
                    ItemId = item.ItemID,
                    PartNumber = item.PartNumber
                });
            }
            return new ItemsResponse { Items = list };

        }

        public PartSearchResponse GetSuggestions(string searchString)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            searchString = rgx.Replace(searchString, "");

            var dbSuggestions = new List<Suggestion>();
            var ihsSuggestion = new List<Suggestion>();

            Parallel.Invoke(
                () => dbSuggestions = GetDbSuggestions(searchString),
                () => ihsSuggestion = GetIhsSuggestions(searchString)
            );
            var dbSuggestionIdList = dbSuggestions.Select(x => x.SourceDataId);
            var ihsItemsInLocalDb = ihsSuggestion.Where(x => dbSuggestionIdList.Contains(x.Id.ToString())).ToList();
            ihsSuggestion = ihsSuggestion.Except(ihsItemsInLocalDb).ToList();

            dbSuggestions.AddRange(ihsSuggestion);
            return new PartSearchResponse { Suggestions = dbSuggestions };
        }

        private List<Suggestion> GetDbSuggestions(string searchString)
        {
            var dbItems = _itemRepository.GetItemList(new SearchFilter { SearchString = searchString, RowLimit = 10, RowOffset = 0 });

            var itemsList = new List<Suggestion>();

            foreach (var dbItem in dbItems)
            {
                itemsList.Add(new Suggestion
                {
                    Id = dbItem.ItemID,
                    Name = dbItem.PartNumber,
                    Commodity = dbItem.CommodityName,
                    Manufacturer = dbItem.MfrName,
                    SourceDataId = dbItem.SourceDataID,
                    IsIhs = false,
                    Data = string.Format("{0} {1}", dbItem.PartNumber, "{" + dbItem.MfrName + "}")
                });
            }
            return itemsList;
        }

        private List<Suggestion> GetIhsSuggestions(string searchString)
        {
            var items = _itemsIhsRepository.GetItems(searchString, 10);

            var itemsList = new List<Suggestion>();

            foreach (var dbItem in items)
            {
                var middleLevelCategory = GetMiddleLevelCategory(dbItem.AbstractProduct.Categories.Names);
                if (middleLevelCategory == null)
                {
                    continue;
                }

                itemsList.Add(new Suggestion
                {
                    Id = long.Parse(dbItem.AbstractProduct.Id),
                    Name = dbItem.AbstractProduct.Part,
                    Commodity = middleLevelCategory,
                    Manufacturer = dbItem.AbstractProduct.Mfr.Name,
                    IsIhs = true,
                    Data = string.Format("{0} {1} {2}", dbItem.AbstractProduct.Part, " {" + dbItem.AbstractProduct.Mfr.Name + "}", " IHS")
                });
            }
            return itemsList;
        }

        private static string GetMiddleLevelCategory(IEnumerable<string> list)
        {
            var categories = list.ToList();
            return categories.Count > 1 ? categories[1] : null;
        }

        public void SetItemExternalId(SetExternalIdRequest request)
        {
            _itemRepository.ItemSapDataSet(request);
        }

        public SyncResponse Sync(int itemId)
        {
            var syncRequest = _itemSyncRequestCreator.Create(itemId);
            return _middlewareService.Sync(syncRequest);
        }

        public int CreateManufacturer(SetManufacturerRequest request)
        {
            return _itemRepository.CreateManufacturer(request);
        }

        public ItemInventoryResponse GetItemInventory(int ItemID, bool ExcludePo, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            var inv = _itemRepository.ItemInventory(ItemID, ExcludePo, RowOffset, RowLimit, SortCol, DescSort);
            var invList = new List<ItemInventoryDetails>();
            foreach (var item in inv)
            {
                invList.Add(new ItemInventoryDetails
                {
                    WarehouseID = item.WarehouseID,
                    WarehouseName = item.WarehouseName,
                    AccountID = item.AccountID,
                    AccountName = item.AccountName,
                    OrigQty = item.OrigQty,
                    AvailableQty = item.Available,
                    PurchaseOrderID = item.PurchaseOrderID,
                    POVersionID = item.POVersionID,
                    POExternalID = item.POExternalID,
                    Buyer = item.Buyers,
                    StatusID = item.StatusID,
                    StatusName = item.StatusName,
                    Cost = item.Cost,
                    Allocated = _bOMsService.MapAllocationJsonToObject(item.Allocations),
                    DateCode = item.DateCode,
                    PackagingName = item.PackagingName,
                    PackageCondition = item.ConditionName,
                    ShipDate = item.ShipDate
                });
            }
            return new ItemInventoryResponse()
            {
                Inventory = invList,
                TotalRowCount = inv.Count() > 0 ? inv[0].TotalRows : 0
            };
        }

        public ItemAvailabilityResponse GetItemAvailability(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            var inv = _itemRepository.ItemAvailability(ItemID, RowOffset, RowLimit, SortCol, DescSort);
            var invList = new List<ItemAvailabilityDetails>();
            foreach (var item in inv)
            {
                invList.Add(new ItemAvailabilityDetails
                {
                    SourceID = item.SourceId,
                    AccountID = item.AccountId,
                    AccountName = item.AccountName,
                    Created = item.Created,
                    CreatedBy = item.CreatedBy,
                    DateCode = item.DateCode,
                    Quantity = item.Qty,
                    Rating = item.Rating,
                    RtpQty = item.RtpQty,
                    Cost = item.Cost,
                    PackagingID = item.PackagingID,
                    PackagingName = item.PackagingName,
                    MOQ = item.MOQ,
                    LeadTime = item.LeadTimeDays,
                    TypeName = item.TypeName,
                    ExternalID = item.ExternalID
                });
            }
            return new ItemAvailabilityResponse() { ItemAvailbility = invList,
                TotalRowCount = inv.Count() > 0 ? inv[0].TotalRows : 0
            };
        }

         public ItemTechnicalDataResponse GetTechnicalData(int itemId)
        {
            var item = _itemRepository.GetItemDetails(itemId);
            var response = new ItemTechnicalDataResponse { TechnicalData = new List<TechnicalDataObject>() };

            if (!string.IsNullOrEmpty(item.SourceDataID))
            {
                var elasticResults = _itemsIhsRepository.SearchItem(item.PartNumber);

                var matchingEsItems = elasticResults.Where(x => x.AbstractProduct.Id == item.SourceDataID);

                if (matchingEsItems.Count() > 0 && matchingEsItems.First().AbstractProduct.TechnicalData != null)
                {
                    foreach (var technicalDataItem in matchingEsItems.First().AbstractProduct.TechnicalData)
                    {
                        response.TechnicalData.Add(new TechnicalDataObject
                        {
                            Key = technicalDataItem.Key,
                            Value = technicalDataItem.Value
                        });
                    }
                }
            }

            return response;
        }
        
        public ItemSalesOrdersResponse GetItemSalesOrders(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            var inv = _itemRepository.ItemSalesOrders(ItemID, RowOffset, RowLimit, SortCol, DescSort);
            var invList = new List<ItemSalesOrdersDetails>();
            foreach (var item in inv)
            {
                invList.Add(new ItemSalesOrdersDetails
                {
                    SOLineID = item.SOLineID,
                    SalesOrderID = item.SalesOrderID,
                    VersionID = item.VersionID,
                    AccountID = item.AccountID,
                    AccountName = item.AccountName,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    ContactID = item.ContactID,
                    OrgName = item.OrgName,
                    OrderDate = item.OrderDate,
                    ShipDate = item.ShipDate,
                    DateCode = item.DateCode,
                    Quantity = item.Qty,
                    Price = item.Price,
                    Cost = item.Cost,
                    PackagingID = item.PackagingID,
                    PackagingName = item.PackagingName,
                    PackageConditionID = item.PackageConditionID,
                    ConditionName = item.ConditionName,
                    Owners = item.Owners,
                    SOExternalID = item.SOExternalID
                });
            }
            return new ItemSalesOrdersResponse() { ItemSalesOrders = invList, TotalRowCount = inv.Count() > 0 ? inv[0].TotalRowCount : 0 };
        }

        public ItemQuotesResponse GetItemQuotes(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            var inv = _itemRepository.ItemQuotes(ItemID, RowOffset, RowLimit, SortCol, DescSort);
            var invList = new List<ItemQuotesDetails>();
            foreach (var item in inv)
            {
                invList.Add(new ItemQuotesDetails
                {
                    QuoteID = item.QuoteID,
                    QuoteLineID = item.QuoteLineID,
                    VersionID = item.VersionID,
                    AccountID = item.AccountID,
                    AccountName = item.AccountName,
                    Created = item.CreatedDate,
                    DateCode = item.DateCode,
                    ContactID = item.ContactID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    OrganizationID = item.OrganizationID,
                    OrgName = item.OrgName,
                    StatusID = item.StatusID,
                    StatusName = item.StatusName,
                    SentDate = item.SentDate,
                    Quantity = item.Qty,
                    Cost = item.Cost,
                    Price = item.Price,
                    PackagingID = item.PackagingID,
                    PackagingName = item.PackagingName,
                    GPM = item.GPM,
                    Owners = item.Owners
                });
            }
            return new ItemQuotesResponse() { ItemQuotes = invList, TotalRowCount = inv.Count() > 0 ? inv[0].TotalRowCount : 0 };
        }

    }

    
}
