using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses.SalesOrders;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sourceportal.DB.Enum;
using Sourceportal.DB.SalesOrders;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.DB.Quotes;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Domain.Models.DB.SalesOrders;
using Sourceportal.Domain.Models.Middleware.Enums;
using SourcePortal.Services.Shared.Middleware;
using SourcePortal.Services.Items;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.DB.Accounts;
using Sourceportal.DB.CommonData;
using System;
using SourcePortal.Services.Ownership;
using Sourceportal.DB.User;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.Middleware.SalesOrder;
using Sourceportal.Utilities;

namespace SourcePortal.Services.SalesOrder
{
    public class SalesOrderService: ISalesOrderService
    {
        
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IQuoteRepository _quoteRepository;        
        private readonly IItemService _itemService;
        private readonly IAccountRepository _accountRepository;
        private readonly ICommonDataRepository _commonDataRepo;
        private readonly IOwnershipService _ownershipService;
        private readonly IUserRepository _userRepo;
        private readonly ISalesOrderMiddlewareClient _salesOrderMiddlewareClient;

        public SalesOrderService(ISalesOrderRepository salesOrderRepository, IQuoteRepository quoteRepository, 
            ISoSyncRequestCreator soSyncRequestCreator, IItemService itemService, IAccountRepository accountRepo, ICommonDataRepository commonRepo, IOwnershipService ownershipService,
            IUserRepository userRepo, ISalesOrderMiddlewareClient salesOrderMiddlewareClient)
        {
            _itemService = itemService;
            _salesOrderRepository = salesOrderRepository;
            _quoteRepository = quoteRepository;            
            _accountRepository = accountRepo;
            _commonDataRepo = commonRepo;
            _ownershipService = ownershipService;
            _userRepo = userRepo;
            _salesOrderMiddlewareClient = salesOrderMiddlewareClient;
        }

        public SalesOrderDetailsResponse SetSalesOrderDetails(SalesOrderDetailsRequest request)
        {
            var dbSalesOrderDetails = _salesOrderRepository.SetSalesOrderDetails(request);
            return CreateSalesOrderResponseFromDb(dbSalesOrderDetails);
        }

        private SalesOrderDetailsResponse CreateSalesOrderResponseFromDb(SalesOrderDetailsDb dbSalesOrderDetails)
        {
            SalesOrderDetailsResponse response = new SalesOrderDetailsResponse();

            response.SalesOrderId = dbSalesOrderDetails.SalesOrderId;
            response.VersionId = dbSalesOrderDetails.VersionId;
            response.AccountId = dbSalesOrderDetails.AccountId;
            response.ContactId = dbSalesOrderDetails.ContactId;
            response.ProjectId = dbSalesOrderDetails.ProjectId;
            response.IncotermId = dbSalesOrderDetails.IncotermId;
            response.PaymentTermId = dbSalesOrderDetails.PaymentTermId;
            response.CurrencyId = dbSalesOrderDetails.CurrencyId;
            response.ShipLocationId = dbSalesOrderDetails.ShipLocationId;
            response.ShippingMethodId = dbSalesOrderDetails.ShippingMethodId;
            response.OrganizationId = dbSalesOrderDetails.OrganizationId;
            response.UltDestinationId = dbSalesOrderDetails.UltDestinationId;
            response.FreightPaymentId = dbSalesOrderDetails.FreightPaymentId;
            response.FreightAccount = dbSalesOrderDetails.FreightAccount;
            response.OrderDate = dbSalesOrderDetails.OrderDate.ToString("d", DateTimeFormatInfo.InvariantInfo);
            response.CustomerPo = dbSalesOrderDetails.CustomerPo;
            response.CarrierId = dbSalesOrderDetails.CarrierId;
            response.CarrierMethodId = dbSalesOrderDetails.CarrierMethodId;

            return response;
        }

