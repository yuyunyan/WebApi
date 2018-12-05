using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sourceportal.DB.Accounts;
using Sourceportal.DB.Comments;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Quotes;
using Sourceportal.Domain.Models.API.Requests.Quotes;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using Sourceportal.Domain.Models.API.Responses.Quotes;
using Sourceportal.Domain.Models.API.Responses.Ownership;
using Sourceportal.Domain.Models.DB.Accounts;
using Sourceportal.Domain.Models.DB.Quotes;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using Sourceportal.Utilities;
using SourcePortal.Services.Accounts;
using SourcePortal.Services.Ownership;

namespace SourcePortal.Services.Quotes
{
   public class QuoteService: IQuoteService
   {
       private readonly IQuoteRepository _quoteRepository;
       private readonly ICommentRepository _commentRepository;
       private readonly IAccountRepository _accountRepository;
       private readonly IAccountService _accountService;
       private readonly IOwnershipService _ownershipService;

       public QuoteService(IQuoteRepository quoteRepository, ICommentRepository commentRepository,
           IAccountRepository accountRepository, IAccountService accountService, IOwnershipService ownershipService)
       {
           _quoteRepository = quoteRepository;
           _commentRepository = commentRepository;
           _accountRepository = accountRepository;
           _accountService = accountService;
            _ownershipService = ownershipService;
       }

        public QuoteDetailsResponse GetQuoteDetails(int quoteId, int versionId)
        {

            var dbQuoteDetails = _quoteRepository.GetQuoteDetails(quoteId, versionId);

            return CreateQuoteDetails(dbQuoteDetails);
        }

       private static double ConvertHoursToTotalDays(double hours)
       {
           TimeSpan result = TimeSpan.FromHours(hours);

           return result.TotalDays;
        }

       public SetQuoteDetailsResponse SetQuoteDetails(SetQuoteDetailsRequest setQuoteDetailsRequest)
       {
           var dbQuoteDetails = _quoteRepository.SetQuoteDetails(setQuoteDetailsRequest);
           var response = new SetQuoteDetailsResponse();
           response.QuoteId = dbQuoteDetails.QuoteId;
           response.VersionId = dbQuoteDetails.VersionId;
           return response;

       }
       private QuoteDetailsResponse CreateQuoteDetails(QuoteDetailsDb dbQuoteDetails)
       {
           var response = new QuoteDetailsResponse();
           var owners = new List<Owner>();
           var validDays = ConvertHoursToTotalDays(dbQuoteDetails.ValidForHours);
           response.QuoteId = dbQuoteDetails.QuoteId;
           response.VersionId = dbQuoteDetails.VersionId;
           response.AccountId = dbQuoteDetails.AccountId;
           response.ContactId = dbQuoteDetails.ContactId;
           response.Email = dbQuoteDetails.Email;
           response.ShipLocationId = dbQuoteDetails.ShipLocationId;
           response.OfficePhone = dbQuoteDetails.OfficePhone;
           response.StatusId = dbQuoteDetails.StatusId;
           response.ValidDays = validDays;
           response.OrganizationId = dbQuoteDetails.OrganizationID;
           response.IncotermId = dbQuoteDetails.IncotermID;
           response.PaymentTermId = dbQuoteDetails.PaymentTermID;
           response.CurrencyId = dbQuoteDetails.CurrencyID;
           response.ShippingMethodId = dbQuoteDetails.ShippingMethodID;
           response.QuoteTypeId = dbQuoteDetails.QuoteTypeID;
           response.IncotermLocation = dbQuoteDetails.IncotermLocation;

           foreach (var ownerDb in dbQuoteDetails.OwnerList)
           {
               owners.Add(new Owner
               {
                   Name = ownerDb.OwnerFirstName + " " + ownerDb.OwnerLastName,
                   UserId = ownerDb.OwnerId,
                   Percentage = ownerDb.Percent
               });

           }
           response.OwnerList = owners;
           return response;
       }

