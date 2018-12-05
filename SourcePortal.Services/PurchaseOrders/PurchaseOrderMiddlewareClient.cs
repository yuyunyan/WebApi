namespace SourcePortal.Services.PurchaseOrders
{
    using Sourceportal.DB.Accounts;
    using Sourceportal.DB.CommonData;
    using Sourceportal.DB.Enum;
    using Sourceportal.DB.Items;
    using Sourceportal.DB.OrderFillment;
    using Sourceportal.DB.PurchaseOrders;
    using Sourceportal.DB.Quotes;
    using Sourceportal.DB.SalesOrders;
    using Sourceportal.DB.User;
    using Sourceportal.Domain.Models.API.Requests;
    using Sourceportal.Domain.Models.API.Responses.Sync;
    using Sourceportal.Domain.Models.DB.Items;
    using Sourceportal.Domain.Models.Middleware;
    using Sourceportal.Domain.Models.Middleware.Enums;
    using Sourceportal.Domain.Models.Middleware.Items;
    using Sourceportal.Domain.Models.Middleware.Owners;
    using Sourceportal.Domain.Models.Middleware.PurchaseOrder;
    using Sourceportal.Utilities;
    using SourcePortal.Services.Shared.Middleware;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PurchaseOrderMiddlewareClient : IPurchaseOrderMiddlewareClient
    {
        IPoSyncRequestCreator _poSyncRequestCreator;
        IMiddlewareService _middlewareService;

        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISyncOwnershipCreator _syncOwnershipCreator;
        private readonly ICommonDataRepository _commonDataRepository;
        private readonly ItemRepository _itemRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IUserRepository _userRepository;        
        private readonly IOrderFillmentRepository _orderFulfillmentRepo;

        public PurchaseOrderMiddlewareClient(IPoSyncRequestCreator poSyncRequestCreator, IMiddlewareService middlewareService, IPurchaseOrderRepository purchaseOrderRepository
            , ISyncOwnershipCreator syncOwnershipCreator, ICommonDataRepository commonDataRepository, ItemRepository itemRepository, IAccountRepository accountRepository,
            IQuoteRepository quoteRepository, IUserRepository userRepository, IOrderFillmentRepository orderFulfillmentRepo)
        {
            _poSyncRequestCreator = poSyncRequestCreator;
            _middlewareService = middlewareService;

            _purchaseOrderRepository = purchaseOrderRepository;
            _syncOwnershipCreator = syncOwnershipCreator;
            _commonDataRepository = commonDataRepository;
            _itemRepository = itemRepository;
            _accountRepository = accountRepository;
            _quoteRepository = quoteRepository;
            _userRepository = userRepository;
            _orderFulfillmentRepo = orderFulfillmentRepo;
        }

        public SyncResponse Sync(int poId, int poVersionId)
        {
            PurchaseOrderSyncRequest syncRequest = CreateRequest(poId, poVersionId);
            return _middlewareService.Sync(syncRequest,"purchaseorders/transactions");
        }

        private PurchaseOrderSyncRequest CreateRequest(int poId, int versionId)
        {
            var purchaseOrderDetails = _purchaseOrderRepository.GetPurchaseOrderDetails(poId, versionId);           
           
            var poSync = new PurchaseOrderSyncRequest(
                poId,
                MiddlewareObjectTypes.PurchaseOrder.ToString(),
                UserHelper.GetUserId(),
                (int)ObjectType.Purchaseorder, purchaseOrderDetails.ExternalId);

            var warehouse = _commonDataRepository.GetWarehouses(null).Where(x => x.WarehouseID == purchaseOrderDetails.ToWarehouseID).FirstOrDefault();

            poSync.VersionId = purchaseOrderDetails.VersionID;
            poSync.IncotermId = _commonDataRepository.GetIncoterm(purchaseOrderDetails.IncotermID) != null ? _commonDataRepository.GetIncoterm(purchaseOrderDetails.IncotermID).ExternalID : null;
            poSync.ToLocationCity = warehouse != null ? _accountRepository.GetLocationDetails(warehouse.LocationID).City : null;
            poSync.OrganizationId = _commonDataRepository.GetOrganization(purchaseOrderDetails.OrganizationID).ExternalID;

            var ownership = _syncOwnershipCreator.Create(poId, ObjectType.Purchaseorder);

            ownership.LeadOwner = ownership.LeadOwner ?? new SyncOwner();
            ownership.SecondOwner = ownership.SecondOwner ?? new SyncOwner();

            poSync.Ownership = new OwnerShip()
            {
                LeadOwner = new Owner()
                {
                    ExternalId = ownership.LeadOwner.ExternalId,
                    Id = ownership.LeadOwner.Id,
                    Name = ownership.LeadOwner.Name,
                    Percentage = ownership.LeadOwner.Percentage
                },
                SecondOwner = new Owner()
                {
                    ExternalId = ownership.SecondOwner.ExternalId,
                    Id = ownership.SecondOwner.Id,
                    Name = ownership.SecondOwner.Name,
                    Percentage = ownership.SecondOwner.Percentage
                }
            };

            poSync.PaymentTermId = _commonDataRepository.GetPaymentTerms(purchaseOrderDetails.PaymentTermID).First().ExternalID;
            poSync.CurrencyId = _commonDataRepository.GetCurrency(purchaseOrderDetails.CurrencyID).ExternalID;
            poSync.OrderDate = purchaseOrderDetails.OrderDate;

            var parsedDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(poSync.OrderDate) && DateTime.TryParse(poSync.OrderDate, out parsedDate))
                poSync.OrderDate = parsedDate.ToString("yyyy-MM-dd");

            poSync.AccountExternalId = _accountRepository.GetAccountBasicDetails(purchaseOrderDetails.AccountID).ExternalId;

            poSync.Lines = PurchaseOrderLineSyncs(poId, versionId);
            poSync.ToLocationExternalId = _orderFulfillmentRepo.GetWarehouseExternalId(purchaseOrderDetails.ToWarehouseID);
            return poSync;
        }

        private List<PurchaseOrderLineSync> PurchaseOrderLineSyncs(int soId, int soVersionId, int? soLineId = null, int? poLineId = null)
        {
            var poLines = _purchaseOrderRepository.GetPurchaseOrderLines(soId, soVersionId, new SearchFilter { RowLimit = 10000 });
            var polineSyncs = new List<PurchaseOrderLineSync>();

            foreach (var poLine in poLines)
            {
                var specBuyForUser = _userRepository.GetUserData(poLine.SpecBuyForUserId);
                var itemDetails = _itemRepository.GetItemDetails(poLine.ItemId);
                var itemSync = SetItemSyncDetails(itemDetails);
                var packageType = _quoteRepository.GetPackagingOption(poLine.PackagingId).FirstOrDefault();
                var packageCondition = _commonDataRepository.GetPackageConditions(poLine.PackageConditionID).FirstOrDefault();

                polineSyncs.Add(new PurchaseOrderLineSync
                {
                    LineNum = poLine.LineNum + "." + poLine.LineRev,
                    Qty = poLine.Qty,
                    Cost = poLine.Cost,
                    PackagingTypeExternalId = packageType != null ? packageType.ExternalId : null,
                    PackageConditionExternalId = packageCondition != null ? packageCondition.ExternalID : null,
                    DateCode = poLine.DateCode,
                    PromisedDate = poLine.PromisedDate?.ToString("yyyy-MM-dd"),
                    DueDate = poLine.DueDate?.ToString("yyyy-MM-dd"),
                    IsSpecBuy = poLine.IsSpecBuy,
                    SpecBuyForUser = specBuyForUser != null ? string.Format("{0} {1}", specBuyForUser.FirstName, specBuyForUser.LastName) : null,
                    SpecBuyForAccount = poLine.SpecBuyForAccountID != 0 ? _accountRepository.GetAccountBasicDetails(poLine.SpecBuyForAccountID).AccountName : null,
                    SpecBuyReason = poLine.SpecBuyReason,
                    ItemDetails = itemSync,
                    ProductSpec = _purchaseOrderRepository.GetProductSpecForPoLine(poLine.POLineId)
                });
            }

            return polineSyncs;
        }

        public ItemSync SetItemSyncDetails(ItemDb itemDetails)
        {
            var itemSync = new ItemSync(itemDetails.ItemID, itemDetails.ExternalID);

            itemSync.Manufacturer = itemDetails.MfrName;
            itemSync.CommodityExternalId = _itemRepository.GetItemCommodityList().Where(x => x.CommodityID == itemDetails.CommodityID).First().ExternalID;
            itemSync.SourceDataId = itemDetails.SourceDataID == null ? 0 : Int64.Parse(itemDetails.SourceDataID);
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