        private SalesOrderDetailsRequest CreateSalesOrderDetailsRequest(SalesOrderIncomingSapResponse sapSalesorder, int? soId = null, int? versionId = null)
        {
            SalesOrderDetailsRequest request = new SalesOrderDetailsRequest();

            request.SalesOrderId = soId != null ? (int)soId : 0;
            request.VersionID = versionId != null ? (int)versionId : 1;
            request.ExternalId = sapSalesorder.ExternalId;
            request.AccountID = _accountRepository.GetAccountIdByExternal(sapSalesorder.AccountExternalId);
            request.ContactID = _accountRepository.GetContactIdByExternal(sapSalesorder.ContactExternalId);
            request.OrganizationID = sapSalesorder.OrgExternalId != null ? _commonDataRepo.GetAllOrganizations().First(x => x.ExternalID == sapSalesorder.OrgExternalId).OrganizationID : 0;
            request.ShipLocationID = sapSalesorder.ShipLocationExternalId != null ? _accountRepository.GetLocationIdByExternal(sapSalesorder.ShipLocationExternalId) : 0;
            request.CustomerPo = sapSalesorder.CustomerPo;
            request.IncotermID = sapSalesorder.IncotermExternalId != null ? _commonDataRepo.GetAllIncoterms().First(x => x.ExternalID == sapSalesorder.IncotermExternalId).IncotermID : 0;
            request.UltDestinationID = sapSalesorder.UltDestinationExternalId != null ? _commonDataRepo.GetCountryByExternal(sapSalesorder.UltDestinationExternalId) : 0;
            request.PaymentTermID = sapSalesorder.PaymentTermExternalId != null ? _commonDataRepo.GetPaymentTermIdByExternal(sapSalesorder.PaymentTermExternalId) : 0;
            request.CurrencyID = sapSalesorder.CurrencyExternalId != null ? _commonDataRepo.GetCurrencyIdByExternal(sapSalesorder.CurrencyExternalId) : null;
            //request.FreightAccount = sapSalesorder.FreightAccount;
            request.OrderDate = sapSalesorder.OrderDate.ToString("d", DateTimeFormatInfo.InvariantInfo);
            
            return request;
        }

        public void SetSalesOrderDetailsFromSap(SalesOrderDetailsRequest request)
        {
            var dbSalesOrderDetails = _salesOrderRepository.SetSalesOrderDetails(request);
            SetSalesOrderLinesSapData(request);
        }

        public BaseResponse HandlingIncomingSalesOrderSapUpdate(SalesOrderIncomingSapResponse request)
        {
            SalesOrderDetailsRequest salesOrder = new SalesOrderDetailsRequest();
            SalesOrderDetailsDb salesOrderDb = _salesOrderRepository.GetSalesOrderFromExternal(request.ExternalId);
            SalesOrderDetailsDb salesOrderResult = new SalesOrderDetailsDb();

            int checkAccountId = _accountRepository.GetAccountIdByExternal(request.AccountExternalId);
            var account = _accountRepository.GetAccountBasicDetails(checkAccountId);
            if (account.IsSourceability)
            {
                return new BaseResponse(); //we dont want to process sales orders that have this type of account
            }

            if (salesOrderDb != null && salesOrderDb.SalesOrderId > 0)
            {                
                salesOrder = CreateSalesOrderDetailsRequest(request, salesOrderDb.SalesOrderId, salesOrderDb.VersionId);
                salesOrderResult = _salesOrderRepository.SetSalesOrderDetails(salesOrder);
            }
            else
            {
                salesOrder = CreateSalesOrderDetailsRequest(request);
                salesOrderResult = _salesOrderRepository.SetSalesOrderDetails(salesOrder);
                if(salesOrderResult.SalesOrderId > 0)
                {
                    var ownerId = _userRepo.GetUserIdFromExternal(request.OwnerExternalId);
                    var ownershipResult = _ownershipService.SetOwnership(new SetOwnershipRequest
                    {
                        ObjectID = salesOrderResult.SalesOrderId,
                        ObjectTypeID = (int)ObjectType.Salesorder,
                        OwnerList = new List<OwnerSetRequest> { new OwnerSetRequest { UserID = ownerId, Percentage = 100 } }
                    });

                    if(ownershipResult == null || ownershipResult.ObjectID == 0)
                    {
                        return new BaseResponse { ErrorMessage = string.Format("Failure to create Onwership for SalesOrder {0} created from ExternalID {1}", salesOrderResult.SalesOrderId, request.ExternalId) };
                    }
                }
            }            

            if(salesOrderResult != null && salesOrderResult.SalesOrderId > 0)
            {
                foreach(var line in request.Lines)
                {
                    SalesOrderLineDetail soLine = new SalesOrderLineDetail();
                    var soLineId = _salesOrderRepository.GetSoLineIdFromExternal(request.ExternalId, line.LineNum);
                    soLine = CreateSalesOrderLineSapRequest(line, soLineId, salesOrderResult.SalesOrderId, salesOrderResult.VersionId);                    
                    _salesOrderRepository.SetSalesOrderLines(soLine);
                }
            }
            else
            {
                return new BaseResponse { ErrorMessage = "Failure to create / update SalesOrder with external Id: " + request.ExternalId };
            }

            return new BaseResponse();
        }

