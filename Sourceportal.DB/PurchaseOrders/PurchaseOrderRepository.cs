using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Items;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.PurchaseOrders;
using Sourceportal.Domain.Models.API.Requests.Quotes;
using Sourceportal.Domain.Models.API.Responses.PurchaseOrders;
using Sourceportal.Domain.Models.DB.PurchaseOrders;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;

namespace Sourceportal.DB.PurchaseOrders
{   
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly IItemRepository _itemRepository;

        public PurchaseOrderRepository(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public List<CurrencyDb> GetCurrencies()
        {
            List<CurrencyDb> currenciesDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                currenciesDb = con.Query<CurrencyDb>("uspCurrenciesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return currenciesDb;
        }
        
        public PurchaseOrderDb SetPurchaseOrderDetails(SetPurchaseOrderDetailsRequest setPoRequest)
        {
            try
            {
                PurchaseOrderDb purchaseOrderDb;
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@PurchaseOrderID", setPoRequest.PurchaseOrderId);
                    param.Add("@VersionID", setPoRequest.VersionId);
                    param.Add("@AccountID", setPoRequest.AccountId);
                    param.Add("@ContactID", setPoRequest.ContactId);
                    param.Add("@StatusID", setPoRequest.StatusId);
                    param.Add("@FromLocationID", setPoRequest.FromLocationId);
                    param.Add("@ToWarehouseID", setPoRequest.ToWarehouseID);
                    param.Add("@PaymentTermID", setPoRequest.PaymentTermId);
                    param.Add("@CurrencyID", setPoRequest.CurrencyId);
                    param.Add("@ShippingMethodID", setPoRequest.ShippingMethodId);
                    param.Add("@OrderDate", setPoRequest.OrderDate);
                    param.Add("@IsDeleted", setPoRequest.IsDeleted);
                    param.Add("@IncotermID", setPoRequest.IncotermId);
                    param.Add("@OrganizationID", setPoRequest.OrganizationId);
                    param.Add("@ExternalID", setPoRequest.ExternalId);
                    param.Add("@PONotes", setPoRequest.PONotes);
                    param.Add("@UserID", UserHelper.GetUserId());

                    param.Add("@ret", direction: ParameterDirection.ReturnValue);

                    var res = con.Query("uspPurchaseOrderSet", param, commandType: CommandType.StoredProcedure);

                    var errorId = param.Get<int>("@ret");
                    if (errorId != 0)
                    {
                        var errorMessage = string.Format("Database error occured: {0}", PODbErrors.ErrorCodes[errorId]);
                        throw new GlobalApiException(errorMessage);
                    }

                    if (!res.Any())
                    {
                        var errorMessage = string.Format(
                            "Database error occured: Purchase Order ID {0} VersionID {1} not found",
                            setPoRequest.PurchaseOrderId, setPoRequest.VersionId);
                        throw new GlobalApiException(errorMessage);
                    }

                    param = new DynamicParameters();
                    param.Add("@PurchaseOrderID", res.First().PurchaseOrderID);
                    param.Add("@VersionID", res.First().VersionID);
                    param.Add("@UserID", UserHelper.GetUserId());

                    purchaseOrderDb = con.Query<PurchaseOrderDb>("uspPurchaseOrderGet", param,
                        commandType: CommandType.StoredProcedure).First();
                    con.Close();
                }

                return purchaseOrderDb;
            }
            catch (Exception ex)
            {
                throw new GlobalApiException(ex.Message);
            }
        }

        public PurchaseOrderDb GetPurchaseOrderDetails(int purchaseOrderId, int versionId)
        {
            PurchaseOrderDb purchaseOrderDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PurchaseOrderID", purchaseOrderId);
                param.Add("@VersionID", versionId);
                param.Add("@UserID", UserHelper.GetUserId());

                var res = con.Query<PurchaseOrderDb>("uspPurchaseOrderGet", param, commandType: CommandType.StoredProcedure);

                if (!res.Any())
                {
                    var errorMessage = string.Format("Database error occured: Purchase Order ID {0} VersionID {1} not found",
                        purchaseOrderId, versionId);
                    throw new GlobalApiException(errorMessage);
                }
                purchaseOrderDb = res.First();
                con.Close();
            }

            return purchaseOrderDb;
        }

        public PurchaseOrderDb GetPurchaseOrderFromExternal(string externalId)
        {
            PurchaseOrderDb purchaseOrderDetailsDb = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@ExternalID", externalId);

                purchaseOrderDetailsDb = con
                    .Query<PurchaseOrderDb>("SELECT * FROM PurchaseOrders WHERE ExternalID=@ExternalID", parm, commandType: null).FirstOrDefault();
                con.Close();
            }

