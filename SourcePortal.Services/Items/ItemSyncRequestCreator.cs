using Sourceportal.DB.CommonData;
using Sourceportal.DB.Items;
using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.Enums;
using Sourceportal.Domain.Models.Middleware.Items;
using Sourceportal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.Enum;

namespace SourcePortal.Services.Items
{
    public class ItemSyncRequestCreator : IItemSyncRequestCreator
    {
        private readonly ICommonDataRepository _commonDataRepository;
        private readonly ItemRepository _itemRepository;

        public ItemSyncRequestCreator(ICommonDataRepository commonDataRepository, ItemRepository itemRepository)
        {
            _commonDataRepository = commonDataRepository;
            _itemRepository = itemRepository;
        }

        public MiddlewareSyncRequest<ItemSync> Create(int itemId)
        {
            var syncRequest = new MiddlewareSyncRequest<ItemSync>(
                itemId, 
                MiddlewareObjectTypes.Material.ToString(), 
                UserHelper.GetUserId(),
                (int)ObjectType.Item
                );
            var itemSync = ItemSync(itemId);
            syncRequest.Data = itemSync;
            return syncRequest;
        }

        private ItemSync ItemSync(int itemId)
        {
            var itemDetails = _itemRepository.GetItemDetails(itemId);
            var itemSync = new ItemSync(itemId, itemDetails.ExternalID);

            itemSync.Manufacturer = itemDetails.MfrName;
            itemSync.CommodityExternalId = _itemRepository.GetItemCommodityList().Where(x => x.CommodityID == itemDetails.CommodityID).First().ExternalID;
            itemSync.SourceDataId = itemDetails.SourceDataID == null ? 0 : Int32.Parse(itemDetails.SourceDataID);
            itemSync.PartNumber = itemDetails.PartNumber;
            itemSync.PartNumberStrip = itemDetails.PartNumberStrip;
            itemSync.Description = itemDetails.PartDescription;
            itemSync.Eurohs = itemDetails.Eurohs;
            itemSync.Eccn = itemDetails.ECCN;
            itemSync.Hts = itemDetails.HTS;
            itemSync.Msl = itemDetails.MSL;
            itemSync.DatasheetUrl = itemDetails.DatasheetURL;

            return itemSync;
        }
    }
}