        private SalesOrderLineDetail CreateSalesOrderLineSapRequest(SalesOrderLineSapDetails sapLine, int soLineId, int soId, int versionId)
        {
            SalesOrderLineDetail line = new SalesOrderLineDetail();
            line.SOLineId = soLineId;
            line.SalesOrderId = soId;
            line.SalesOrderVersionId = versionId;
            line.LineNum = Int32.Parse(sapLine.LineNum);
            line.ItemId = _salesOrderRepository.GetItemIdFromExternal(sapLine.ItemExternalId);
            line.CustomerLine = sapLine.CustomerLine != null ? Int32.Parse(sapLine.CustomerLine) : 0;
            line.Qty = sapLine.Qty;
            line.Price = sapLine.Price;
            line.Cost = sapLine.Cost;
            line.DateCode = sapLine.DateCode;
            line.PackingId = _commonDataRepo.GetPackagingIdFromExternal(sapLine.PackagingExternalId);
            line.PackageConditionId = _commonDataRepo.GetPackageConditionIdFromExternal(sapLine.PackageConditionExternalId);
            line.CustomerPartNum = sapLine.CustomerPartNum;
            line.ShipDate = sapLine.ShipDate != null ? sapLine.ShipDate.ToString("d", DateTimeFormatInfo.InvariantInfo) : null;
            line.DueDate = sapLine.DueDate != null ? sapLine.DueDate.ToString("d", DateTimeFormatInfo.InvariantInfo) : null ;
            line.IsDeleted = sapLine.IsDeleted;
            line.ProductSpec = sapLine.ProductSpec;

            return line;
        }

        public SalesOrderListResponse GetSalesOrderList(SearchFilter searchfilter)
        {
            var dbSalesOrders = _salesOrderRepository.GetSalesOrderList(searchfilter);
            var list = new List<SalesOrderResponse>();
            int TotalCount = 0;
            foreach (var dbSalesOrder in dbSalesOrders)
            {
                list.Add(new SalesOrderResponse
                {
                    SalesOrderID = dbSalesOrder.SalesOrderID,
                    VersionID = dbSalesOrder.VersionID,
                    AccountID = dbSalesOrder.AccountID,
                    AccountName = dbSalesOrder.AccountName,
                    ContactID = dbSalesOrder.ContactID,
                    ContactFirstName = dbSalesOrder.ContactFirstName,
                    ContactLastName = dbSalesOrder.ContactLastName,
                    StatusID = dbSalesOrder.StatusID,
                    StatusName = dbSalesOrder.StatusName,//Replace w/ Status = ((ItemStatusEnum)dbItem.ItemStatusID).GetEnumDescription(),
                    CountryName = dbSalesOrder.CountryName,
                    OrganizationID = dbSalesOrder.OrganizationID,
                    OrganizationName = dbSalesOrder.OrganizationName,
                    OrderDate = dbSalesOrder.OrderDate,
                    Owners = dbSalesOrder.Owners,
                    IncotermLocation = dbSalesOrder.IncotermLocation,
                    ExternalID = dbSalesOrder.ExternalID
                });
            }
            if (dbSalesOrders.Count() > 0)
                TotalCount = dbSalesOrders[0].TotalRows;
            return new SalesOrderListResponse { SalesOrders = list, TotalRowCount = TotalCount };
        }