       public QuoteHeaderResponse GetQuoteHeader(int quoteId, int versionId)
       {
           var dbQuoteHeader = _quoteRepository.GetQuoteHeader(quoteId, versionId);
           var response = new QuoteHeaderResponse();
           response.QuoteProfit = dbQuoteHeader.QuoteProfit;
           response.QuoteGPM = dbQuoteHeader.QuoteGPM;
           response.QuoteCost = dbQuoteHeader.QuoteCost;
           response.QuotePrice = dbQuoteHeader.QuotePrice;
           response.AccountID = dbQuoteHeader.AccountID;
           response.Salesperson = dbQuoteHeader.Salesperson;
           response.SalespersonEmail = dbQuoteHeader.SalespersonEmail;
           response.SentDate = dbQuoteHeader.SentDate;
           response.Created = dbQuoteHeader.Created;
           response.UserID = dbQuoteHeader.UserID;
           return response;
       }

       public List<CustomerAccount> GetAllCustomers()
       {
           var dbCustomerAccount = _quoteRepository.GetAllCustomers();
           var response = new List<CustomerAccount>();

           foreach (var value in dbCustomerAccount)
           {
                response.Add(new CustomerAccount { Name= value.AccountName,AccountId = value.AccountId});
           }

           return response;
       }

       public List<AccountContact> GetAccountTypeContacts(int accountId)
       {
           var dbAccountContact = _quoteRepository.GetAccountTypeContacts(accountId);
           var response = new List<AccountContact>();

           foreach (var value in dbAccountContact)
           {
               response.Add(new AccountContact {Name = value.ContactName,Id = value.ContactId});
           }
           return response;
       }

       public List<AccountShipAddress> GetAccountTypeAddress(int accountId)
       {
           var dbAccountShipAddress = _quoteRepository.GetAccountTypeAddress(accountId);
           var response = new List<AccountShipAddress>();

           foreach (var value in dbAccountShipAddress)
           {
               response.Add(new AccountShipAddress {Name = value.LocationName,Id = value.LocationId});
           }
           return response;
       }

       public List<Status> GetAllQuoteStatus()
       {
           var dbQuoteStatus = _quoteRepository.GetAllQuoteStatus();
           var response = new List<Status>();

           foreach (var value in dbQuoteStatus)
           {
               response.Add(new Status {Name = value.StatusName,Id = value.StatusId});
           }
           return response;
       }

       public List<QuoteType> GetAllQuoteTypes()
       {
           var dbQuoteTypes = _quoteRepository.GetAllQuoteTypes();
           var response = new List<QuoteType>();

           foreach (var value in dbQuoteTypes)
           {
               response.Add(new QuoteType { Name = value.TypeName, Id = value.QuoteTypeId });
           }
           return response;
       }

        public PartsResponse GetPartsList(QuotePartsFilter filter)
        {
           var dbPartsList = _quoteRepository.GetAllParts(filter);
           //var response = new List<PartsResponse>();
           var partsList = new PartsResponse();
           var response = new List<PartDetails>();

           foreach (var value in dbPartsList)
           {
               if (value.AltFor == 0)
               {
                    response.Add(new PartDetails
                    {
                        PartNumber = value.PartNumber,
                        PartNumberStrip = value.PartNumberStrip,
                        QuoteLineId = value.QuoteLineId,
                        StatusId = value.StatusId,
                        StatusName = value.StatusName,
                        StatusCanceled = value.StatusIsCanceled,
                        LineNo = value.LineNum,
                        CommodityName = value.CommodityName,
                        CustomerPartNo = value.CustomerPartNum,
                        CommodityId = value.CommodityId,
                        Quantity = value.Qty,
                        Price = value.Price,
                        Cost = value.Cost,
                        GPM = value.GPM,
                        DateCode = value.DateCode,
                        PackingId = value.PackagingId,
                        PackingName = value.PackagingName,
                        ShipDate = value.ShipDate.ToString("MM/dd/yyyy"),
                        CustomerLine = value.CustomerLine,
                        ItemId = value.ItemId,
                        Manufacturer = value.Manufacturer,
                        Alternates = new List<PartDetails>(),
                        Comments = value.Comments,
                        RoutedTo = MapRouteToJSONObject(value.RoutedTo),
                        LeadTimeDays = value.LeadTimeDays,
                        IsPrinted = value.IsPrinted,
                        SourceMatchStatus = value.HasSourceMatch,
                        SourceMatchCount = value.SourceMatchCount,
                        SourceMatchQty = value.SourceMatchQty,
                        SourceType = value.SourceType
                    });
                }

           }

           foreach (var v in dbPartsList)
           {
               if (v.AltFor != 0)
               {
                   var parentPart = response.FirstOrDefault(x => x.QuoteLineId == v.AltFor);

                   if (parentPart != null)
                   {
                       parentPart.Alternates.Add(new PartDetails
                       {
                           PartNumber = v.PartNumber,
                           PartNumberStrip = v.PartNumberStrip,
                           QuoteLineId = v.QuoteLineId,
                           StatusId = v.StatusId,
                           StatusName = v.StatusName,
                           StatusCanceled = v.StatusIsCanceled,
                           LineNo = v.LineNum,
                           CommodityName = v.CommodityName,
                           CustomerPartNo = v.CustomerPartNum,
                           LeadTimeDays = v.LeadTimeDays,
                           CommodityId = v.CommodityId,
                           Quantity = v.Qty,
                           Price = v.Price,
                           Cost = v.Cost,
                           GPM = v.GPM,
                           DateCode = v.DateCode,
                           PackingId = v.PackagingId,
                           PackingName = v.PackagingName,
                           ShipDate = v.ShipDate.ToString("MM/dd/yyyy"),
                           CustomerLine = v.CustomerLine,
                           ItemId = v.ItemId,
                           Manufacturer = v.Manufacturer,
                           Comments = v.Comments,
                           RoutedTo = MapRouteToJSONObject(v.RoutedTo)
                       });
                   }
               }
           }

           partsList.PartsListResponse = response;
           return partsList;
        }

