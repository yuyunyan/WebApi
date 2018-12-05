using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.VendorRfqs;
using Sourceportal.Domain.Models.API.Responses.RFQ;
using Sourceportal.Domain.Models.DB.RFQ;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Domain.Models.Shared;
using Sourceportal.Utilities;
using Sourceportal.DB.Items;

namespace Sourceportal.DB.VendorRFQs
{
    public class VendorRfqRepository : IVendorRfqRepository
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;
        private readonly IItemRepository _itemRepository;

        public VendorRfqRepository(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        public RfqDetailsDb GetRfqBasicDetails(int rfqId)
        {
            RfqDetailsDb rfqDetailsDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@rfqId", rfqId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                param.Add("@UserID", UserHelper.GetUserId());

                var res = con.Query<RfqDetailsDb>("uspVendorRfqGet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", RfqDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                if (res.Count() != 1)
                {
                    var errorMessage = string.Format("Database error occured: rfq id: {0} not found.", rfqId);
                    throw new GlobalApiException(errorMessage);
                }
                rfqDetailsDb = res.First();
                con.Close();
            }
            return rfqDetailsDb;
        }

        public IList<RfqLinesDb> GetRfqLines(VendorRfqLinesGetRequest vendorRfqLinesGetRequest)
        {
            IList<RfqLinesDb> rfqLinesDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@RfqLineID", vendorRfqLinesGetRequest.RfqLineId);
                param.Add("@RfqID", vendorRfqLinesGetRequest.RfqId);
                param.Add("@RowOffset", vendorRfqLinesGetRequest.RowOffset);
                param.Add("@RowLimit", vendorRfqLinesGetRequest.RowLimit);
                param.Add("@SortBy", vendorRfqLinesGetRequest.SortBy);
                param.Add("@DescSort", vendorRfqLinesGetRequest.DescSort);
                param.Add("@partnumberstrip", vendorRfqLinesGetRequest.PartNumberStrip);
                param.Add("@statusId", vendorRfqLinesGetRequest.StatusId);
                rfqLinesDb =
                    con.Query<RfqLinesDb>("uspVendorRFQLinesGet", param, commandType: CommandType.StoredProcedure)
                        .ToList();
                con.Close();
            }

            return rfqLinesDb;
        }

	    public IList<RfqLineResponseDb> GetRfqLineResponses(VendorRfqLineResponsesGetRequest vendorRfqLinesGetRequest)
        {
            IList<RfqLineResponseDb> rfqLineResponseDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                
                    param.Add("@VRfqLineID", vendorRfqLinesGetRequest.RfqLineId);
                    param.Add("@RowOffset", vendorRfqLinesGetRequest.RowOffset);
                    param.Add("@RowLimit", vendorRfqLinesGetRequest.RowLimit);
                    param.Add("@SortBy", vendorRfqLinesGetRequest.SortBy);
                    param.Add("@DescSort", vendorRfqLinesGetRequest.DescSort);
                    param.Add("@CommentTypeID", (int)CommentType.SourcesJoin);

                var response = con.Query<RfqLineResponseDb>("uspVendorRfqLineResponsesGet", param,commandType: CommandType.StoredProcedure);
                rfqLineResponseDb = response != null ? response.ToList() : new List<RfqLineResponseDb>();
                con.Close();
            }