        public GetSalesOrderLinesResponse GetSalesOrderLines(int soId, int soVersionId, SearchFilter searchFilter)
        {
            var soLines = new GetSalesOrderLinesResponse();
            var response = new List<SalesOrderLineDetail>();

            var dbSoLines = _salesOrderRepository.GetSalesOrderLines(soId, soVersionId, searchFilter);

            foreach (var line in dbSoLines)
            {
                response.Add(CreateSalesOrderLine(line)); 
            }

            soLines.SOLinesResponse = response;

            return soLines;
        }

        private static SalesOrderLineDetail CreateSalesOrderLine(SalesOrderLinesDb line)
        {
            return new SalesOrderLineDetail
            {
                SOLineId = line.SOLineId,
                StatusId = line.StatusId,
                StatusName = line.StatusName,
                StatusIsCanceled = line.StatusIsCanceled,
                LineNum = line.LineNum,
                ItemId = line.ItemId,
                CustomerLine = line.CustomerLine,
                PartNumber = line.PartNumber,
                CommodityId = line.CommodityId,
                CommodityName = line.CommodityName,
                CustomerPartNum = line.CustomerPartNum,
                Qty = line.Qty,
                Reserved = line.Reserved,
                Price = line.Price,
                Cost = line.Cost,
                DeliveryRuleId = line.DeliveryRuleId,
                DateCode = line.DateCode,
                PackingId = line.PackagingId,
                PackagingName = line.PackagingName,
                PackageConditionId = line.PackageConditionID,
                ShipDate = line.ShipDate == DateTime.MinValue ? null :line.ShipDate.ToString("MM/dd/yyyy"),
                DueDate = line.DueDate == DateTime.MinValue ? null : line.DueDate.ToString("MM/dd/yyyy"),
                MfrName = line.MfrName,
                TotalRows = line.TotalRows,
                Comments = line.Comments
            };
        }

        public SalesOrderLineDetail SetSalesOrderLines(SalesOrderLineDetail setSalesOrderLinesRequest)
        {
            var dbSalesOrderLine = _salesOrderRepository.SetSalesOrderLines(setSalesOrderLinesRequest);
            return CreateSalesOrderLine(dbSalesOrderLine);
        }

        public SalesOrderLinesDeleteResponse DeleteSalesOrderLines(List<int> soLineIds)
        {
            var dbDeleteSalesOrderLines = _salesOrderRepository.DeleteSalesOrderLines(soLineIds);
            var response = new SalesOrderLinesDeleteResponse();

            if (dbDeleteSalesOrderLines != null)
            {
                response.IsSuccess = true;
            }

            return response;
        }

        public SalesOrderExtraResponse GetSalesOrderExtra(int soId, int soVersionId, int rowOffset, int rowLimit)
        {
            var dbSalesOrderExtra = _salesOrderRepository.GetSalesOrderExtra(soId, soVersionId, rowOffset, rowLimit);
            var response = new List<ExtraListResponse>();

            foreach (var value in dbSalesOrderExtra)
            {
                response.Add(new ExtraListResponse
                {
                    SOExtraId = value.SOExtraId,
                    LineNum = value.LineNum,
                    RefLineNum = value.RefLineNum,
                    ItemExtraId = value.ItemExtraId,
                    ExtraName = value.ExtraName,
                    ExtraDescription = value.ExtraDescription,
                    Note = value.Note,
                    Qty = value.Qty,
                    Price = value.Price,
                    Cost = value.Cost,
                    Gpm = value.Gpm,
                    StatusId = value.StatusId,
                    StatusName = value.StatusName,
                    PrintOnSO = value.PrintOnSO,
                    TotalRows = value.TotalRows,
                    Comments = value.Comments

                });
            }
            return new SalesOrderExtraResponse(){ ExtraListResponse = response};
        }

        public SetSalesOrderExtraResponse SetSalesOrderExtra(SetSalesOrderExtraRequest setSalesOrderExtraRequest)
        {
            var dbSetSalesOrderExtra = _salesOrderRepository.SetSalesOrderExtra(setSalesOrderExtraRequest);
            var response = new SetSalesOrderExtraResponse();

            response.SOExtraId = dbSetSalesOrderExtra.SOExtraId;

            return response;
        }

