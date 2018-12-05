using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.BOMs;
using Sourceportal.Domain.Models.API.Responses.BOMs;
using Sourceportal.Domain.Models.DB.BOMs;
using Sourceportal.Domain.Models.DB.PurchaseOrders;
using Sourceportal.Domain.Models.DB.Quotes;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;
using EMSLineBom = Sourceportal.Domain.Models.DB.BOMs.EMSLineBom;
using PurchaseOrderLineBom = Sourceportal.Domain.Models.DB.BOMs.PurchaseOrderLineBom;

namespace Sourceportal.DB.BOMs
{
    public class BOMsRepository : IBOMsRepository
    {
        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public bool SaveXlsDataAccountMap(BOMBody bomBody, List<int> xlsDataMapIdList)
        {
            int rowCount;

            var XLSDataMapObjectList = mappingObjectList(xlsDataMapIdList);

            var jsonXLSDataMap = JsonConvert.SerializeObject(XLSDataMapObjectList);

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                var param = new DynamicParameters();
                param.Add("@XlsAccountMapsJSON", jsonXLSDataMap);
                param.Add("@AccountID", bomBody.AccountID);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@XlsType", bomBody.XlsType);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                var res = con.Query<int>("uspXlsAccountMapSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}, reason: {1}", BOMDbErrors.ErrorCodes[1], BOMDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                rowCount = res.First();