        public int SetQuoteLinePrint(QuotePrint quotePrint)
        {
            return _quoteRepository.SetQuoteLinePrint(quotePrint);

        }

       public List<QuoteRouteToResponse> MapRouteToJSONObject (string RouteToJSON)
       {
           var responseList = new List<QuoteRouteToResponse>();

            if (RouteToJSON == null)
            {
                return responseList;
            }
           var quoteRouteToObjectList = JsonConvert.DeserializeObject<List<QuoteRouteToMap>>(RouteToJSON);
           foreach (var quoteRouteToObject in quoteRouteToObjectList)
           {
               var buyerNameArray = quoteRouteToObject.BuyerName.Split();
               var buyerInitial = new StringBuilder();
               if (buyerNameArray.Length == 2)
               {
                   buyerInitial.Append(buyerNameArray[0].Length > 0 ? buyerNameArray[0][0] : ' ');
                   buyerInitial.Append(buyerNameArray[1].Length > 0 ? buyerNameArray[1][0] : ' ');
                }
                var response = new QuoteRouteToResponse
               {
                   BuyerInitials = buyerInitial.ToString(),
                   QuoteLineID = quoteRouteToObject.QuoteLineID,
                   StatusName = quoteRouteToObject.StatusName,
                   RouteStatusID = quoteRouteToObject.RouteStatusID,
                   BuyerName = quoteRouteToObject.BuyerName,
                   Icon = quoteRouteToObject.Icon,
                   IconColor = quoteRouteToObject.IconColor
               };
               responseList.Add(response);
            }
           
           return responseList;

       }

