using System.Collections.Generic;
using Sourceportal.Domain.Models.DB.Items;
using Sourceportal.Domain.Models.API.Requests.Items;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.API.Responses.Items;

namespace Sourceportal.DB.Items
{
    public interface IItemRepository
    {
        IList<CommodityDb> GetItemCommodityList();
        int GetItemIdFromExternal(string externalId);
        IList<ItemGroupDb> GetItemGroupList();
        IList<ManufacturerDb> GetManufacturerList(string searchString);
        IList<ItemPOsDb> GetItemPOList(ItemPOsListGetRequest itemPOsListRequest);
        IList<ItemExtraDb> GetItemExtraList();
        ItemDb GetItemDetails(int ItemID);
        IList<ItemDb> GetItemList(SearchFilter searchfilter);
        IList<ItemStatus> GetItemStatuses();
        SetItemDetailsDb SetItemDetails(SetItemDetailsRequest itemdetails);
        IList<ItemDb> GetItems();
        ItemDb CreateIhsItemInDb(long itemId);
        void ItemSapDataSet(SetExternalIdRequest request);
        ItemInventory GetItemInventoryByExternalId(string requestInventoryExternalId);
        int GetItemByExternalId(string externalId);
        string GetItemExternalById(int itemId);
        int CreateManufacturer(SetManufacturerRequest model);
        IList<ItemInventoryDb> ItemInventory(int ItemID, bool ExcludePo, int RowOffset, int RowLimit, string SortCol, bool DescSort);
        IList<ItemAvailabilityDb> ItemAvailability(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort);
        IList<ItemSalesOrdersDb> ItemSalesOrders(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort);
        IList<ItemQuotesDb> ItemQuotes(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort);
    }
}
