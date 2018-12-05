namespace SourcePortal.Services.SalesOrder
{
    using Sourceportal.DB.Accounts;
    using Sourceportal.DB.CommonData;
    using Sourceportal.DB.Enum;
    using Sourceportal.DB.Items;
    using Sourceportal.DB.SalesOrders;
    using Sourceportal.DB.User;
    using Sourceportal.Domain.Models.API.Requests;
    using Sourceportal.Domain.Models.API.Responses.Sync;
    using Sourceportal.Domain.Models.DB.SalesOrders;
    using Sourceportal.Domain.Models.Middleware;
    using Sourceportal.Domain.Models.Middleware.Enums;
    using Sourceportal.Domain.Models.Middleware.SalesOrder;
    using Sourceportal.Utilities;
    using SourcePortal.Services.OrderFulfillment;
    using SourcePortal.Services.Shared.Middleware;
    using System.Collections.Generic;
    using System.Linq;

    public class SalesOrderMiddlewareClient : ISalesOrderMiddlewareClient
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISyncOwnershipCreator _syncOwnershipCreator;
        private readonly ICommonDataRepository _commonDataRepository;
        private readonly ItemRepository _itemRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IOrderFulfillmentService _orderFulfillmentService;
        private readonly IMiddlewareService _middlewareService;
        

        public SalesOrderMiddlewareClient(ISalesOrderRepository salesOrderRepository, ISyncOwnershipCreator syncOwnershipCreator,
            ICommonDataRepository commonDataRepository, ItemRepository itemRepository, IAccountRepository accountRepository, IOrderFulfillmentService orderFulfillmentService,
            ISoSyncRequestCreator requestCreator, IMiddlewareService middlewareService)
        {
            _salesOrderRepository = salesOrderRepository;
            _syncOwnershipCreator = syncOwnershipCreator;
            _commonDataRepository = commonDataRepository;
            _itemRepository = itemRepository;
            _accountRepository = accountRepository;
            _orderFulfillmentService = orderFulfillmentService;
            _middlewareService = middlewareService;
            
        }

        private SalesOrderSyncRequest CreateRequest(int soId, int versionId)
        {
            var salesOrderDetails = _salesOrderRepository.GetSalesOrderDetails(soId, versionId);

            var request = new SalesOrderSyncRequest(soId,
               MiddlewareObjectTypes.SalesOrder.ToString(),
               UserHelper.GetUserId(),
               (int)ObjectType.Salesorder,
               salesOrderDetails.ExternalId);

            request.AccountExternalId = _accountRepository.GetAccountBasicDetails(salesOrderDetails.AccountId).ExternalId;
            request.ContactExternalId = _accountRepository.GetContactDetails(salesOrderDetails.ContactId).ExternalId;
            request.CurrencyExternalId = _commonDataRepository.GetCurrency(salesOrderDetails.CurrencyId).ExternalID;
            request.CustomerPo = salesOrderDetails.CustomerPo;
            request.FreightAccount = salesOrderDetails.FreightAccount;
            request.VersionId = salesOrderDetails.VersionId;
            request.OrderDate = salesOrderDetails.OrderDate.ToString("yyyy-MM-dd");

            var Ownership = _syncOwnershipCreator.Create(soId, ObjectType.Salesorder);
            Ownership = Ownership ?? new Sourceportal.Domain.Models.Middleware.Owners.SyncOwnership();

            Ownership.LeadOwner = Ownership.LeadOwner ?? new Sourceportal.Domain.Models.Middleware.Owners.SyncOwner();
            Ownership.SecondOwner = Ownership.SecondOwner ?? new Sourceportal.Domain.Models.Middleware.Owners.SyncOwner();

            request.Ownership = new OwnerShip()
            {
                LeadOwner = new Owner()
                {
                    ExternalId = Ownership.LeadOwner.ExternalId,
                    Id = Ownership.LeadOwner.Id,
                    Name = Ownership.LeadOwner.Name,
                    Percentage = Ownership.LeadOwner.Percentage
                },
                SecondOwner = new Owner()
                {
                    ExternalId = Ownership.SecondOwner.ExternalId,
                    Id = Ownership.SecondOwner.Id,
                    Name = Ownership.SecondOwner.Name,
                    Percentage = Ownership.SecondOwner.Percentage
                }
            };

            var incoterm = _commonDataRepository.GetIncoterm(salesOrderDetails.IncotermId);

            request.IncoTermExternalId = incoterm.ExternalID;
            request.IncotermLocation = incoterm.ExternalID;

            request.Lines = GetSalesOrderLinesToSync(salesOrderDetails);
            
            request.OrgExternalId = _commonDataRepository.GetOrganization(salesOrderDetails.OrganizationId).ExternalID;
            request.PaymentTermExternalId = _commonDataRepository.GetPaymentTerms(salesOrderDetails.PaymentTermId).First().ExternalID;
            request.UltDestinationId = salesOrderDetails.UltDestinationId > 0 ? _commonDataRepository.GetCountry(salesOrderDetails.UltDestinationId).First().CountryName : null;

            return request;

        }
      
        public SyncResponse Sync(int soId, int versionId)
        {
            var request = CreateRequest(soId, versionId);
            return _middlewareService.Sync(request , "api/saleorders/transactions");
        }

        protected List<Line> GetSalesOrderLinesToSync(SalesOrderDetailsDb salesOrderDetails)
        {
            var linesFromDB = _salesOrderRepository.GetSalesOrderLines(salesOrderDetails.SalesOrderId, salesOrderDetails.VersionId, new SearchFilter { RowLimit = 10000 });            

            var linesToSync = new List<Line>();

            for (int i = 0; i < linesFromDB.Count; i++)
            {
                var lineFromDB = linesFromDB[i];
                var sosWarehouse = _orderFulfillmentService.GetWarehouseSoSDetails(lineFromDB.SOLineId);
                var sourceOfSupply = _orderFulfillmentService.GetSourceOfSupply(salesOrderDetails, lineFromDB.SOLineId, sosWarehouse);

                var lineToAdd = new Line()
                {
                    Cost = lineFromDB.Cost,
                    CustomerLine = lineFromDB.CustomerLine,
                    CustomerPartNum = lineFromDB.CustomerPartNum,
                    DateCode = lineFromDB.DateCode,
                    DueDate = lineFromDB.DueDate.ToString("yyyy-MM-dd"),
                    ItemDetails = GetItemDetailByItemId(lineFromDB.ItemId),
                    LineNum = lineFromDB.LineNum,
                    PackageConditionId = lineFromDB.PackageConditionID > 0 ? _commonDataRepository.GetPackageConditions(lineFromDB.PackageConditionID).First().ExternalID : null,
                    PackagingId = _commonDataRepository.GetPackagingOptions(lineFromDB.PackagingId).First().ExternalId,
                    Price = lineFromDB.Price,
                    ProductSpec = lineFromDB.ProductSpec,
                    Quantity = lineFromDB.Qty,
                    ShipDate = lineFromDB.ShipDate.ToString("yyyy-MM-dd"),
                    SoLineId = lineFromDB.SOLineId,
                    SourceOfSupply=sourceOfSupply
                };

                linesToSync.Add(lineToAdd);

            }

            return linesToSync;

        }

        protected ItemDetails GetItemDetailByItemId(int ItemId)
        {
            var itemDetails = _itemRepository.GetItemDetails(ItemId);

            var detail = new ItemDetails()
            {
                CommodityExternalId = itemDetails.ExternalID,
                DatasheetUrl = itemDetails.DatasheetURL,
                Description = itemDetails.PartDescription, //// is it OK?
                Eccn = itemDetails.ECCN,
                Eurohs = itemDetails.Eurohs,
                ExternalId = itemDetails.ExternalID,
                Hts = itemDetails.HTS,
                Id = itemDetails.ItemID,
                Msl=itemDetails.MSL,
                PartNumber=itemDetails.PartNumber,
                PartNumberStrip=itemDetails.PartNumberStrip,
                SourceDataId=itemDetails.SourceDataID
            };

            return detail;
        }
    }
}
