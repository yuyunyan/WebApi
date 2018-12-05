using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.DB.Sourcing;
using Sourceportal.Domain.Models.API.Requests.Sourcing;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses.Quotes;
using Sourceportal.Domain.Models.API.Responses.Sourcing;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;
using Sourceportal.DB.Items;

namespace Sourceportal.DB.Sourcing
{
    public class SourcingRepository : ISourcingRepository
    {
        private readonly IItemRepository _itemRepository;

        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public SourcingRepository(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        public List<SourcingQuoteListDb> GetSourcingQuoteList(SourcingQuoteLinesFilter sourceFilter)
        {
            List<SourcingQuoteListDb> sourcingQuoteListDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@RouteStatusID", sourceFilter.StatusId);
                param.Add("@RowOffset", sourceFilter.RowOffset);
                param.Add("@RowLimit", sourceFilter.RowLimit);
                param.Add("@SortBy", sourceFilter.SortCol);
                param.Add("@DescSort", sourceFilter.DescSort);
                param.Add("@FilterBy", sourceFilter.FilterBy);
                param.Add("@FilterText", sourceFilter.FilterText);
                param.Add("@CommentTypeID", CommentType.QuotePart);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                sourcingQuoteListDb = con.Query<SourcingQuoteListDb>("uspQuoteLinesForSourcingGet", param,
                    commandType: CommandType.StoredProcedure).ToList();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SourcingDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                con.Close();
            }

            return sourcingQuoteListDb;
        }

