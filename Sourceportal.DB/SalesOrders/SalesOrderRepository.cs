using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Items;
using Sourceportal.Domain.Models.DB.SalesOrders;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.API.Responses.SalesOrders;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.DB.SalesOrders
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly IItemRepository _itemRepository;

        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public SalesOrderRepository(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public SalesOrderDetailsDb SetSalesOrderDetails(SalesOrderDetailsRequest request)
        {
            try
            {
                SalesOrderDetailsDb salesOrderDetails = null;
            
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SalesOrderID", request.SalesOrderId);
                    param.Add("@VersionID", request.VersionID);
                    param.Add("@AccountID", request.AccountID);
                    param.Add("@ContactID", request.ContactID);
                    param.Add("@ProjectID", request.ProjectID);
                    param.Add("@StatusID", request.StatusID);
                    param.Add("@IncotermID", request.IncotermID);
                    param.Add("@PaymentTermID", request.PaymentTermID);
                    param.Add("@CurrencyID", request.CurrencyID);
                    param.Add("@ShipLocationID", request.ShipLocationID);
                    param.Add("@ShipFromRegionID", request.ShipFromRegionID);
                    param.Add("@OrganizationID", request.OrganizationID);
                    param.Add("@DeliveryRuleID", request.DeliveryRuleID);
                    param.Add("@UltDestinationID", request.UltDestinationID);
                    param.Add("@FreightPaymentID", request.FreightPaymentID);
                    param.Add("@FreightAccount", request.FreightAccount);
                    param.Add("@OrderDate", request.OrderDate);
                    param.Add("@CustomerPo", request.CustomerPo);
                    param.Add("@UserID", UserHelper.GetUserId());
                    param.Add("@ExternalID", request.ExternalId);
                    param.Add("@ShippingNotes", request.ShippingNotes);
                    param.Add("@QCNotes", request.QCNotes);
                    param.Add("@CarrierID", request.CarrierID);
                    param.Add("@CarrierMethodID", request.CarrierMethodID);
                    param.Add("@ret", direction: ParameterDirection.ReturnValue);

                    //Only returns SalesOrderID and VersionID
                    var res = con.Query("uspSalesOrderSet", param, commandType: CommandType.StoredProcedure);

                    var errorId = param.Get<int>("@ret");
                    if (errorId != 0)
                    {
                        var errorMessage = string.Format("Database error occured: {0}",
                            SalesOrderDbErrors.ErrorCodes[errorId]);
                        throw new GlobalApiException(errorMessage);
                    }

                    param = new DynamicParameters();
                    param.Add("@SalesOrderID", res.First().SalesOrderID);
                    param.Add("@VersionID", request.VersionID);
                    param.Add("@UserID", UserHelper.GetUserId());

                    salesOrderDetails = con.Query<SalesOrderDetailsDb>("uspSalesOrderGet", param, commandType: CommandType.StoredProcedure).First();
                    
                    con.Close();
                }
            

            return salesOrderDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public List<SalesOrderDb> GetSalesOrderList(SearchFilter searchFilter)
        {
            List<SalesOrderDb> salesOrderDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SearchString", searchFilter.SearchString);
                param.Add("@RowOffset", searchFilter.RowOffset);
                param.Add("@RowLimit", searchFilter.RowLimit);
                param.Add("@SortBy", searchFilter.SortCol);
                param.Add("@DescSort", searchFilter.DescSort);
                param.Add("@UserID", UserHelper.GetUserId());

                salesOrderDb = con.Query<SalesOrderDb>("uspSalesOrdersListGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return salesOrderDb;
        }

        public List<SalesOrderLinesDb> GetSalesOrderLines(int salesOrderId, int salesOrderVersionId, SearchFilter searchFilter)
        {
            List<SalesOrderLinesDb> soLinesDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SalesOrderID", salesOrderId);
                param.Add("@SOVersionID", salesOrderVersionId);
                param.Add("@RowOffset", searchFilter.RowOffset);
                param.Add("@RowLimit", searchFilter.RowLimit);
                param.Add("@SortBy", searchFilter.SortCol);
                param.Add("@DescSort", searchFilter.DescSort);
                param.Add("@CommentTypeID", CommentType.SalesOrderLine);
                soLinesDb = con.Query<SalesOrderLinesDb>("uspSalesOrderLinesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return soLinesDb;
        }

        public SalesOrderLinesDb GetSalesOrderLine(int soLineId)
        {
            SalesOrderLinesDb soLineDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SOLineID", soLineId);
                soLineDb = con.Query<SalesOrderLinesDb>("uspSalesOrderLinesGet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return soLineDb;
        }

        public SalesOrderLinesDb SetSalesOrderLines(SalesOrderLineDetail setSalesOrderLinesRequest)
        {
            SalesOrderLinesDb soLineDb;

            if (setSalesOrderLinesRequest.IsIhsItem)
            {
                var itemDb = _itemRepository.CreateIhsItemInDb(setSalesOrderLinesRequest.ItemId);
                setSalesOrderLinesRequest.ItemId = itemDb.ItemID;
            }

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                if(setSalesOrderLinesRequest.LineNum > 0)
                    param.Add("@LineNum", setSalesOrderLinesRequest.LineNum);
                param.Add("@SOLineID", setSalesOrderLinesRequest.SOLineId);
                param.Add("@SalesOrderID", setSalesOrderLinesRequest.SalesOrderId);
                param.Add("@SOVersionID", setSalesOrderLinesRequest.SalesOrderVersionId);
                param.Add("@StatusID", setSalesOrderLinesRequest.StatusId);
                param.Add("@ItemID", setSalesOrderLinesRequest.ItemId);
                param.Add("@CustomerLine", setSalesOrderLinesRequest.CustomerLine);
                param.Add("@CustomerPartNum", setSalesOrderLinesRequest.CustomerPartNum);
                param.Add("@Qty", setSalesOrderLinesRequest.Qty);
                param.Add("@Price", setSalesOrderLinesRequest.Price);
                param.Add("@Cost", setSalesOrderLinesRequest.Cost);
                param.Add("@DateCode", setSalesOrderLinesRequest.DateCode);
                param.Add("@PackagingID", setSalesOrderLinesRequest.PackingId);
                param.Add("@PackageConditionID", setSalesOrderLinesRequest.PackageConditionId);
                param.Add("@DeliveryRuleID", setSalesOrderLinesRequest.DeliveryRuleId);
                param.Add("@ShipDate", setSalesOrderLinesRequest.ShipDate);
                param.Add("@DueDate", setSalesOrderLinesRequest.DueDate);
                param.Add("@IsDeleted", setSalesOrderLinesRequest.IsDeleted);
                param.Add("@ProductSpec", setSalesOrderLinesRequest.ProductSpec);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspSalesOrderLineSet", param, commandType: CommandType.StoredProcedure).First();
                
                int SalesOrderLineId = 0;
                if(setSalesOrderLinesRequest.SOLineId == 0)
                {
                    SalesOrderLineId = res.SOLineID;
                }

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SalesOrderDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                param = new DynamicParameters();
                param.Add("SOLineID", res.SOLineID);
                param.Add("@SOVersionID", setSalesOrderLinesRequest.SalesOrderVersionId);

                soLineDb = con.Query<SalesOrderLinesDb>("uspSalesOrderLinesGet", param, commandType: CommandType.StoredProcedure).First();

                con.Close();
            }

            return soLineDb;
        }

        public void SetSalesOrderLinesSapData(SalesOrderLineDetail setSalesOrderLinesRequest)
        {

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@LineNum", setSalesOrderLinesRequest.LineNum);
                param.Add("@SalesOrderID", setSalesOrderLinesRequest.SalesOrderId);
                param.Add("@SOVersionID", setSalesOrderLinesRequest.SalesOrderVersionId);
                param.Add("@ProductSpec", setSalesOrderLinesRequest.ProductSpec);
                param.Add("@UserID", UserHelper.GetUserId());

                var res = con.Query("uspSalesOrderLineSapDataSet", param, commandType: CommandType.StoredProcedure);

                con.Close();
            }
        }

        public int DeleteSalesOrderLines(List<int> soLineIds)
        {
            int count = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var addSOLineIdProperty = soLineIds.Select(x => new { SOLineID = x }).ToList();
                var jsonSOLineIds = JsonConvert.SerializeObject(addSOLineIdProperty);

                param.Add("@SOLinesJSON", jsonSOLineIds);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<int>("uspSalesOrderLinesDelete", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SalesOrderDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                count = res.First();
                con.Close();
            }
            return count;
        }

        public List<SalesOrderExtraDb> GetSalesOrderExtra(int salesOrderId, int salesOrderVersionId, int rowOffset, int rowLimit)
        {
            List<SalesOrderExtraDb> soExtraDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@SalesOrderID", salesOrderId);
                param.Add("@SOVersionID", salesOrderVersionId);
                param.Add("@RowOffset", rowOffset);
                param.Add("@RowLimit", rowLimit);
                param.Add("@CommentTypeID", CommentType.SalesOrderExtra);

                soExtraDbs = con.Query<SalesOrderExtraDb>("uspSalesOrderExtrasGet", param,
                    commandType: CommandType.StoredProcedure).ToList();


                con.Close();
            }
            return soExtraDbs;
        }

        public SalesOrderExtraDb SetSalesOrderExtra(SetSalesOrderExtraRequest setSalesOrderExtraRequest)
        {
            SalesOrderExtraDb salesOrderExtraDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@SOExtraID", setSalesOrderExtraRequest.SOExtraId);
                param.Add("@SalesOrderID", setSalesOrderExtraRequest.SalesOrderId);
                param.Add("@SOVersionID", setSalesOrderExtraRequest.SOVersionId);
                param.Add("@ItemExtraID", setSalesOrderExtraRequest.ItemExtraId);
                param.Add("@QuoteExtraID", setSalesOrderExtraRequest.QuoteExtraId);
                param.Add("@RefLineNum", setSalesOrderExtraRequest.RefLineNum);
                param.Add("@StatusID", setSalesOrderExtraRequest.StatusId);
                param.Add("@Qty", setSalesOrderExtraRequest.Qty);
                param.Add("@Price", setSalesOrderExtraRequest.Price);
                param.Add("@Cost", setSalesOrderExtraRequest.Cost);
                param.Add("@PrintOnSO", setSalesOrderExtraRequest.PrintOnSO);
                param.Add("@Note", setSalesOrderExtraRequest.Note);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@IsDeleted", setSalesOrderExtraRequest.IsDeleted);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspSalesOrderExtraSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SalesOrderDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                param = new DynamicParameters();
                param.Add("@SOExtraID", res.First().SOExtraID);

                salesOrderExtraDb = con.Query<SalesOrderExtraDb>("uspSalesOrderExtrasGet", param,
                    commandType: CommandType.StoredProcedure).First();

                con.Close();
            }

            return salesOrderExtraDb;
        }

        public int DeleteSalesOrderExtras(List<int> soExtraIds)
        {
            int count = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var addSOExtraIdProperty = soExtraIds.Select(x => new { SOExtraID = x }).ToList();
                var jsonSOExtraIds = JsonConvert.SerializeObject(addSOExtraIdProperty);

                param.Add("@SOExtrasJSON", jsonSOExtraIds);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ResultCount", count);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<int>("uspSalesOrderExtrasDelete", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", SalesOrderDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                count = res.First();

                con.Close();
            }
            return count;
        }

        public SalesOrderDetailsDb GetSalesOrderDetails(int soId, int versionId)
        {
            SalesOrderDetailsDb salesOrderDetailsDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SalesOrderID",soId);
                parm.Add("@VersionID",versionId);
                parm.Add("@UserID", UserHelper.GetUserId());
                

                var res = con
                    .Query<SalesOrderDetailsDb>("uspSalesOrderGet", parm, commandType: CommandType.StoredProcedure);
                if (!res.Any())
                {
                    var errorMessage = string.Format("Database error occured: SalesOrderID {0} VersionID {1} not found", 
                        soId, versionId);
                    throw new GlobalApiException(errorMessage);
                }
                salesOrderDetailsDbs = res.First();
                con.Close();
            }

            return salesOrderDetailsDbs;
        }


        public SalesOrderOrganizationDb GetSalesOrderOrganization(int soId, int versionId)
        {
            SalesOrderOrganizationDb salesOrderOrgDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SalesOrderID", soId);
                parm.Add("@VersionID", versionId);
                parm.Add("@UserID", UserHelper.GetUserId());

                var res = con.Query<SalesOrderOrganizationDb>("uspSalesOrderOrganizationGet", parm, commandType: CommandType.StoredProcedure);
                if (res.Count() > 0)
                    salesOrderOrgDb = res.First();
                else
                    salesOrderOrgDb = null;
                con.Close();
            }

            return salesOrderOrgDb;
        }

        public SalesOrderDetailsDb GetSalesOrderFromExternal(string externalId)
        {
            SalesOrderDetailsDb salesOrderDetailsDbs = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@ExternalID", externalId);

                salesOrderDetailsDbs = con
                    .Query<SalesOrderDetailsDb>("SELECT * FROM SalesOrders WHERE ExternalID=@ExternalID", parm, commandType: null).FirstOrDefault();
                con.Close();
            }

            return salesOrderDetailsDbs;
        }

        public SalesOrderDetailsDb GetSalesOrderFromLine(int soLineId)
        {
            SalesOrderDetailsDb salesOrderDetailsDbs = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SOLineID", soLineId);

                salesOrderDetailsDbs = con
                    .Query<SalesOrderDetailsDb>("SELECT * FROM SalesOrders so " +
                    "INNER JOIN SalesOrderLines sol ON sol.SalesOrderID = so.SalesOrderID WHERE sol.SOLineID=@SOLineID", parm, commandType: null).FirstOrDefault();
                con.Close();
            }

            return salesOrderDetailsDbs;
        }

        public string GetProductSpecId(int soLineId)
        {
            string externalId = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@SOLineID", soLineId);

                var result = con.Query<string>("SELECT ProductSpec FROM SalesOrderLines WHERE SOLineID=@SOLineID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    externalId = result.First();
                }

                con.Close();
            }

            return externalId;
        }

        public int GetSoLineIdFromProductSpec(string productSpec)
        {
            int soLineId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ProductSpec", productSpec);

                var result = con.Query<int>("SELECT SOLineID FROM SalesOrderLines WHERE ProductSpec=@ProductSpec", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    soLineId = result.First();
                }

                con.Close();
            }

            return soLineId;
        }

        public List<SalesOrderLinesDb> GetSoLinesFromProductSpec(string productSpec)
        {
            List<SalesOrderLinesDb> soLines = new List<SalesOrderLinesDb>();
            List<int> soLineIds = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ProductSpec", productSpec);

                soLineIds = con.Query<int>("SELECT SOLineID FROM SalesOrderLines WHERE ProductSpec=@ProductSpec", param, commandType: null).ToList();

                con.Close();
            }

            foreach (var id in soLineIds)
            {
                soLines.Add(GetSalesOrderLine(id));
            }

            return soLines;
        }

        public int GetItemIdFromExternal(string externalId)
        {
            int itemId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", externalId);

                var result = con.Query<int>("SELECT ItemID FROM Items WHERE ExternalID=@ExternalID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    itemId = result.First();
                }

                con.Close();
            }

            return itemId;
        }

        public int GetSoLineIdFromExternal(string soExternalId, string soLineNum)
        {
            int soLineId = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", soExternalId);
                param.Add("@LineNum", Int32.Parse(soLineNum));

                var result = con.Query<int>("SELECT sol.SOLineID FROM SalesOrderLines sol " +
                    "INNER JOIN SalesOrders so ON so.SalesOrderID = sol.SalesOrderID " +
                    "WHERE so.ExternalID = @ExternalID AND sol.LineNum = @LineNum " +
                    "ORDER BY so.VersionID DESC", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    soLineId = result.First();
                }

                con.Close();
            }

            return soLineId;
        }

        public IList<SalesOrderLineHistoryDb> GetSalesOrderLineByAccountId(int accountId,int? contactId=null)
        {
            List<SalesOrderLineHistoryDb> SOLineDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@ContactID", contactId);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@RowLimit", 999999);

                SOLineDbs = con.Query<SalesOrderLineHistoryDb>("uspSalesOrderLinesHistoryGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return SOLineDbs;
        }
    }
    
}