        public SalesOrderExtrasDeleteResponse DeleteSalesOrderExtras(List<int> soExtraIds)
        {
            var dbDeleteSalesOrderExtras = _salesOrderRepository.DeleteSalesOrderExtras(soExtraIds);
            var response = new SalesOrderExtrasDeleteResponse();

            if (dbDeleteSalesOrderExtras != null)
            {
                response.IsSuccess = true;
            }

            return response;
        }
        
        public SalesOrderDetailsResponse GetSalesOrderDetails(int soId, int versionId)
        {
            var dbSalesOrderDetails = _salesOrderRepository.GetSalesOrderDetails(soId, versionId);
            var dbOrg = _salesOrderRepository.GetSalesOrderOrganization(soId, versionId);

            var response = new SalesOrderDetailsResponse();
            var orgResp = new SalesOrderOrganization();
            orgResp.Bank = new SalesOrderOrganizationBank();

            response.AccountId = dbSalesOrderDetails.AccountId;
            response.AccountName = dbSalesOrderDetails.AccountName;
            response.ContactId = dbSalesOrderDetails.ContactId;
            response.ContactName = dbSalesOrderDetails.ContactName;
            response.CustomerPo = dbSalesOrderDetails.CustomerPo;
            response.Email = dbSalesOrderDetails.Email;
            response.Phone = dbSalesOrderDetails.OfficePhone;
            response.SalesOrderId = dbSalesOrderDetails.SalesOrderId;
            response.VersionId = dbSalesOrderDetails.VersionId;
            response.ShipLocationId = dbSalesOrderDetails.ShipLocationId;
            response.ShipLocationName = dbSalesOrderDetails.ShipLocationName;
            response.StatusId = dbSalesOrderDetails.StatusId;
            response.StatusName = dbSalesOrderDetails.StatusName;
            response.DeliveryRuleId = dbSalesOrderDetails.DeliveryRuleId;
            response.UltDestinationId = dbSalesOrderDetails.UltDestinationId;
            response.UltDestinationName = dbSalesOrderDetails.UltDestinationName;
            response.SOCost = dbSalesOrderDetails.SOCost;
            response.SOGPM = dbSalesOrderDetails.SOGPM;
            response.SOPrice = dbSalesOrderDetails.SOPrice;
            response.SOProfit = dbSalesOrderDetails.SOProfit;
            response.ShippingMethodId = dbSalesOrderDetails.ShippingMethodId;
            response.ShipFromRegionID = dbSalesOrderDetails.ShipFromRegionID;
            response.CurrencyId = dbSalesOrderDetails.CurrencyId;
            response.PaymentTermId = dbSalesOrderDetails.PaymentTermId;
            response.ProjectId = dbSalesOrderDetails.ProjectId;
            response.IncotermId = dbSalesOrderDetails.IncotermId;
            response.OrganizationId = dbSalesOrderDetails.OrganizationId;
            response.FreightPaymentId = dbSalesOrderDetails.FreightPaymentId;
            response.FreightAccount = dbSalesOrderDetails.FreightAccount;
            response.OrderDate = dbSalesOrderDetails.OrderDate.ToShortDateString();
            response.ShippingNotes = dbSalesOrderDetails.ShippingNotes;
            response.QCNotes = dbSalesOrderDetails.QCNotes;
            response.CarrierId = dbSalesOrderDetails.CarrierId;
            response.CarrierName = dbSalesOrderDetails.CarrierName;
            response.MethodName = dbSalesOrderDetails.MethodName;
            response.CarrierMethodId = dbSalesOrderDetails.CarrierMethodId;
            response.UserID = dbSalesOrderDetails.UserID;
            response.ExternalID = dbSalesOrderDetails.ExternalId;

            //Organization data
            if (dbOrg != null)
            {
                orgResp.OrganizationName = dbOrg.OrganizationName;
                orgResp.Address1 = dbOrg.Address1;
                orgResp.Address2 = dbOrg.Address2;
                orgResp.Address4 = dbOrg.Address4;
                orgResp.City = dbOrg.City;
                orgResp.Email = dbOrg.Email;
                orgResp.Fax = dbOrg.Fax;
                orgResp.HouseNumber = dbOrg.HouseNumber;
                orgResp.MobilePhone = dbOrg.MobilePhone;
                orgResp.OfficePhone = dbOrg.OfficePhone;
                orgResp.Street = dbOrg.Street;
                orgResp.StateCode = dbOrg.StateCode;
                orgResp.StateName = dbOrg.StateName;
                orgResp.CountryName = dbOrg.CountryName;
                orgResp.PostalCode = dbOrg.PostalCode;

                orgResp.Bank.BranchName = dbOrg.BranchName;
                orgResp.Bank.BankName = dbOrg.BankName;
                orgResp.Bank.USDAccount = dbOrg.USDAccount;
                orgResp.Bank.EURAccount = dbOrg.EURAccount;
                orgResp.Bank.SwiftAccount = dbOrg.SwiftAccount;
                orgResp.Bank.RoutingNumber = dbOrg.RoutingNumber;

            }
            
            response.Organization = orgResp;
            return response;
        }