        public List<SourcingStatusesDb> GetSourcingStatuses()
        {
            List<SourcingStatusesDb> sourcingStatusesListDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                sourcingStatusesListDb = con.Query<SourcingStatusesDb>("uspSourcingStatusesGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return sourcingStatusesListDb;
        }

        public List<RouteStatusDb> GetRouteStatuses()
        {
            List<RouteStatusDb> routeStatusDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", UserHelper.GetUserId());
                routeStatusDbs = con.Query<RouteStatusDb>("uspRouteStatusesForUserGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return routeStatusDbs;
        }

        public List<SourceTypesDb> GetSourceTypes()
        {
            List<SourceTypesDb> sourceTypesDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                sourceTypesDb = con.Query<SourceTypesDb>("uspSourceTypesGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return sourceTypesDb;
        }

        public List<SourceListDb> GetSourceList(int itemId, string partNumber, int objectId, int objectTypeId , bool showAll, bool showInventory)
        {
            List<SourceListDb> sourceListDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ItemID", itemId);
                param.Add("@PartNumber", partNumber);
                param.Add("@MatchedObjectID", objectId);
                param.Add("@MatchedObjectTypeID", objectTypeId);
                param.Add("@CommentTypeID", CommentType.Sourcing);
                param.Add("@ShowAll", showAll);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                param.Add("@ShowInventory", showInventory);

                sourceListDbs = con.Query<SourceListDb>("uspSourcesGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SourcingDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }
            return sourceListDbs;
        }

        public SourceListDb SetSource(SetSourceRequest setSourceRequest)
        {
            SourceListDb setSourceDbs;

            if (setSourceRequest.IsIhsItem)
            {
                var itemDb = _itemRepository.CreateIhsItemInDb(setSourceRequest.ItemId);
                setSourceRequest.ItemId = itemDb.ItemID;
            }

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@SourceID", setSourceRequest.SourceId);
                param.Add("@SourceTypeID", setSourceRequest.SourceTypeId);
                param.Add("@ItemID", setSourceRequest.ItemId);
                param.Add("@PartNumber", setSourceRequest.PartNumber);
                param.Add("@CommodityID", setSourceRequest.CommodityId);
                param.Add("@AccountID", setSourceRequest.AccountId);
                param.Add("@Qty", setSourceRequest.Qty);
                param.Add("@Cost", setSourceRequest.Cost);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@CurrencyID", setSourceRequest.CurrencyId);
                param.Add("@DateCode", setSourceRequest.DateCode);
                param.Add("@ContactID", setSourceRequest.ContactId);
                param.Add("@Manufacturer", setSourceRequest.Manufacturer);
                param.Add("@PackagingID", setSourceRequest.PackagingId);
                param.Add("@MOQ", setSourceRequest.MOQ);
                param.Add("@SPQ", setSourceRequest.SPQ);
                param.Add("@LeadTimeDays", setSourceRequest.LeadTimeDays);
                param.Add("@ValidForHours", setSourceRequest.ValidForHours);
                param.Add("@IsNoStock", 0);
                param.Add("@RTBQty", setSourceRequest.RtbQty);
                param.Add("@IsDeleted", setSourceRequest.IsDeleted);
                param.Add("@RequestToBuy", setSourceRequest.RequestToBuy);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<SourceListDb>("uspSourceSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SourcingDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                if (!res.Any())
                {
                    var errorMessage = string.Format("Database error occured: Source ID {0} not found",
                        setSourceRequest.SourceId);
                    throw new GlobalApiException(errorMessage);
                }

                setSourceDbs = res.First();
                con.Close();
            }
            return setSourceDbs;
        }

        public bool SetSourceStatus(SetSourceStatus setSourceStatus)
        {
            int count = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@SourceID", setSourceStatus.SourceId);
                param.Add("@ObjectID", setSourceStatus.ObjectId);
                param.Add("@ObjectTypeID", setSourceStatus.ObjectTypeId);
                param.Add("@IsMatch", setSourceStatus.IsMatch);
                param.Add("@IsDeleted", !setSourceStatus.IsJoined);
                param.Add("@Qty", setSourceStatus.RTPQty);
                param.Add("@UserId", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                con.Query("uspSourceJoinSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SourcingDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                con.Close();
            }
            return true;
        }

        public int SetBuyerRoute(SetBuyerRouteRequest setBuyerRouteRequest)
        {
            int rowCount;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@RouteStatusID", setBuyerRouteRequest.RouteStatusID);
                var quoteLineJson = BuildQuoteLineJson(setBuyerRouteRequest.QuoteLines);
                param.Add("@QuoteLinesJSON", JsonConvert.SerializeObject(quoteLineJson));
                rowCount = con.Query<int>("uspMapBuyerRoutesSet", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return rowCount;
        }

        private static List<QuoteLineJSON> BuildQuoteLineJson(List<BuyerQuoteLine> buyerQuoteLines)
        {
            return buyerQuoteLines.Select(buyerQuoteLine => new QuoteLineJSON
                {
                    QuoteLineID = buyerQuoteLine.QuoteLineID
                })
                .ToList();
        }

        public List<BuyerNameDb> QuoteLineRouteBuyersGet(int quoteLineId)
        {
            List<BuyerNameDb> buyerNameDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@QuoteLineID", quoteLineId);
                buyerNameDbs = con.Query<BuyerNameDb>("uspQuoteLineRouteBuyersGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return buyerNameDbs;
        }

        public SourceCommentUIDDb GetSourceCommentUID(SourceCommentUIDRequest sourceCommentUIdRequest)
        {
            SourceCommentUIDDb commentUIdDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@ObjectID", sourceCommentUIdRequest.ObjectID);
                param.Add("@ObjectTypeID", sourceCommentUIdRequest.ObjectTypeID);
                param.Add("@SourceID", sourceCommentUIdRequest.SourceID);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con
                    .Query<SourceCommentUIDDb>("uspGetSourceCommentUID", param,
                        commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SourcingDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                var tempResult = res.FirstOrDefault();

                if (tempResult != null)
                {
                    commentUIdDb = tempResult;
                }
                else
                {
                    commentUIdDb = new SourceCommentUIDDb();
                    commentUIdDb.CommentUID = -1;
                }

                con.Close();
            }

            return commentUIdDb;
        }
        public int SourceToPurchaseOrder(SourceToPORequest request)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                var jsonLineIds = JsonConvert.SerializeObject(request.LinesToCopy);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@AccountID", request.AccountID);
                param.Add("@ContactID", request.ContactID);
                param.Add("@ShipFromLocationID", request.ShipFrom);
                param.Add("@ShipToLocationID", request.ShipTo);
                param.Add("@PaymentTermID", request.PaymentTermID);
                param.Add("@IncotermID", request.IncotermID);
                param.Add("@LinesToCopyJSON", jsonLineIds);

                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con
                    .Query<int>("uspSourceToPO", param,
                        commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                return errorId;
            }
        }

        public List<SourceListDb> GetSourceByIds(PartDetails[] sourceLines)
        {
            var sourceListDbs = new List<SourceListDb>();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                foreach (var sourceLine in sourceLines)
                {
                    DynamicParameters param = new DynamicParameters();

                    param.Add("@SourceId", sourceLine.SourceID);
                    var souceDb = con
                        .Query<SourceListDb>("uspSourcesGet", param, commandType: CommandType.StoredProcedure)
                        .FirstOrDefault();
                    //if (souceDb != null)
                    //{
                    //    souceDb.Qty = sourceLine.Quantity;
                    //    souceDb.ItemId = sourceLine.ItemId;
                    //    souceDb.IsIhs = sourceLine.IsIhs;
                    sourceListDbs.Add(souceDb);
                    //}
                }

                con.Close();
            }
            return sourceListDbs;
        }

        public List<SourceLineDb> GetSourceLineByAccountId(int accountId,int? contactId=null)
        {
            List<SourceLineDb> SourceLineDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@ContactID", contactId);
                param.Add("@RowLimit", 999999);

                SourceLineDbs = con.Query<SourceLineDb>("uspSourcesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return SourceLineDbs;
        }
    }

    }