       public PartDetails SetPartList(PartDetails setPartsListRequest)
       {
           var dbSetPartList = _quoteRepository.SetPartList(setPartsListRequest);

           if (!string.IsNullOrEmpty(dbSetPartList.Error))
           {
               return new PartDetails {ErrorMessage = dbSetPartList.Error};
           }

           var response = new PartDetails();

           response.QuoteLineId = dbSetPartList.QuoteLineId;
           response.PackingName = dbSetPartList.PackagingName;
           response.Price = dbSetPartList.Price;
           response.DateCode = dbSetPartList.DateCode;
           response.Quantity = dbSetPartList.Qty;
           response.ShipDate = dbSetPartList.ShipDate.ToString("MM/dd/yyyy");
           response.PackageConditionID = dbSetPartList.PackageConditionID;
           response.ConditionName = dbSetPartList.ConditionName;
           response.CommodityId = dbSetPartList.CommodityId;
           response.CommodityName = dbSetPartList.CommodityName;
           response.Cost = dbSetPartList.Cost;
           response.CustomerLine = dbSetPartList.CustomerLine;
           response.CustomerPartNo = dbSetPartList.CustomerPartNum;
           response.GPM = dbSetPartList.GPM;
           response.ItemId = dbSetPartList.ItemId;
           response.LineNo = dbSetPartList.LineNum;
           response.PackingId = dbSetPartList.PackagingId;
           response.StatusCanceled = dbSetPartList.StatusIsCanceled;
           response.StatusId = dbSetPartList.StatusId;
           response.StatusName = dbSetPartList.StatusName;
           response.PartNumber = dbSetPartList.PartNumber;
           response.PartNumberStrip = dbSetPartList.PartNumberStrip;
           response.SourceMatchStatus = dbSetPartList.HasSourceMatch;
           response.SourceMatchCount = dbSetPartList.SourceMatchCount;
           response.SourceMatchQty = dbSetPartList.SourceMatchQty;
           response.SourceType = dbSetPartList.SourceType;
           response.LeadTimeDays = dbSetPartList.LeadTimeDays;

           if (dbSetPartList.AltFor != 0)
           {
               response.AlternateId = dbSetPartList.AltFor;
           }
  
           return response;
       }

        public QuotePartsDeleteResponse DeleteQuoteParts(List<int> quoteLineIds)
       {
           var dbDeleteQuoteParts = _quoteRepository.DeleteQuoteParts(quoteLineIds);
           var response = new QuotePartsDeleteResponse();

           if (dbDeleteQuoteParts != null)
           {
               response.IsSuccess = true;
           }
           return response;

       }

       public CommodityOptionsResponse GetCommodityOptins()
       {
           var dbCommodityOptions = _quoteRepository.GetCommodityOptions();

           var response = new List<CommodityResponse>();

           foreach (var value in dbCommodityOptions)
           {
               response.Add(new CommodityResponse
               {
                  Name = value.CommodityName,
                  Id = value.CommodityId
               });
           }

           return new CommodityOptionsResponse {Commodity = response};
       }

       public PackagingOptionsResponse GetPackagingOptions()
       {
          var dbPackagingOptions = _quoteRepository.GetPackaingOptions();
          var response = new List<PackagingResponse>();

           foreach (var value in dbPackagingOptions)
           {
                response.Add(new PackagingResponse
                {
                    Name = value.PackagingName,
                    Id = value.PackagingId
                });
               
           }
           return new PackagingOptionsResponse {PackagingList = response};
       }

        public PackagingOptionsResponse GetConditionOptions()
        {
            var dbConditionOptions = _quoteRepository.GetConditionOptions();
            var response = new List<PackagingResponse>();

            foreach (var value in dbConditionOptions)
            {
                response.Add(new PackagingResponse
                {
                    Name = value.ConditionName,
                    Id = value.PackageConditionID
                });

            }
            return new PackagingOptionsResponse { ConditionList = response };
        }
        
       public QuoteExtraResponse GetQuoteExtra(int quoteId, int quoteVersionId, int rowOffset, int rowLimit)
       {
           var dbQuoteExtraList = _quoteRepository.GetQuoteExtra(quoteId, quoteVersionId,rowOffset,rowLimit);
           var response = new List<ExtraListResponse>();

           foreach (var value in dbQuoteExtraList)
           {
                response.Add(new ExtraListResponse
                {
                    QuoteExtraId = value.QuoteExtraId,
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
                    CommentCount = value.CommentCount,
                    PrintOnQuote = value.PrintOnQuote,
                    TotalRows = value.TotalRows,
                    Comments = value.Comments
                });
           }
           return new QuoteExtraResponse {ExtraListResponse = response};
       }

       public SetQuoteExtraResponse SetQuoteExtra(SetQuoteExtraRequest setQuoteExtraRequest)
       {
           var dbSetQuoteExtra = _quoteRepository.setQuoteExtra(setQuoteExtraRequest);
           var response = new SetQuoteExtraResponse();
           response.QuoteExtraId = dbSetQuoteExtra.QuoteExtraId;
           return response;
       }