        private void SetSalesOrderLinesSapData(SalesOrderDetailsRequest request)
        {
            int soId = request.SalesOrderId;
            int versionId = request.VersionID;

            foreach(var item in request.Lines)
            {
                SalesOrderLineDetail line = new SalesOrderLineDetail();
                line.SalesOrderId = soId;
                line.SalesOrderVersionId = versionId;
                line.LineNum = item.LineNum;
                line.ProductSpec = item.ProductSpec;
                _salesOrderRepository.SetSalesOrderLinesSapData(line);
            }             

            foreach(var item in request.Items)
            {
                _itemService.SetItemExternalId(new SetExternalIdRequest { ObjectId = item.Id, ExternalId = item.ExternalId});
            }
        }

        public AccountsResponse GetAccountsList()
        {
            var dbCustomerAccount = _quoteRepository.GetAllCustomers();
            var response = new List<Account>();

            foreach (var value in dbCustomerAccount)
            {
                response.Add(new Account { Name = value.AccountName, Id = value.AccountId});
            }
            return new AccountsResponse {Accounts = response};
        }

        public SyncResponse Sync(int soId, int versionId)
        {            
            return _salesOrderMiddlewareClient.Sync(soId, versionId);
        }

        public SalesOrderLineListResponse GetSalesOrderLineByAccountId(int accountId, int? contactId=null)
        {
            var dbSoLineHistory = _salesOrderRepository.GetSalesOrderLineByAccountId(accountId,contactId);
            var response = new List<SalesOrderLineResponse>();
            var SOLineResponse = new SalesOrderLineListResponse();

            foreach (var value in dbSoLineHistory)
            {
                response.Add(new SalesOrderLineResponse
                {
                    SOLineId = value.SOLineId,
                    LineNum = value.LineNum,
                    SOExternalID = value.SOExternalID,
                    OrderDate = value.OrderDate,
                    ShipDate = value.ShipDate,
                    PartNumber = value.PartNumber,
                    Manufacturer = value.MfrName,
                    Qty = value.Qty,
                    Price = value.Price,
                    Cost = value.Cost,
                    GPM = value.GPM,
                    DateCode = value.DateCode,
                    Packaging = value.PackagingName,
                    SalesOrderID = value.SalesOrderID,
                    VersionId = value.VersionId,
                    AccountId = value.AccountId,
                    AccountName = value.AccountName,
                    ContactId = value.ContactId,
                    FirstName = value.FirstName,
                    LastName = value.LastName,
                    StatusId = value.StatusId,
                    StatusName = value.StatusName,
                    FullName= FullNameFormatter.FormatFullName(value.FirstName,value.LastName),
                    Owners = CleanLists(value.Owners)
                });

            }
            SOLineResponse.SOLineList = response;

            return SOLineResponse;
        }

        private static string CleanLists(string stringToClean)
        {
            return !string.IsNullOrEmpty(stringToClean) ? stringToClean.Trim().TrimEnd(',') : null;
        }
    }
}