            return purchaseOrderDetailsDb;
        }

        public PurchaseOrderDb GetPurchaseOrderDetailsFromLineId(int poLineId)
        {
            PurchaseOrderDb purchaseOrderDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PoLineID", poLineId);

                purchaseOrderDb = con.Query<PurchaseOrderDb>(
                    "SELECT po.PurchaseOrderID, po.VersionID FROM PurchaseOrders po " +
                    "INNER JOIN PurchaseOrderLines pol ON pol.PurchaseOrderID = po.PurchaseOrderID " +
                    "WHERE pol.POLineID = @PoLineID"
                    , param, commandType: null).FirstOrDefault();
                
                con.Close();
            }

            return purchaseOrderDb;
        }

        public List<PurchaseOrderDb> GetPurchaseOrderList(SearchFilter searchFilter)
        {
            List<PurchaseOrderDb> purchaseOrderDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SearchString", searchFilter.SearchString);
                param.Add("@RowOffset", searchFilter.RowOffset);
                param.Add("@RowLimit", searchFilter.RowLimit);
                param.Add("@SortBy", searchFilter.SortCol);
                param.Add("@IncludeComplete", 1);
                param.Add("@IncludeCanceled", 1);
                param.Add("@DescSort", searchFilter.DescSort ? 1 : 0);
                param.Add("@UserID", UserHelper.GetUserId());

                purchaseOrderDb= con.Query<PurchaseOrderDb>("uspPurchaseOrdersListGet", param, commandType: CommandType.StoredProcedure).ToList();

                con.Close();
            }

            return purchaseOrderDb;
        }

        public List<PurchaseOrderLinesDb> GetPurchaseOrderLines(int purchaseOrderId, int purchaseOrderVersionId, SearchFilter searchFilter)
        {
            List<PurchaseOrderLinesDb> poLinesDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PurchaseOrderID", purchaseOrderId);
                param.Add("@POVersionID", purchaseOrderVersionId);
                param.Add("@POLineID", searchFilter.PoLineId);
                param.Add("@RowOffset", searchFilter.RowOffset);
                param.Add("@RowLimit", searchFilter.RowLimit);
                param.Add("@SortBy", searchFilter.SortCol);
                param.Add("@DescSort", searchFilter.DescSort);
                param.Add("@CommentTypeID", CommentType.PurchasingLine);
                poLinesDb = con.Query<PurchaseOrderLinesDb>("uspPurchaseOrderLinesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return poLinesDb;
        }

        public string GetProductSpecForPoLine(int poLineId)
        {
            string productSpec;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PoLineID", poLineId);

                productSpec = con.Query<string>(
                    "SELECT sol.productSpec FROM salesorderlines sol " +
                    "INNER JOIN mapSOPOAllocation map ON map.SOLineID = sol.SOLineID " +
                    "WHERE map.POLineID = @PoLineID AND map.IsDeleted = 0"
                    , param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return productSpec;
        }

        public PurchaseOrderLinesDb SetPurchaseOrderLine(SetPurchaseOrderLineRequest setPurchaseOrderLineRequest)
        {
            PurchaseOrderLinesDb purchaseOrderLineDb;

            if (setPurchaseOrderLineRequest.IsIhsItem)
            {
                var itemDb = _itemRepository.CreateIhsItemInDb(setPurchaseOrderLineRequest.ItemId);
                setPurchaseOrderLineRequest.ItemId = itemDb.ItemID;
            }

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@POLineID", setPurchaseOrderLineRequest.POLineId);
                param.Add("@POVersionID", setPurchaseOrderLineRequest.POVersionId);
                param.Add("@ItemID", setPurchaseOrderLineRequest.ItemId);
                param.Add("@PurchaseOrderID", setPurchaseOrderLineRequest.PurchaseOrderId);
                param.Add("@VendorLine", setPurchaseOrderLineRequest.VendorLine);
                param.Add("@StatusID", setPurchaseOrderLineRequest.StatusId);
                param.Add("@Qty", setPurchaseOrderLineRequest.Qty);
                param.Add("@Cost", setPurchaseOrderLineRequest.Cost);
                param.Add("@DateCode", setPurchaseOrderLineRequest.DateCode);
                param.Add("@PackagingID", setPurchaseOrderLineRequest.PackagingId);
                param.Add("@PackageConditionID", setPurchaseOrderLineRequest.PackageConditionID);
                param.Add("@DueDate", setPurchaseOrderLineRequest.DueDate);
                param.Add("@ToWarehouseID", setPurchaseOrderLineRequest.ToWarehouseID);
                //param.Add("@PromisedDate", setPurchaseOrderLineRequest.PromisedDate);
                param.Add("@DateCode", setPurchaseOrderLineRequest.DateCode);
                param.Add("@IsSpecBuy", setPurchaseOrderLineRequest.IsSpecBuy);
                param.Add("@SpecBuyForUserId", setPurchaseOrderLineRequest.SpecBuyForUserID);
                param.Add("@SpecBuyForAccountId", setPurchaseOrderLineRequest.SpecBuyForAccountID);
                param.Add("@SpecBuyReason", setPurchaseOrderLineRequest.SpecBuyReason);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ClonedFromID", setPurchaseOrderLineRequest.ClonedFromID);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                param.Add("@IsDeleted", setPurchaseOrderLineRequest.IsDeleted);

                if(setPurchaseOrderLineRequest.LineNum != null) 
                    param.Add("@LineNum", setPurchaseOrderLineRequest.LineNum);
                if (setPurchaseOrderLineRequest.LineRev != null)
                    param.Add("@LineRev", setPurchaseOrderLineRequest.LineRev);


                var res = con.Query<PurchaseOrderLinesDb>("uspPurchaseOrderLineSet", param,
                    commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", PODbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                var id = res.FirstOrDefault().POLineId;
                param = new DynamicParameters();
                param.Add("@POLineID", id);

                purchaseOrderLineDb = con.Query<PurchaseOrderLinesDb>("uspPurchaseOrderLinesGet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                con.Close();
            }

            return purchaseOrderLineDb;
        }

        public int DeletePurchaseOrderLines(List<int> poLineIds)
        {
            int count = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var addPOLineIdProperty = poLineIds.Select(x => new { POLineID = x }).ToList();
                var jsonPOLineIds = JsonConvert.SerializeObject(addPOLineIdProperty);

                param.Add("@POLinesJSON", jsonPOLineIds);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ResultCount", count);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspPurchaseOrderLinesDelete", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", PODbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                count = res.First().ResultCount;
                con.Close();
            }
            return count;
        }

        public List<PurchaseOrderExtraDb> GetPurchaseOrderExtras(int poId, int poVersionId,
            int rowOffset, int rowLimit)
        {
            List<PurchaseOrderExtraDb> poExtrasDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PurchaseOrderID", poId);
                param.Add("@POVersionID", poVersionId);
                param.Add("@RowOffset", rowOffset);
                param.Add("@RowLimit", rowLimit);
                param.Add("@CommentTypeID", CommentType.PurchasingExtra);
                poExtrasDb = con.Query<PurchaseOrderExtraDb>("uspPurchaseOrderExtrasGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return poExtrasDb;
        }

        public PurchaseOrderExtraDb SetPurchaseOrderExtra(SetPurchaseOrderExtraRequest setPurchaseOrderExtraRequest)
        {
            PurchaseOrderExtraDb purchaseOrderExtraDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@POExtraID", setPurchaseOrderExtraRequest.POExtraId);
                param.Add("@POVersionID", setPurchaseOrderExtraRequest.POVersionId);
                param.Add("@ItemExtraID", setPurchaseOrderExtraRequest.ItemExtraId);
                param.Add("@PurchaseOrderID", setPurchaseOrderExtraRequest.PurchaseOrderId);
                param.Add("@RefLineNum", setPurchaseOrderExtraRequest.RefLineNum);
                param.Add("@StatusID", setPurchaseOrderExtraRequest.StatusId);
                param.Add("@Qty", setPurchaseOrderExtraRequest.Qty);
                param.Add("@Cost", setPurchaseOrderExtraRequest.Cost);
                param.Add("@PrintOnPO", setPurchaseOrderExtraRequest.PrintOnPO);
                param.Add("@Note", setPurchaseOrderExtraRequest.Note);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@IsDeleted", setPurchaseOrderExtraRequest.IsDeleted);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<PurchaseOrderExtraDb>("uspPurchaseOrderExtraSet", param,
                    commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", PODbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                param = new DynamicParameters();
                param.Add("@POExtraID", res.First().POExtraID);

                purchaseOrderExtraDb = con.Query<PurchaseOrderExtraDb>("uspPurchaseOrderExtrasGet", param,
                    commandType: CommandType.StoredProcedure).First();

                con.Close();
            }

            return purchaseOrderExtraDb;
        }

        public int DeletePurchaseOrderExtras(List<int> poExtraIds)
        {
            int count = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var addPOExtraIdProperty = poExtraIds.Select(x => new { POExtraID = x }).ToList();
                var jsonPOExtraIds = JsonConvert.SerializeObject(addPOExtraIdProperty);

                param.Add("@POExtrasJSON", jsonPOExtraIds);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ResultCount", count);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspPurchaseOrderExtrasDelete", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", PODbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                count = res.First().ResultCount;
                con.Close();
            }
            return count;
        }

        public List<PurchaseOrderLinesDb> SetPoLines(int poId, int versionId, List<SetPurchaseOrderLineRequest> poLines)
        {
            var poLinesDb = new List<PurchaseOrderLinesDb>();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                foreach (var poLine in poLines)
                {
                    var param = new DynamicParameters();
                    var listParm = new DynamicParameters();

                    param.Add("@POVersionID", versionId);
                    param.Add("@ItemID", poLine.ItemId);
                    param.Add("@PurchaseOrderID", poId);
                    param.Add("@Qty", poLine.Qty);
                    param.Add("@Cost", poLine.Cost);
                    param.Add("@UserID", UserHelper.GetUserId());
                    param.Add("@ret", direction: ParameterDirection.ReturnValue);

                    var res = con.Query("uspPurchaseOrderLineSet", param, commandType: CommandType.StoredProcedure);

                    listParm.Add("@POLineID", res.First().POLineID);

                    var setLineListDb = con.Query<PurchaseOrderLinesDb>("uspPurchaseOrderLinesGet", listParm, commandType: CommandType.StoredProcedure).First();

                    poLinesDb.Add(setLineListDb);
                }

                con.Close();
            }
            return poLinesDb;
        }

        public string GetManufactuerItem(int itemId)
        {
            string mfr;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ItemID", itemId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                var res = con.Query("uspGetMfrItem", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", PODbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                mfr = res.First().MfrName;

                con.Close();
            }
            return mfr;
        }

        public void SetExternalId(int poId, string externalId)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@PurchaseOrderID", poId);
                param.Add("@ExternalId", externalId);
                param.Add("@UserID", UserHelper.GetUserId());

                con.Query("uspPurchaseOrderSapDataSet", param,
                    commandType: CommandType.StoredProcedure);
                con.Close();
            }
        }

        public int GetPoLineIdFromExternal(string poExternalId, string poLineNum)
        {
            int poLineId = 0, lineNum, lineRev;

            if (poLineNum.Contains("."))
            {
                lineNum = Int32.Parse(poLineNum.Split('.').First());
                lineRev = Int32.Parse(poLineNum.Split('.').Last());
            }
            else
            {
                lineNum = Int32.Parse(poLineNum);
                lineRev = 0;
            }

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", poExternalId);
                param.Add("@LineNum", lineNum);
                param.Add("@LineRev", lineRev);

                var result = con.Query<int>("SELECT pol.POLineID FROM PurchaseOrderLines pol " +
                    "INNER JOIN PurchaseOrders po ON po.PurchaseOrderID = pol.PurchaseOrderID " +
                    "WHERE po.ExternalID = @ExternalID AND pol.LineNum = @LineNum AND pol.LineRev = @LineRev", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    poLineId = result.First();
                }

                con.Close();
            }

            return poLineId;
        }

        public PurchaseOrderSOAllocation GetPOAllocationFromProductSpec(string productSpec, int poLineId)
        {
            PurchaseOrderSOAllocation poAlloc = null;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ProductSpec", productSpec);
                param.Add("@POLineID", poLineId);

                var result = con.Query<PurchaseOrderSOAllocation>("SELECT * FROM mapSOPOAllocation m " +
                    "INNER JOIN SalesOrderLines sol ON sol.SOLineID = m.SOLineID " +
                    "WHERE sol.ProductSpec = @ProductSpec AND m.POLineID = @POLineID AND m.IsDeleted = 0", param, commandType: null);

                if (result != null && result.Count() > 0)
                {
                    poAlloc = result.First();
                }

                con.Close();
            }

            return poAlloc;
        }

        public void UpdateWarehouseOnPurchaseOrderLines(int warehouseId, int purchaseOrderId)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@PurchaseOrderID", purchaseOrderId);
                parm.Add("@ToWarehouseID", warehouseId);

                con.Query("UPDATE PurchaseOrderLines SET ToWarehouseID=@ToWarehouseID WHERE PurchaseOrderID=@PurchaseOrderID", parm, commandType: null).FirstOrDefault();
                con.Close();
            }

            return;
        }

        public PurchaseOrderSOAllocation GetPOAllocationFromLine(int poLineId)
        {
            PurchaseOrderSOAllocation poAlloc = null;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@POLineID", poLineId);

                var result = con.Query<PurchaseOrderSOAllocation>("SELECT * FROM mapSOPOAllocation m WHERE m.POLineID = @POLineID AND m.IsDeleted = 0", param, commandType: null);

                if (result != null && result.Count() > 0)
                {
                    poAlloc = result.First();
                }

                con.Close();
            }

            return poAlloc;
        }

        public IList<PurchaseOrderLineHistoryDb> GetPurchaseOrderByAccountId(int accountId,int? contactId=null)
        {
            List<PurchaseOrderLineHistoryDb> POLineDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@ContactID", contactId);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@RowLimit", 999999);

                POLineDbs = con.Query<PurchaseOrderLineHistoryDb>("uspPurchaseOrderLinesHistoryGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return POLineDbs;
        }
    }

}