        public QuoteListResponse GetQuoteList(SearchFilter searchfilter)
        {
           var dbAllQuoteList = _quoteRepository.getQuoteList(searchfilter);

           var response = new List<QuoteResponse>();
           var main = new QuoteListResponse();
           var TotalCount = 0;

           foreach (var value in dbAllQuoteList)
           {
                response.Add(new QuoteResponse
                {
                    QuoteId = value.QuoteId,
                    VersionId = value.VersionId,
                    AccountId = value.AccountId,
                    AccountName = value.AccountName,
                    ContactId = value.ContactId,
                    ContactFirstName = value.ContactFirstName,
                    ContactLastName = value.ContactLastName,
                    StatusId = value.StatusId,
                    StatusName = value.StatusName,
                    OrganizationId = value.OrganizationId,
                    SentDate = value.SentDate,
                    ContactFullName = (string.IsNullOrEmpty(value.ContactFirstName)? "": value.ContactFirstName.Trim() + " ")  +
                       (string.IsNullOrEmpty(value.ContactLastName) ? "" : value.ContactLastName.Trim()),
                    Owners = CleanLists(value.Owners),
                    CountryName = value.CountryName
                });
               
           }
           if (dbAllQuoteList.Count() > 0)
           {
               TotalCount = dbAllQuoteList[0].TotalRows;

           }
           if (response !=null)
           {
               main.TotalRowCount = TotalCount;
               main.IsSuccess = true;
           }

           main.QuoteList = response;

           return main;
        }

       public QuoteToSOResponse QuoteToSalesOrder(QuoteToSORequest quoteToSoRequest)
       {
           var dbNewSalesOrder = _quoteRepository.QuoteToSalesOrder(quoteToSoRequest);
       
           var response = new QuoteToSOResponse();

           response.SalesOrderId = dbNewSalesOrder.SalesOrderId;
           response.VersionId = dbNewSalesOrder.VersionId;
           response.LinesCopiedCount = dbNewSalesOrder.LinesCopiedCount;
           response.ExtrasCopiedCount = dbNewSalesOrder.ExtrasCopiedCount;

           if (response.SalesOrderId > 0)
           {
               response.IsSuccess = true;
           }
           else
           {
               response.ErrorMessage = "Fail to convert Quote to Sales Order!";
           }

           return response;

       }

       private static string CleanLists(string stringToClean)
       {
           return !string.IsNullOrEmpty(stringToClean) ? stringToClean.Trim().TrimEnd(',') : null;
       }

       public int RouteQuoteLines(RouteQuoteLineRequest routeQuoteLineRequest)
       {
           var result = _quoteRepository.RouteQuoteLines(routeQuoteLineRequest);
            return result;
       }

       public SetQuoteDetailsResponse SetQuoteExistingCustomer(SetQuoteExistingCustomerRequest setQuoteExistingCustomerRequest)
       {
           var setQuoteDetailsRequest = new SetQuoteDetailsRequest();

           setQuoteDetailsRequest.AccountId = setQuoteExistingCustomerRequest.AccountID;
           setQuoteDetailsRequest.ContactId = setQuoteExistingCustomerRequest.ContactID;
           var dbQuoteDetails = _quoteRepository.SetQuoteDetails(setQuoteDetailsRequest);

            
           var quoteParts = setQuoteExistingCustomerRequest.QuoteParts;

           var quoteId = dbQuoteDetails.QuoteId;
           var versionId = dbQuoteDetails.VersionId;


           var commentId = _commentRepository.SetComment(new SetCommentRequest
           {
               CommentTypeID = (int) CommentType.SalesQuote,
               Comment = setQuoteExistingCustomerRequest.Comment,
               ObjectID = quoteId,
               ObjectTypeID = (int) ObjectType.Quote
           }).CommentID;

            var ownerList = new List<Sourceportal.Domain.Models.API.Requests.Ownership.OwnerSetRequest>
               {
                   new Sourceportal.Domain.Models.API.Requests.Ownership.OwnerSetRequest
                   {
                       Percentage = 100,
                       UserID = UserHelper.GetUserId()
                   }
               };

            _ownershipService.SetOwnership(new SetOwnershipRequest
            {
                ObjectID = quoteId,
                ObjectTypeID = (int)ObjectType.Quote,
                OwnerList = ownerList

            });

           var quotePartDbs = _quoteRepository.SetPartLists(quoteId, versionId, quoteParts);

           return new SetQuoteDetailsResponse
           {
               QuoteId = quoteId,
               VersionId = versionId,
               IsSuccess = (quotePartDbs.Count == quoteParts.Count) && (setQuoteExistingCustomerRequest.Comment == null || commentId > 0)
           };
       }

