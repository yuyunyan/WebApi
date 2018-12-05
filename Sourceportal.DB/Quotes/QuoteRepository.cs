using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.DB.Quotes;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Items;
using Sourceportal.Domain.Models.API.Requests.Quotes;
using Sourceportal.Domain.Models.API.Responses.Quotes;
using Sourceportal.Domain.Models.DB.Accounts;
using Sourceportal.Domain.Models.Shared;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.Items;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.DB.Quotes
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly IItemRepository _itemRepository;

        public QuoteRepository(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public QuoteDetailsDb GetQuoteDetails(int quoteId, int versionId)
        {
            QuoteDetailsDb quoteDetailsDbs;
            IList<OwnerDb> ownerDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                DynamicParameters ownerParm = new DynamicParameters();

                param.Add("@QuoteID", quoteId);
                param.Add("@VersionID", versionId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                param.Add("@UserID", UserHelper.GetUserId());

                var res = con
                    .Query<QuoteDetailsDb>("uspQuoteGet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");

                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", QuoteDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                if (!res.Any())
                {
                    var errorMessage = string.Format("Database error occured: QuoteID {0} VersionID {1} not found",
                        quoteId, versionId);
                    throw new GlobalApiException(errorMessage);
                }

                quoteDetailsDbs = res.First();

                ownerParm.Add("@ObjectID", quoteId);
                ownerParm.Add("@ObjectTypeID", ObjectType.ObjectTypeId);

                ownerDbs = con.Query<OwnerDb>("uspOwnershipGet", ownerParm, commandType: CommandType.StoredProcedure)
                    .ToList();
                quoteDetailsDbs.OwnerList = ownerDbs.ToList();
                con.Close();
            }

            return quoteDetailsDbs;
        }

        public QuoteHeaderDb GetQuoteHeader(int quoteId, int versionId)
        {
            QuoteHeaderDb quoteHeaderDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@QuoteID", quoteId);
                param.Add("@VersionID", versionId);
                param.Add("@UserID", UserHelper.GetUserId());
               
                param.Add("@ret", direction: ParameterDirection.ReturnValue);


                var res = con
                    .Query<QuoteHeaderDb>("uspQuoteGet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");

                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", QuoteDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                if (!res.Any())
                {
                    var errorMessage = string.Format("Database error occured: QuoteID {0} VersionID {1} not found",
                        quoteId, versionId);
                    throw new GlobalApiException(errorMessage);
                }
                quoteHeaderDbs = res.First();

                con.Close();
            }
            return quoteHeaderDbs;
        }
    

        public QuoteDetailsDb SetQuoteDetails(SetQuoteDetailsRequest setQuoteDetailsRequest)
        {
            QuoteDetailsDb quoteDetailsDbs;
            
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var validHours = ConvertDaysToHours(setQuoteDetailsRequest.ValidForDays);

                param.Add("@QuoteID",setQuoteDetailsRequest.QuoteId);
                param.Add("@VersionID",setQuoteDetailsRequest.VersionId);
                param.Add("@AccountID",setQuoteDetailsRequest.AccountId);
                param.Add("@ContactID",setQuoteDetailsRequest.ContactId);
                param.Add("@StatusID",setQuoteDetailsRequest.StatusId);
                param.Add("@ShipLocationID",setQuoteDetailsRequest.ShipLocationId);
                param.Add("@OrganizationID", setQuoteDetailsRequest.OrganizationId);
                param.Add("@IncotermID", setQuoteDetailsRequest.IncotermId);
                param.Add("@PaymentTermID", setQuoteDetailsRequest.PaymentTermId);
                param.Add("@CurrencyID", setQuoteDetailsRequest.CurrencyId);
                param.Add("@ShippingMethodID", setQuoteDetailsRequest.ShippingMethodId);
                param.Add("@QuoteTypeID", setQuoteDetailsRequest.QuoteTypeId);
                param.Add("@ValidForHours", validHours); //Need convert days to hours
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@IncotermLocation", setQuoteDetailsRequest.IncotermLocation);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspQuoteSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", QuoteDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                param = new DynamicParameters();
                param.Add("@QuoteID", res.First().QuoteID);
                param.Add("@VersionID", res.First().VersionID);
                param.Add("@UserID", UserHelper.GetUserId());

                quoteDetailsDbs = con
                    .Query<QuoteDetailsDb>("uspQuoteGet", param, commandType: CommandType.StoredProcedure).First();

                con.Close();
            }
            return quoteDetailsDbs;
        }

        private static double ConvertDaysToHours(int days)
        {
            TimeSpan result = TimeSpan.FromDays(days);

            return result.TotalHours;
        }

        public IList<CustomerAccountDb> GetAllCustomers()
        {
                        IList<CustomerAccountDb> customerAccountDbs;
                        using (var con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            DynamicParameters parm = new DynamicParameters();
            
                            parm.Add("@AccountTypeID",ObjectType.AccountTypeCustomerId);
            
                            customerAccountDbs = con
                                .Query<CustomerAccountDb>("uspAccountsByTypeGet", parm, commandType: CommandType.StoredProcedure)
                                .ToList();
            
                            con.Close();
                        }
                        return customerAccountDbs;
//            IList<CustomerAccountDb> customerAccountDbs;
//            using (var con = new SqlConnection(ConnectionString))
//            {
//                con.Open();
//                customerAccountDbs = con
//                    .Query<CustomerAccountDb>("uspContactsGet", commandType: CommandType.StoredProcedure)
//                    .ToList();
//
//                con.Close();
//            }
//            return customerAccountDbs;
        }

        public IList<AccountContactDb> GetAccountTypeContacts(int accountId)
        {
            IList<AccountContactDb> accountContactDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@AccountID",accountId);

                accountContactDbs = con
                    .Query<AccountContactDb>("uspContactsByAccountGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                
                con.Close();
            }
            return accountContactDbs;
        }

        public IList<AccountShipAddressDb> GetAccountTypeAddress(int accountId)
        {
            IList<AccountShipAddressDb> accountShipAddressDbs;

            using (var con= new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("AccountID",accountId);

                accountShipAddressDbs = con
                    .Query<AccountShipAddressDb>("uspLocationsByAccountGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return accountShipAddressDbs;
        }

        public IList<QuoteStautsDb> GetAllQuoteStatus()
        {
            IList<QuoteStautsDb> quoteStautsDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@ObjectTypeID", ObjectType.ObjectTypeId);
                quoteStautsDbs = con
                    .Query<QuoteStautsDb>("uspStatusesGet",parm,commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return quoteStautsDbs;
        }

        public IList<QuoteTypesDb> GetAllQuoteTypes()
        {
            IList<QuoteTypesDb> quoteTypesDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();
                quoteTypesDbs = con
                    .Query<QuoteTypesDb>("uspQuoteTypesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return quoteTypesDbs;
        }

        public IList<PartsListDb> GetAllParts(QuotePartsFilter filter)
        {
            IList<PartsListDb> partsListDbs;
          

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@QuoteID",filter.QuoteID);
                parm.Add("@QuoteVersionID",filter.VersionID);
                parm.Add("@CommentTypeID", CommentType.QuotePart);
                parm.Add("@FilterBy", filter.FilterBy);
                parm.Add("@FilterText", filter.FilterText);

                partsListDbs = con.Query<PartsListDb>("uspQuoteLinesGet", parm,
                    commandType: CommandType.StoredProcedure).ToList();
        
                con.Close();
                
            }
            return partsListDbs;

        }

        public int SetQuoteLinePrint(QuotePrint quotePrint)
        {
            //QuotePrint quotePrint = new QuotePrint() { QuoteLineId = quoteLineId, IsPrinted = isPrinted };
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();
                DynamicParameters listParm = new DynamicParameters();

                param.Add("@QuoteLineID", quotePrint.QuoteLineId);
                param.Add("@IsPrinted", quotePrint.IsPrinted);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspQuoteLinePrintSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", QuoteDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
                return errorId;
            }
        }
        public PartsListDb SetPartList(PartDetails partDetails)
        {
            PartsListDb setPartListDb;
            using (var con= new SqlConnection(ConnectionString)) 
            {
                con.Open();

                if (partDetails.IsIhs)
                {
                   var itemDb = _itemRepository.CreateIhsItemInDb(partDetails.ItemId);
                    partDetails.ItemId = itemDb.ItemID;
                }

                DynamicParameters param = new DynamicParameters();
                DynamicParameters listParm = new DynamicParameters();

                param.Add("@QuoteLineID",partDetails.QuoteLineId);
                param.Add("@QuoteID",partDetails.QuoteId);
                param.Add("@QuoteVersionID",partDetails.QuoteVersionId);
                param.Add("@StatusID", partDetails.StatusId);
                param.Add("@ItemID",partDetails.ItemId);
                param.Add("@CommodityID",partDetails.CommodityId);
                param.Add("@AltFor",partDetails.AltFor);
                param.Add("@CustomerLine",partDetails.CustomerLine);
                param.Add("@CustomerPartNum",partDetails.CustomerPartNo);
                param.Add("@PartNumber",partDetails.PartNumber);
                param.Add("@Manufacturer",partDetails.Manufacturer);
                param.Add("@LeadTimeDays", partDetails.LeadTimeDays);
                param.Add("@Qty",partDetails.Quantity);
                param.Add("@TargetPrice",partDetails.TargetPrice);
                param.Add("@Price",partDetails.Price);
                param.Add("@Cost",partDetails.Cost);
                param.Add("@TargetDateCode",partDetails.TargetDateCode);
                param.Add("@DateCode",partDetails.DateCode);
                param.Add("@PackagingID",partDetails.PackingId);
                param.Add("@ShipDate",partDetails.ShipDate);
                param.Add("@IsRoutedToBuyers",partDetails.IsRoutedToBuyers);
                param.Add("@UserID",UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                
                var res = con.Query("uspQuoteLineSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", QuoteDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                
                listParm.Add("@QuoteLineID", res.First().QuoteLineID);

                setPartListDb = con.Query<PartsListDb>("uspQuoteLinesGet", listParm, commandType: CommandType.StoredProcedure).First();
                //var a = con.Query("uspQuoteLinesGet", listParm, commandType: CommandType.StoredProcedure);

                con.Close();
                //setPartListDb = null;
            }
            return setPartListDb;

        }

        public int DeleteQuoteParts(List<int> quoteLineIds)
        {
            //QuotePartsDeleteDb deleteQuoteParts;
            int count =0;

            using (var con= new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var addQuoteLineIdProperty = quoteLineIds.Select(x=> new  { QuoteLineID = x}).ToList();
                var jsonQuoteLineIds = JsonConvert.SerializeObject(addQuoteLineIdProperty);

                param.Add("@QuoteLinesJSON",jsonQuoteLineIds);
                param.Add("@UserID",UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                param.Add("@ResultCount", count,direction:ParameterDirection.ReturnValue);

                con.Query("uspQuoteLinesDelete", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", QuoteDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }
            return count;
        }

        public List<CommodityOptionsDb> GetCommodityOptions()
        {
            List<CommodityOptionsDb> commodityOptionsDb;

            using (var con= new SqlConnection(ConnectionString))
            {
                con.Open();
                commodityOptionsDb = con
                    .Query<CommodityOptionsDb>("uspItemCommoditiesGet", commandType: CommandType.StoredProcedure)
                    .ToList();

                con.Close();
            }
            return commodityOptionsDb;
        }

        public List<PackagingOptionsDb> GetPackaingOptions()
        {
            return GetPackagingOptions(0);
        }

        public List<PackageConditionsDb> GetConditionOptions()
        {
            {
                List<PackageConditionsDb> packagingOptionsDbs;

                DynamicParameters param = new DynamicParameters();

                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    packagingOptionsDbs = con
                        .Query<PackageConditionsDb>("uspPackageConditionsGet", param, commandType: CommandType.StoredProcedure).ToList();
                    con.Close();
                }
                return packagingOptionsDbs;
            }
        }

        public List<PackagingOptionsDb> GetPackagingOption(int packagingId)
        {
            return GetPackagingOptions(packagingId);
        }

        private List<PackagingOptionsDb> GetPackagingOptions(int packagingId)
        {
            List<PackagingOptionsDb> packagingOptionsDbs;

            DynamicParameters param = new DynamicParameters();

            param.Add("@PackagingID", packagingId);

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                packagingOptionsDbs = con
                    .Query<PackagingOptionsDb>("uspPackagingGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return packagingOptionsDbs;
        }

        public List<QuoteExtraDb> GetQuoteExtra(int quoteId, int quoteVersionId, int rowOffset, int rowLimit)
        {
            List<QuoteExtraDb> quoteExtraDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters parm = new DynamicParameters();

                parm.Add("@QuoteID",quoteId);
                parm.Add("@QuoteVersionID",quoteVersionId);
                parm.Add("@RowOffset",rowOffset);
                parm.Add("@RowLimit",rowLimit);
                parm.Add("@CommentTypeID", CommentType.QuoteExtra);

                quoteExtraDbs = con.Query<QuoteExtraDb>("uspQuoteExtrasGet", parm,
                    commandType: CommandType.StoredProcedure).ToList();


                con.Close();
            }
            return quoteExtraDbs;
        }

        public QuoteExtraDb setQuoteExtra(SetQuoteExtraRequest setQuoteExtraRequest)
        {
            QuoteExtraDb quoteExtraDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@QuoteExtraID",setQuoteExtraRequest.QuoteExtraId);
                param.Add("@QuoteID",setQuoteExtraRequest.QuoteId);
                param.Add("@QuoteVersionID",setQuoteExtraRequest.QuoteVersionId);
                param.Add("@ItemExtraID", setQuoteExtraRequest.ItemExtraId);
                param.Add("@RefLineNum",setQuoteExtraRequest.RefLineNum);
                param.Add("@StatusID",setQuoteExtraRequest.StatusId);
                param.Add("@Qty",setQuoteExtraRequest.Qty);
                param.Add("@Price",setQuoteExtraRequest.Price);
                param.Add("@Cost",setQuoteExtraRequest.Cost);
                param.Add("@PrintOnQuote",setQuoteExtraRequest.PrintOnQuote);
                param.Add("@Note",setQuoteExtraRequest.Note);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@IsDeleted",setQuoteExtraRequest.IsDeleted);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspQuoteExtraSet", param,
                    commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", QuoteDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                param = new DynamicParameters();
                param.Add("@QuoteExtraID", res.First().QuoteExtraID);

                quoteExtraDbs = con.Query<QuoteExtraDb>("uspQuoteExtrasGet", param,
                    commandType: CommandType.StoredProcedure).First();

                con.Close();

            }
            return quoteExtraDbs;
        }

        public List<QuoteListDb> getQuoteList(SearchFilter searchfilter)
        {
            List<QuoteListDb> quoteListDbs;

            using (var con= new SqlConnection(ConnectionString))
            {
               con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SearchString", searchfilter.SearchString);
                param.Add("@RowOffset", searchfilter.RowOffset);
                param.Add("@RowLimit", searchfilter.RowLimit);
                param.Add("@SortBy", searchfilter.SortCol);
                param.Add("@DescSort", searchfilter.DescSort);
                param.Add("@FilterBy", searchfilter.FilterBy);
                param.Add("FilterText", searchfilter.FilterText);
                param.Add("@IncludeComplete", searchfilter.IncludeCompleted);
                param.Add("@IncludeCanceled", searchfilter.IncludeCanceled);
                param.Add("@UserID", UserHelper.GetUserId());

                quoteListDbs = con.Query<QuoteListDb>("uspQuotesListGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close(); 
            }
            return quoteListDbs;
        }

        public NewSalesOrderDb QuoteToSalesOrder(QuoteToSORequest quoteToSoRequest)
        {
            NewSalesOrderDb newSalesOrderDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();
                param.Add("@QuoteID", quoteToSoRequest.QuoteId);
                param.Add("@CustomerPO", quoteToSoRequest.CustomerPO);
                param.Add("@UserID", UserHelper.GetUserId());

                var addQuoteLineProperty = quoteToSoRequest.LinesToCopy.Select(x => new { QuoteLineID = x.QuoteLineId }).ToList();
                var jsonQuoteLineIds = JsonConvert.SerializeObject(addQuoteLineProperty);
                param.Add("@LinesToCopyJSON", jsonQuoteLineIds);

                var addQuoteExtraProperty = quoteToSoRequest.ExtrasToCopy.Select(x => new { QuoteExtraID = x.QuoteExtraId }).ToList();
                var jsonQuoteExtraIds = JsonConvert.SerializeObject(addQuoteExtraProperty);
                param.Add("@ExtrasToCopyJSON", jsonQuoteExtraIds);

                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<NewSalesOrderDb>("uspQuoteToSO", param, commandType: CommandType.StoredProcedure);
                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", QuoteDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                newSalesOrderDb = res.First();
                con.Close();
            }

            return newSalesOrderDb;
        }

        public List<PartsListDb> SetPartLists(int quoteId, int versionId, List<SetPartsListRequest> quoteParts)
        {
            var partListDbs = new List<PartsListDb>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                foreach (var quotePart in quoteParts)
                {
                    var param = new DynamicParameters();
                    var listParm = new DynamicParameters();

                    param.Add("@QuoteID", quoteId);
                    param.Add("@QuoteVersionID", versionId);
                    param.Add("@StatusID", quotePart.StatusID);
                    param.Add("@PartNumber", quotePart.PartNumber);
                    param.Add("@Manufacturer", quotePart.Manufacturer);
                    param.Add("@Qty", quotePart.Qty);
                    param.Add("@Price", quotePart.Price);
                    param.Add("@Cost", quotePart.Cost);
                    param.Add("@CommodityID", quotePart.CommodityID);
                    param.Add("@UserID", UserHelper.GetUserId());

                    var res = con.Query("uspQuoteLineSet", param, commandType: CommandType.StoredProcedure);

                    listParm.Add("@QuoteLineID", res.First().QuoteLineID);

                    var setPartListDb = con.Query<PartsListDb>("uspQuoteLinesGet", listParm, commandType: CommandType.StoredProcedure).First();

                    partListDbs.Add(setPartListDb);
                }

                con.Close();
            }

            return partListDbs;
        }

        public int RouteQuoteLines(RouteQuoteLineRequest routeQuoteLineRequest)
        {
            int result;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                if (routeQuoteLineRequest.IsSpecificBuyer)
                {
                    param.Add("@QuoteLinesJSON", JsonConvert.SerializeObject(routeQuoteLineRequest.QuoteLineIds));
                    param.Add("@UsersJSON", JsonConvert.SerializeObject(routeQuoteLineRequest.BuyerIDs));
                    param.Add("@UserID", UserHelper.GetUserId());
                    param.Add("@ret", direction: ParameterDirection.ReturnValue);
                    con.Query("uspQuoteLinesSpecificRoute", param, commandType: CommandType.StoredProcedure);
                    var quoteId = routeQuoteLineRequest.QuoteLineIds[0].QuoteLineID;
                    result = CheckForQuote(quoteId);
                }
                else
                {
                    param.Add("@QuoteLinesJSON", JsonConvert.SerializeObject(routeQuoteLineRequest.QuoteLineIds));
                    param.Add("@UserID", UserHelper.GetUserId());
                    param.Add("@ret", direction: ParameterDirection.ReturnValue);
                    con.Query("uspQuoteLinesAutoRoute", param, commandType: CommandType.StoredProcedure);
                    result = param.Get<int>("@ret");
                    var quoteId = routeQuoteLineRequest.QuoteLineIds[0].QuoteLineID;
                    result = CheckForQuote(quoteId);
                }
                con.Close();
            }
            return result;
        }

        public int CheckForQuote(int quoteId)
        {
            int result;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@QuoteLineID", quoteId);
                result = con.Query<int>("SELECT * FROM mapBuyerQuoteRoutes WHERE IsDeleted = 0 AND QuoteLineID = @QuoteLineID", param, commandType: null).FirstOrDefault();
                con.Close();
            }
            return result;
        }

        public IList<QuoteLineHistoryDb> GetQuoteLineByAccountId(int accountId,int? contactId=null)
        {
            List<QuoteLineHistoryDb> quoteLineDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@ContactID", contactId);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@RowLimit", 999999);

                quoteLineDbs = con.Query<QuoteLineHistoryDb>("uspQuoteLinesHistoryGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return quoteLineDbs;
        }
    }
    
}