                con.Close();
            }
            return rowCount == XLSDataMapObjectList.Count;
        }

        private List<XLSDataMapObject> mappingObjectList(List<int> xlsDataMapIdList)
        {
            var XLSDataMapObjectList = new List<XLSDataMapObject>();
            for (int idx = 0; idx < xlsDataMapIdList.Count; idx++)
            {
                if (xlsDataMapIdList[idx] == 0)
                {
                    continue;
                }
                var mapObject = new XLSDataMapObject
                {
                    XlsDataMapID = xlsDataMapIdList[idx],
                    ColumnIndex = idx
                };
                XLSDataMapObjectList.Add(mapObject);
            }
            return XLSDataMapObjectList;
        }

        public int UploadListBOMBodySet(BOMBody bomBody, List<ItemListLineBOMRequest> itemListLines)
        {
            int itemListId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                var jsonItemListLines = JsonConvert.SerializeObject(itemListLines);

                var param = new DynamicParameters();
                param.Add("@ListData", jsonItemListLines);
                param.Add("@AccountID", bomBody.AccountID);
                param.Add("@ContactID", bomBody.ContactID);
                param.Add("@SalesUserID", bomBody.SalesUserID);
                param.Add("@CurrencyID", bomBody.CurrencyId);
                param.Add("@ListName", bomBody.ListName);
                param.Add("@ListTypeID", bomBody.ListTypeID);
                param.Add("@SourceTypeID", bomBody.SourcingTypeID);
                param.Add("@PublishToSources", bomBody.PublishToSources);
                param.Add("@QuoteTypeID" , bomBody.QuoteTypeId);
                param.Add("@UserID", UserHelper.GetUserId());

                var res = con.Query<int>("uspItemListIns", param, commandType: CommandType.StoredProcedure);

                if (!res.Any())
                {
                    var errorMessage = string.Format("Database error occured: {0}", BOMDbErrors.ErrorCodes[4]);
                    throw new GlobalApiException(errorMessage);
                }

                itemListId = res.First();

                con.Close();
            }
            return itemListId;
        }

        public int UploadListExcessBodySet(BOMBody bomBody, List<ItemListLineExcessRequest> itemListLines)
        {
            int itemListId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                var jsonItemListLines = JsonConvert.SerializeObject(itemListLines);

                var param = new DynamicParameters();
                param.Add("@ListData", jsonItemListLines);
                param.Add("@AccountID", bomBody.AccountID);
                param.Add("@ContactID", bomBody.ContactID);
                param.Add("@SalesUserID", bomBody.SalesUserID);
                param.Add("@CurrencyID", bomBody.CurrencyId);
                param.Add("@ListName", bomBody.ListName);
                param.Add("@ListTypeID", bomBody.ListTypeID);
                param.Add("@SourceTypeID", bomBody.SourcingTypeID);
                param.Add("@PublishToSources", bomBody.PublishToSources);
                param.Add("@UserID", UserHelper.GetUserId());

                var res = con.Query<int>("uspItemListIns", param, commandType: CommandType.StoredProcedure);

                if (!res.Any())
                {
                    var errorMessage = string.Format("Database error occured: {0}", BOMDbErrors.ErrorCodes[4]);
                    throw new GlobalApiException(errorMessage);
                }

                itemListId = res.First();

                con.Close();
            }
            return itemListId;
        }
        public List<BOMListDbs> GetBOMList(SearchFilter searchFilter)
        {
            List<BOMListDbs> bomList;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                var param = new DynamicParameters();
                param.Add("@SearchString", searchFilter.SearchString);
                param.Add("@RowOffset", searchFilter.RowOffset);
                param.Add("@RowLimit", searchFilter.RowLimit);
                param.Add("@SortBy", searchFilter.SortCol);
                param.Add("@DescSort", searchFilter.DescSort);
                param.Add("@ObjectTypeID", ObjectType.ItemList);
                param.Add("@UserID", UserHelper.GetUserId());

                bomList = con.Query<BOMListDbs>("uspBOMListGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                con.Close();
            }

            return bomList;
        }

        public int ProcessMatch(ProcessMatchRequest processMatchRequest)
        {
            var searchId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
          
                DynamicParameters parm = new DynamicParameters();
                var addPartNoProperty = processMatchRequest.PartNumbers.Select(x => new { PartNumber = x}).ToList();
                var jsonPartNumbers = JsonConvert.SerializeObject(addPartNoProperty);

                parm.Add("@ItemListID", processMatchRequest.ItemListId);
                parm.Add("@PartNumbers", jsonPartNumbers);
                parm.Add("@Manufacturer", processMatchRequest.Manufacturer);
                parm.Add("@AccountName", processMatchRequest.Account);
                parm.Add("@SearchType", processMatchRequest.SearchType);
                parm.Add("@DateStart", processMatchRequest.DateStart);
                parm.Add("@DateEnd", processMatchRequest.DateEnd);
                parm.Add("@MatchQuote", processMatchRequest.MatchQuote);
                parm.Add("@MatchSO", processMatchRequest.MatchSo);
                parm.Add("@MatchPO", processMatchRequest.MatchPo);
                parm.Add("@MatchInventory", processMatchRequest.MatchInventory);
                parm.Add("@MatchOffers", processMatchRequest.MatchOffers);
                parm.Add("@MatchRFQ", processMatchRequest.MatchRfq);
                parm.Add("@MatchBOM", processMatchRequest.MatchBom);
                parm.Add("@MatchCustomerQuote", processMatchRequest.MatchCustomerQuote);
                parm.Add("@UserID", UserHelper.GetUserId());

                var result = con.Query("uspBOMProcessMatch", parm, commandType: CommandType.StoredProcedure, commandTimeout: 600); //timeout: 10 min

                if (!result.Any())
                {
                    var errorMessage = string.Format("Database error occured: {0}", BOMDbErrors.ErrorCodes[5]);
                    throw new GlobalApiException(errorMessage);
                }

                searchId = result.First().SearchID;
                con.Close();
            }
            return searchId;

        }

        public IList<SalesOrderDbs> GetSalesOrder(BomSearchRequest bomSearchRequest)
        {
            IList<SalesOrderDbs> salesOrderDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy",bomSearchRequest.SortCol);
                parm.Add("@DescSort",bomSearchRequest.DescSort);
                salesOrderDbs = con
                    .Query<SalesOrderDbs>("uspBOMSalesOrderLinesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return salesOrderDbs;
        }

        public IList<InventoryDbs> GetInventory(BomSearchRequest bomSearchRequest)
        {
            IList<InventoryDbs> invDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy", bomSearchRequest.SortCol);
                parm.Add("@DescSort", bomSearchRequest.DescSort);
                invDbs = con
                    .Query<InventoryDbs>("uspBOMInventoryLinesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return invDbs;
        }

        public IList<OutsideOffersDbs> GetOutsideOffers(BomSearchRequest bomSearchRequest)
        {
            IList<OutsideOffersDbs> ooDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy", bomSearchRequest.SortCol);
                parm.Add("@DescSort", bomSearchRequest.DescSort);
                ooDbs = con
                    .Query<OutsideOffersDbs>("uspBOMOutsideOffersLinesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return ooDbs;
        }

        public IList<VendorQuotesDbs> GetVendorQuotes(BomSearchRequest bomSearchRequest)
        {
            IList<VendorQuotesDbs> vqDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy", bomSearchRequest.SortCol);
                parm.Add("@DescSort", bomSearchRequest.DescSort);
                vqDbs = con
                    .Query<VendorQuotesDbs>("uspBOMVendorQuotesLinesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return vqDbs;
        }

        public IList<PurchaseOrderLineBom> GetPurchaseOrders(BomSearchRequest bomSearchRequest)
        {
            IList<PurchaseOrderLineBom> purchaseOrderLineList;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy", bomSearchRequest.SortCol);
                parm.Add("@DescSort", bomSearchRequest.DescSort);

                purchaseOrderLineList = con
                    .Query<PurchaseOrderLineBom>("uspBOMPurchaseOrderLinesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return purchaseOrderLineList;
        }

        public IList<CustomerRFQLineBom> GetCustomerQuotes(BomSearchRequest bomSearchRequest)
        {
            IList<CustomerRFQLineBom> customerQuoteLineList;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy", bomSearchRequest.SortCol);
                parm.Add("@DescSort", bomSearchRequest.DescSort);

                customerQuoteLineList = con
                    .Query<CustomerRFQLineBom>("uspBOMCustomerQuotesLinesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return customerQuoteLineList;
        }

        public IList<CustomerRFQLineBom> GetCustomerRfqs(BomSearchRequest bomSearchRequest)
        {
            IList<CustomerRFQLineBom> customerRfqLineList;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy", bomSearchRequest.SortCol);
                parm.Add("@DescSort", bomSearchRequest.DescSort);

                customerRfqLineList = con
                    .Query<CustomerRFQLineBom>("uspBOMCustomerRFQsLinesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return customerRfqLineList;
        }

        public IList<ResultSummaryDbs> GetResultSummary(BomSearchRequest bomSearchRequest)
        {

            IList<ResultSummaryDbs> resultSummaryList;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy", bomSearchRequest.SortCol);
                parm.Add("@DescSort", bomSearchRequest.DescSort);

                resultSummaryList = con
                    .Query<ResultSummaryDbs>("uspBOMResultsSummaryGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return resultSummaryList;
        }

        public IList<EMSLineBom> GetEMSs(BomSearchRequest bomSearchRequest)
        {
            IList<EMSLineBom> emsLineList;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchID", bomSearchRequest.SearchId);
                parm.Add("@RowOffset", bomSearchRequest.RowOffset);
                parm.Add("@RowLimit", bomSearchRequest.RowLimit);
                parm.Add("@SortBy", bomSearchRequest.SortCol);
                parm.Add("@DescSort", bomSearchRequest.DescSort);

                emsLineList = con
                    .Query<EMSLineBom>("uspBOMRecordLinesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return emsLineList;
        }

        public IList<BomSearchResultDbs> GetPartSearchResult(string searchString, string searchType)
        {
            IList<BomSearchResultDbs> searchResultList;
            using(var con= new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@SearchString", searchString);
                parm.Add("@SearchType", searchType);

                searchResultList = con.Query<BomSearchResultDbs>("uspItemSearchTotalsGet", parm, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return searchResultList;
        }

        public IList<AvailabilityPartDbs> GetAvailabilityPart(int itemId)
        {
            IList<AvailabilityPartDbs> availabilityPartList;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();
                
                parm.Add("@ItemID",itemId);
                parm.Add("@RowLimit", 999999);
                availabilityPartList = con.Query<AvailabilityPartDbs>("uspAvailableInvPOGet", parm, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return availabilityPartList;
        }
    }
}