       public SetQuoteDetailsResponse SetQuoteNewCustomer(SetQuoteNewCustomerRequest setQuoteNewCustomerRequest)
       {
           /*
            * Set Owner to user
            */
           var owerList = new List<Sourceportal.Domain.Models.API.Requests.Accounts.OwnerSetRequest>
           {
               new Sourceportal.Domain.Models.API.Requests.Accounts.OwnerSetRequest
               {
                   Percentage = 100,
                   UserId = UserHelper.GetUserId()
               }
           };

           setQuoteNewCustomerRequest.ContactDetails.OwnerList = owerList;

            /*
             * Set Account
             */
           setQuoteNewCustomerRequest.AccountDetails.OrganizationId = setQuoteNewCustomerRequest.OrganizationID;
           var accountId = _accountService.SetAccountBasicDetails(setQuoteNewCustomerRequest.AccountDetails).AccountId;
            
           setQuoteNewCustomerRequest.ContactDetails.AccountId = accountId;
            
           /*
            * Set Contact for account
            */
           var contactId = _accountService.SetContactDetails(setQuoteNewCustomerRequest.ContactDetails).ContactId;
           
           var setQuoteDetailsRequest = new SetQuoteDetailsRequest
           {
               AccountId = accountId,
               ContactId = contactId,
           };

           /*
            * Set Quote for account and contact, then set QuoteLines
            */
           var dbQuoteDetails = _quoteRepository.SetQuoteDetails(setQuoteDetailsRequest);

           var quoteParts = setQuoteNewCustomerRequest.QuoteParts;

           var quoteId = dbQuoteDetails.QuoteId;
           var versionId = dbQuoteDetails.VersionId;

           var quotePartDbs = _quoteRepository.SetPartLists(quoteId, versionId, quoteParts);

           /*
            * Create comment
            */
           var commentId = _commentRepository.SetComment(new SetCommentRequest
           {
               CommentTypeID = (int)CommentType.SalesQuote,
               Comment = setQuoteNewCustomerRequest.Comment,
               ObjectID = quoteId,
               ObjectTypeID = (int)ObjectType.Quote
           }).CommentID;

           return new SetQuoteDetailsResponse
           {
               QuoteId = quoteId,
               VersionId = versionId,
               IsSuccess = (quotePartDbs.Count == quoteParts.Count) && (setQuoteNewCustomerRequest.Comment == null || commentId > 0)
           };
       }

        public QuoteLineHistoryResponse GetQuoteLineByAccountId(int accountId,int? contactId=null)
        {
            var dbQuoteLine = _quoteRepository.GetQuoteLineByAccountId(accountId, contactId);
            var response = new List<QuoteLineResponse>();
            var quoteLineResponse = new QuoteLineHistoryResponse();
           
            foreach (var value in dbQuoteLine)
            {
                response.Add(new QuoteLineResponse
                {
                    QuoteLineId = value.QuoteLineId,
                    QuoteDate= value.SentDate,
                    PartNumber= value.PartNumber,
                    ItemID = value.ItemID,
                    Manufacturer = value.Manufacturer,
                    Qty= value.Qty,
                    Price = value.Price,
                    Cost = value.Cost,
                    GPM= value.GPM,
                    DateCode= value.DateCode,
                    Packaging= value.PackagingName,
                    QuoteId = value.QuoteId,
                    LineNum = value.LineNum,
                    VersionId = value.VersionId,
                    AccountId = value.AccountId,
                    AccountName = value.AccountName,
                    ContactId = value.ContactId,
                    ContactFirstName = value.FirstName,
                    ContactLastName = value.LastName,
                    StatusId = value.StatusId,
                    StatusName = value.StatusName,
                    ContactFullName= FullNameFormatter.FormatFullName(value.FirstName,value.LastName),
                    Owners = CleanLists(value.Owners)
                });

            }
            quoteLineResponse.QuoteLineList = response;

            return quoteLineResponse;
        }
    }
}
