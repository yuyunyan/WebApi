using System.Collections.Generic;
using System.Linq;
using Sourceportal.DB.Accounts;
using Sourceportal.DB.CommonData;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Items;
using Sourceportal.DB.SalesOrders;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.Enums;
using Sourceportal.Domain.Models.Middleware.SalesOrder;
using SourcePortal.Services.Shared.Middleware;
using Sourceportal.Domain.Models.DB.Items;
using Sourceportal.Domain.Models.Middleware.Items;
using System;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.DB.SalesOrders;
using SourcePortal.Services.OrderFulfillment;

namespace SourcePortal.Services.SalesOrder
{
    public class SoSyncRequestCreator : ISoSyncRequestCreator
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISyncOwnershipCreator _syncOwnershipCreator;
        private readonly ICommonDataRepository _commonDataRepository;
        private readonly ItemRepository _itemRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IOrderFulfillmentService _orderFulfillmentService;

        public SoSyncRequestCreator(ISalesOrderRepository salesOrderRepository, ISyncOwnershipCreator syncOwnershipCreator, 
            ICommonDataRepository commonDataRepository, ItemRepository itemRepository, IAccountRepository accountRepository, IOrderFulfillmentService orderFulfillmentService)
        {
            _salesOrderRepository = salesOrderRepository;
            _syncOwnershipCreator = syncOwnershipCreator;
            _commonDataRepository = commonDataRepository;
            _itemRepository = itemRepository;
            _accountRepository = accountRepository;
            _orderFulfillmentService = orderFulfillmentService;
        }
        public MiddlewareSyncRequest<SalesOrderSync> Create(int soId, int versionId)
        {
            var syncRequest = new MiddlewareSyncRequest<SalesOrderSync>(
                soId, 
                MiddlewareObjectTypes.SalesOrder.ToString(), 
                UserHelper.GetUserId(),
                (int)ObjectType.Salesorder);
            var soSync = SalesOrderSync(soId, versionId);
            syncRequest.Data = soSync;
            return syncRequest;
        }

        private SalesOrderSync SalesOrderSync(int soId, int versionId)
        {
            var salesOrderDetails = _salesOrderRepository.GetSalesOrderDetails(soId, versionId);
            var soSync = new SalesOrderSync(salesOrderDetails.SalesOrderId, salesOrderDetails.ExternalId);
            
            soSync.VersionId = salesOrderDetails.VersionId;
            soSync.CustomerPo = salesOrderDetails.CustomerPo;
            soSync.IncoTermExternalId = _commonDataRepository.GetIncoterm(salesOrderDetails.IncotermId).ExternalID;
            soSync.OrgExternalId = _commonDataRepository.GetOrganization(salesOrderDetails.OrganizationId).ExternalID;

            soSync.Ownership = _syncOwnershipCreator.Create(soId, ObjectType.Salesorder);

            soSync.PaymentTermExternalId = _commonDataRepository.GetPaymentTerms(salesOrderDetails.PaymentTermId).First().ExternalID;
            soSync.CurrencyExternalId = _commonDataRepository.GetCurrency(salesOrderDetails.CurrencyId).ExternalID;
            soSync.OrderDate = salesOrderDetails.OrderDate.ToShortDateString();
            soSync.AccountExternalId = _accountRepository.GetAccountBasicDetails(salesOrderDetails.AccountId).ExternalId;
            soSync.ContactExternalId = _accountRepository.GetContactDetails(salesOrderDetails.ContactId).ExternalId;

            soSync.UltDestinationId = salesOrderDetails.UltDestinationId > 0 ? _commonDataRepository.GetCountry(salesOrderDetails.UltDestinationId).First().CountryName : null;
            soSync.FreightAccount = salesOrderDetails.FreightAccount;
            soSync.IncotermLocation = _commonDataRepository.GetIncoterm(salesOrderDetails.IncotermId).ExternalID;

            soSync.Lines = SalesOrderLineSyncs(salesOrderDetails);
            return soSync;
        }
        
        private List<SalesOrderLineSync> SalesOrderLineSyncs(SalesOrderDetailsDb salesOrderDetails)
        {
            var soLines = _salesOrderRepository.GetSalesOrderLines(salesOrderDetails.SalesOrderId, salesOrderDetails.VersionId, new SearchFilter { RowLimit = 10000 });

            var solineSyncs = new List<SalesOrderLineSync>();
            
            foreach (var soLine in soLines)
            {
                var itemDetails = _itemRepository.GetItemDetails(soLine.ItemId);
                var itemSync = SetItemSyncDetails(itemDetails);
                var sosWarehouse = _orderFulfillmentService.GetWarehouseSoSDetails(soLine.SOLineId);
                var sourceOfSupply = _orderFulfillmentService.GetSourceOfSupply(salesOrderDetails, soLine.SOLineId, sosWarehouse);

                solineSyncs.Add(new SalesOrderLineSync
                {
                    SOLineId = soLine.SOLineId,
                    CustomerLine = soLine.CustomerLine,
                    DueDate = soLine.DueDate.ToShortDateString(),
                    ItemExternalId = _itemRepository.GetItemDetails(soLine.ItemId).ExternalID,
                    LineNum = soLine.LineNum,
                    Price = soLine.Price,
                    Quantity = soLine.Qty,
                    CustomerPartNum = soLine.CustomerPartNum,
                    ShipDate = soLine.ShipDate.ToShortDateString(),
                    Cost = soLine.Cost,
                    DateCode = soLine.DateCode,
                    PackagingId = _commonDataRepository.GetPackagingOptions(soLine.PackagingId).First().ExternalId,
                    PackageConditionId = soLine.PackageConditionID > 0 ? _commonDataRepository.GetPackageConditions(soLine.PackageConditionID).First().ExternalID : null, 
                    ProductSpec = soLine.ProductSpec,
                    ItemDetails = itemSync,
                    SourceOfSupply = sourceOfSupply
                });
            }

            return solineSyncs;

        }

        public ItemSync SetItemSyncDetails(ItemDb itemDetails)
        {
            var itemSync = new ItemSync(itemDetails.ItemID, itemDetails.ExternalID);

            itemSync.Manufacturer = itemDetails.MfrName;
            itemSync.CommodityExternalId = _itemRepository.GetItemCommodityList().Where(x => x.CommodityID == itemDetails.CommodityID).First().ExternalID;
            itemSync.SourceDataId = itemDetails.SourceDataID == null ? 0 : Double.Parse(itemDetails.SourceDataID);
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