            return rfqLineResponseDb;
        }

        public List<RfqDetailsDb> GetAllRfqs(VendorRfqLineResponsesGetRequest request)
        {
            List<RfqDetailsDb> rfqDetailsDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@RfqId", 0);
                param.Add("@SearchString", request.SearchString);
                param.Add("@RowOffset", request.RowOffset);
                param.Add("@RowLimit", request.RowLimit);
                param.Add("@SortBy", request.SortBy);
                param.Add("@DescSort", request.DescSort);
                param.Add("@UserID", UserHelper.GetUserId());

                rfqDetailsDb = con.Query<RfqDetailsDb>("uspVendorRfqGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return rfqDetailsDb;

        }

        public RfqDetailsDb SaveBasicDetails(VendorRfqSaveRequest vendorRfqSaveRequest)
        {
            var rfq = new RfqDetailsDb();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@rfqId", vendorRfqSaveRequest.RfqId);
                param.Add("@accountId", vendorRfqSaveRequest.SupplierId);
                param.Add("@contactId", vendorRfqSaveRequest.ContactId);
                param.Add("@statusId", vendorRfqSaveRequest.StatusId);
                param.Add("@organizationId", vendorRfqSaveRequest.OrganizationId);
                param.Add("@currencyId", vendorRfqSaveRequest.CurrencyId);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                var res = con.Query("uspVendorRfqSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", RfqDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                rfq.VendorRFQID = res.First().rfqId;
                

                con.Close();
            }

            return rfq;
        }

        public RfqLinesDb SaveRfqLine(RfqLineSaveRequest rfqLineSaveRequest)
        {
            var rfqLinesDb = new RfqLinesDb();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                if (rfqLineSaveRequest.IsIHS && rfqLineSaveRequest.ItemId != null)
                {
                    var itemDb = _itemRepository.CreateIhsItemInDb((int)rfqLineSaveRequest.ItemId);
                    rfqLineSaveRequest.ItemId = itemDb.ItemID;
                }

                DynamicParameters param = new DynamicParameters();
                param.Add("@VRfqLineId", rfqLineSaveRequest.VrfqLineId);
                param.Add("@VRfqId", rfqLineSaveRequest.VrfqId);
                param.Add("@CommodityId", rfqLineSaveRequest.CommodityId);
                param.Add("@DateCode", rfqLineSaveRequest.DateCode);
                param.Add("@Manufacturer", rfqLineSaveRequest.Manufacturer);
                param.Add("@Note", rfqLineSaveRequest.Note);
                param.Add("@PackagingId", rfqLineSaveRequest.PackagingId);
                param.Add("@PartNumber", rfqLineSaveRequest.PartNumber);
                param.Add("@ItemId", rfqLineSaveRequest.ItemId);
                param.Add("@Qty", rfqLineSaveRequest.Qty);
                param.Add("@TargetCost", rfqLineSaveRequest.TargetCost);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspVendorRfqLineSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", RfqDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                param = new DynamicParameters();
                param.Add("@RfqLineID", res.First().VRfqLineId);

                rfqLinesDb = con.Query<RfqLinesDb>("uspVendorRFQLinesGet", param, commandType: CommandType.StoredProcedure).First();

                con.Close();
            }
           
            return rfqLinesDb;
        }

        public bool DeleteRfqLines(List<int> rfqLineIds)
        {
            bool status = true;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var rfqLineIdList = rfqLineIds.Select(x => new {RfqLineID = x}).ToList();
                var jsonRfqLineIds = JsonConvert.SerializeObject(rfqLineIdList);

                param.Add("@RFQLinesJSON", jsonRfqLineIds);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                con.Query("uspVendorRfqLinesDelete", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", RfqDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }


                con.Close();
            }
            return status;
        }

        public RfqLineResponseDb SaveRfqLineResponse(RfqLineResponseSaveRequest rfqLineResponseSaveRequest)
        {
            var rfqLineResponseDb = new RfqLineResponseDb();

            if (rfqLineResponseSaveRequest.IsIHS && rfqLineResponseSaveRequest.ItemID != 0)
            {
                var itemDb = _itemRepository.CreateIhsItemInDb((int)rfqLineResponseSaveRequest.ItemID);
                rfqLineResponseSaveRequest.ItemID = itemDb.ItemID;
            }

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SourceId", rfqLineResponseSaveRequest.SourceId);
                param.Add("@VRfqLineId", rfqLineResponseSaveRequest.RfqLineId);
                param.Add("@Cost", rfqLineResponseSaveRequest.Cost);
                param.Add("@DateCode", rfqLineResponseSaveRequest.DateCode);
                param.Add("@Manufacturer", rfqLineResponseSaveRequest.Manufacturer);
                param.Add("@PackagingId", rfqLineResponseSaveRequest.PackagingId);
                param.Add("@PartNumber", rfqLineResponseSaveRequest.PartNumber);
                param.Add("@ItemID", rfqLineResponseSaveRequest.ItemID);
                param.Add("@Qty", rfqLineResponseSaveRequest.OfferQty);
                param.Add("@Moq", rfqLineResponseSaveRequest.Moq);
                param.Add("@Spq", rfqLineResponseSaveRequest.Spq);
                param.Add("@ValidforHours", rfqLineResponseSaveRequest.ValidforHours);
                param.Add("@LeadTimeDays", rfqLineResponseSaveRequest.LeadTimeDays);
                param.Add("@IsNoStock", rfqLineResponseSaveRequest.IsNoStock);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspVendorRfqLineResponseSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", RfqDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                param = new DynamicParameters();
                param.Add("@SourceID", res.First().SourceID);
                rfqLineResponseDb = con.Query<RfqLineResponseDb>("uspVendorRfqLineResponsesGet", param, commandType: CommandType.StoredProcedure).First();
                    
                con.Close();
            }
            return rfqLineResponseDb;
        }

        public bool DeleteRfqLineResponses(List<int> sourceIds, int rfqLineId)
        {
            bool status = true;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var sourcesIdList = sourceIds.Select(x => new { SourceID = x }).ToList();
                var jsonSourceIds = JsonConvert.SerializeObject(sourcesIdList);

                param.Add("@RFQLineRsponsesJSON", jsonSourceIds);
                param.Add("@RFQLineId", rfqLineId);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                con.Query("uspVendorRfqLineResponsesDelete", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", RfqDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }
            return status;
        }

        public List<RfqLinesDb> SetRfqLineList(int rfqId, List<RfqLineSaveRequest> rfqLineList)
        {
            var rfqLinesDb = new List<RfqLinesDb>();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                foreach (var rfqLine in rfqLineList)
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@VRfqId", rfqId);
                    param.Add("@CommodityId", 1);
                    param.Add("@Manufacturer", rfqLine.Manufacturer);
                    param.Add("@PackagingId", 1);
                    param.Add("@PartNumber", rfqLine.PartNumber);
                    param.Add("@Qty", rfqLine.Qty);
                    param.Add("@TargetCost", rfqLine.TargetCost);
                    param.Add("@UserID", UserHelper.GetUserId());
                    param.Add("@ret", direction: ParameterDirection.ReturnValue);

                    var res = con.Query("uspVendorRfqLineSet", param, commandType: CommandType.StoredProcedure);

                    var errorId = param.Get<int>("@ret");
                    if (errorId != 0)
                    {
                        var errorMessage = string.Format("Database error occured: {0}", RfqDbErrors.ErrorCodes[errorId]);
                        throw new GlobalApiException(errorMessage);
                    }
                    param = new DynamicParameters();
                    param.Add("@RfqLineID", res.First().VRfqLineId);

                    var rfqLineDb = con.Query<RfqLinesDb>("uspVendorRFQLinesGet", param, commandType: CommandType.StoredProcedure).First();

                    rfqLinesDb.Add(rfqLineDb);
                }

                con.Close();
            }

            return rfqLinesDb;
        }
    }
}
