using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.DB.OrderFulfillment;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Dapper;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.OrderFulfillment;
using Sourceportal.Domain.Models.API.Responses.OrderFulfillment;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.DB.ItemStock;

namespace Sourceportal.DB.OrderFillment
{
    [DataContract]
    public class OrderFillmentRepository : IOrderFillmentRepository
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;
        public IList<OFListDb> GetOrderfullfillmentList(OrderFulfillmentListSearchFilter searchFilter)
        {
            var ofListDb = new List<OFListDb>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                
                param.Add("@SearchString", searchFilter.SearchString);
                param.Add("@RowOffset", searchFilter.RowOffset);
                param.Add("@RowLimit", searchFilter.RowLimit);
                param.Add("@DescSort", searchFilter.DescSort);
                param.Add("@UnderallocatedOnly", searchFilter.UnderallocatedOnly);
                param.Add("@BuyerID", UserHelper.GetUserId());
                param.Add("@SortBy",searchFilter.SortCol);
                param.Add("@DescSort",searchFilter.DescSort);
                param.Add("@FilterBy", searchFilter.FilterBy);
                param.Add("@FilterText", searchFilter.FilterText);
                param.Add("@CommentTypeID", CommentType.SalesOrderLine);
                ofListDb = con.Query<OFListDb>("uspOrderFulfillmentGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                con.Close();
            }
            return ofListDb;
        }

        public IList<OFListDb> GetRequestToPurchaseList(RequestToPurchaseListRequest searchFilter)
        {
            var ofListDb = new List<OFListDb>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                //var buyerId = searchFilter.BuyerId == 0 ? UserHelper.GetUserId() : searchFilter.BuyerId;
                param.Add("@RowOffset", searchFilter.RowOffset);
                param.Add("@RowLimit", searchFilter.RowLimit);
                param.Add("@DescSort", searchFilter.DescSort);
                param.Add("@UnderallocatedOnly", searchFilter.UnderallocatedOnly);
                param.Add("@BuyerID", searchFilter.BuyerId);
                param.Add("@AccountID", searchFilter.AccountId);
                param.Add("@SortBy", searchFilter.SortBy);
                param.Add("@DescSort", searchFilter.DescSort);
                ofListDb = con.Query<OFListDb>("uspRequestToPurchaseGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                con.Close();
            }
            return ofListDb;
        }

        public IList<OFAvailabilityDb> GetOrderFulfillmentAvailability(int soLineId)
        {
            var ofAvailDb = new List<OFAvailabilityDb>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@SOLineID", soLineId);

                ofAvailDb = con.Query<OFAvailabilityDb>("uspAvailableInvPOGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                con.Close();
            }
            return ofAvailDb;
        }

        public string GetWarehouseExternalId(int warehouseId)
        {
            string externalId = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@WarehouseID", warehouseId);

                var result = con.Query<string>("SELECT ExternalID FROM Warehouses WHERE WarehouseID=@WarehouseID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    externalId = result.First();
                }

                con.Close();
            }

            return externalId;
        }

        public string GetWarehouseExternalIdFromBin(int warehouseBinId)
        {
            string externalId = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@WarehouseBinID", warehouseBinId);

                var result = con.Query<string>("SELECT wh.ExternalID FROM Warehouses wh " +
                    "INNER JOIN WarehouseBins whb ON wh.WarehouseID = whb.WarehouseID WHERE whb.WarehouseBinID=@WarehouseBinID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    externalId = result.First();
                }

                con.Close();
            }

            return externalId;
        }

        public string GetQcBinUUIDFromWarehouseBin(int warehouseBinId)
        {
            string externalId = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@WarehouseBinID", warehouseBinId);

                var result = con.Query<string>("SELECT whb.ExternalUUID FROM Warehouses wh " +
                    "INNER JOIN WarehouseBins whb ON wh.WarehouseID = whb.WarehouseID " +
                    "WHERE whb.WarehouseID = (SELECT WarehouseID FROM WarehouseBins WHERE WarehouseBinID=@WarehouseBinID) " +
                    "AND whb.ExternalID = 'QC'", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    externalId = result.First();
                }

                con.Close();
            }

            return externalId;
        }

        public string GetWarehouseBinExternalId(int warehouseBinId)
        {
            string externalId = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@WarehouseBinID", warehouseBinId);

                var result = con.Query<string>("SELECT ExternalID FROM WarehouseBins WHERE WarehouseBinID=@WarehouseBinID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    externalId = result.First();
                }

                con.Close();
            }

            return externalId;
        }

        public IList<SOAllocationDb> GetUnallocatedSOLines(UnallocatedSOLinesGetRequest unallocatedSoLinesGetRequest)
        {
            var ofListDb = new List<SOAllocationDb>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@POLineID", unallocatedSoLinesGetRequest.PoLineId);
                param.Add("@IncludeUnallocated", unallocatedSoLinesGetRequest.IncludeUnallocated);
                ofListDb = con.Query<SOAllocationDb>("uspPOLineAllocationGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                con.Close();
            }
            return ofListDb;
        }

        public int SetItemInventory(ItemInventoryDb ii)
        {
            int inventoryId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                if (ii.InventoryID < 1)
                    param.Add("@InventoryID", null);
                else
                    param.Add("@InventoryID", ii.InventoryID);
                param.Add("@StockID", ii.StockID);
                param.Add("@WarehouseBinID", ii.WarehouseBinID);
                param.Add("@Qty", ii.Qty);
                param.Add("@IsInspection", ii.IsInspection);
                param.Add("@IsDeleted", ii.IsDeleted);
                param.Add("@UserID", UserHelper.GetUserId());

                inventoryId = con.Query<int>("uspItemInventorySet", param,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                con.Close();
            }

            return inventoryId;
        }

        public int SetItemStock(ItemStockDB ist)
        {
            int stock;
            bool isEditing = false;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                if (ist.ItemStockID > 1)
                {
                    param.Add("@StockID", ist.ItemStockID);
                    isEditing = true;
                }
                else
                {
                    param.Add("@StockID", null);
                }
                param.Add("@POLineID", ist.POLineID);
                param.Add("@ItemID", ist.ItemID);
                param.Add("@InvStatusID", ist.InvStatusID);
                param.Add("@DateCode", ist.DateCode);
                param.Add("@MfrLotNum", ist.MfrLotNum);
                param.Add("@ReceivedDate", ist.ReceivedDate);
                param.Add("@PackagingID", ist.PackagingID);
                param.Add("@PackageConditionID", ist.PackageConditionID);
                //param.Add("@WarehouseBinID", ist.WarehouseBinID);
                param.Add("@COO", ist.COO);
                param.Add("@Expiry", ist.Expiry);
                param.Add("@StockDescription", ist.StockDescription);
                param.Add("@ExternalID", ist.ExternalID);
                param.Add("@IsRejected", ist.IsRejected);
                param.Add("@IsDeleted", ist.IsDeleted);
                param.Add("@ClonedFromID", ist.ClonedFromID);
                param.Add("@UserID", UserHelper.GetUserId());

                stock = con.Query<int>("uspItemStockSet", param,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }

            //success
            if (stock > 0 && !isEditing)
            {
                //new stock, map to inspection
                using (var mCon = new SqlConnection(ConnectionString))
                {
                    mCon.Open();
                    int success = 0;
                    DynamicParameters mParam = new DynamicParameters();
                    mParam.Add("@StockID", stock);
                    mParam.Add("@InspectionID", ist.InspectionID);
                    mParam.Add("@CreatedBy", UserHelper.GetUserId());

                    success = mCon.Query<int>("uspMapInspectionStockSet", mParam, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    mCon.Close();
                }
            }
            return stock;
        }

        public List<SoSWarehouseDetailsDB> GetWarehouseSoSDetails(int soLineId)
        {
            List<SoSWarehouseDetailsDB> whSoS = new List<SoSWarehouseDetailsDB>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@SOLineID", soLineId);

                whSoS = con.Query<SoSWarehouseDetailsDB>("SELECT * FROM dbo.tfnGetWarehouseSoSDetails(@SOLineID)", param, commandType: null).ToList();

                con.Close();
            }

            return whSoS;
        }

        public List<SoSWarehouseDetailsDB> GetStockWarehouseSoSDetails(int stockId)
        {
            List<SoSWarehouseDetailsDB> whSoS = new List<SoSWarehouseDetailsDB>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@StockID", stockId);

                whSoS = con.Query<SoSWarehouseDetailsDB>("SELECT wh.OrganizationID, wh.ShipfromRegionID FROM vwStockQty sq " +
                    "INNER JOIN Warehouses wh ON sq.WarehouseID = wh.WarehouseID " +
                    "WHERE sq.StockID = @StockID", param, commandType: null).ToList();

                con.Close();
            }

            return whSoS;
        }

        public List<SoSWarehouseDetailsDB> GetPoWarehouseSoSDetails(int poLineId)
        {
            List<SoSWarehouseDetailsDB> whSoS = new List<SoSWarehouseDetailsDB>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@POLineID", poLineId);

                whSoS = con.Query<SoSWarehouseDetailsDB>("SELECT wh.OrganizationID, wh.ShipFromRegionID FROM vwPurchaseOrderLines pol " +
                    "INNER JOIN Warehouses wh ON pol.ToWarehouseID = wh.WarehouseID " +
                    "WHERE pol.POLineID = @POLineID", param, commandType: null).ToList();

                con.Close();
            }

            return whSoS;
        }

        public List<ItemStockDB> GetInspectionItemStockList(int inspectionId)
        {
            var istDb = new List<ItemStockDB>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@InspectionID", inspectionId);
                istDb = con.Query<ItemStockDB>("uspItemStockListGet", param, commandType: CommandType.StoredProcedure).ToList();

                con.Close();
            }
            return istDb;
        }
        public ItemStockDB GetItemStock(int stockId)
        {
            var istDb = new ItemStockDB();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@StockID", stockId);
                istDb = con.Query<ItemStockDB>("uspItemStockGet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                con.Close();
            }
            return istDb;
        }

        public ItemInventoryDb GetItemInventory(int itemInventoryId)
        {
            var iiDb = new ItemInventoryDb();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@InventoryID", itemInventoryId);
                iiDb = con.Query<ItemInventoryDb>("uspItemInventoryGet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                con.Close();
            }
            return iiDb;
        }

        public List<ItemInventoryDb> GetItemInventoryOnStock(int stockId)
        {
            var itemInventoryList = new List<ItemInventoryDb>();
            var invIdList = new List<int>();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@StockID", stockId);

                invIdList = con.Query<int>("SELECT InventoryID FROM ItemInventory WHERE StockID=@StockID AND IsDeleted = 0", param, commandType: null).ToList();

                con.Close();
            }

            foreach(var id in invIdList)
            {
                itemInventoryList.Add(GetItemInventory(id));
            }

            return itemInventoryList;
        }

        public void UpdateItemStock(int inspectionId)
        {
            var istDb = new List<ItemStockDB>();
            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                con.Open();
                param.Add("@InspectionID", inspectionId);
                istDb = con.Query<ItemStockDB>("uspItemStockListGet", param, commandType: CommandType.StoredProcedure).ToList();

                foreach (var item in istDb)
                {
                    if (item.IsRejected)
                    {
                        item.WarehouseBinID = item.RejectedBinID;
                    }
                    else
                    {
                        item.WarehouseBinID = item.AcceptedBinID;
                    }
                    param.Add("@StockID", item.ItemStockID);
                    param.Add("@WarehouseBinId", item.WarehouseBinID);
                    con.Query<int>("UPDATE ItemInventory SET WarehouseBinID=@WarehouseBinId WHERE StockID=@StockID", param, commandType: null);
                }
                con.Close();
            }
        }
        public SetOrderFulfillmentQtyDb SetOrderFulfillmentQty(int soLineId, int id, string idType, int qty, bool isDeleted)
        {
            var setQtyDb = new SetOrderFulfillmentQtyDb();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@SOLineID", soLineId);
                param.Add("@Qty", qty);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                if (idType.Equals("Inventory"))
                {
                    param.Add("@StockID", id);
                    var res = con.Query<SetOrderFulfillmentQtyDb>("uspSoInvFulfillmentSet", param,
                        commandType: CommandType.StoredProcedure);

                    var errorId = param.Get<int>("@ret");
                    if (errorId != 0)
                    {
                        var errorMessage = string.Format("Database error occured: {0}", OFDbErrors.ErrorCodes[errorId]);
                        throw new GlobalApiException(errorMessage);
                    }
                    setQtyDb = res.First();
                }
                else if (idType.Equals("Purchase Order"))
                {
                    param.Add("@POLineID", id);
                    param.Add("@IsDeleted", isDeleted);
                    var res = con.Query<SetOrderFulfillmentQtyDb>("uspPurchaseOrderAllocationSet", param,
                        commandType: CommandType.StoredProcedure);

                    var errorId = param.Get<int>("@ret");
                    if (errorId != 0)
                    {
                        var errorMessage = string.Format("Database error occured: {0}", OFPoAllocationErrors.ErrorCodes[errorId]);
                        throw new GlobalApiException(errorMessage);
                    }
                    setQtyDb = res.First();
                }
                
                con.Close();
            }

            return setQtyDb;
        }

        public SetOrderFulfillmentQtyDb UpdateExistingInventoryFulfillmentQty(int id,  int qty, bool isDeleted)
        {
            var setQtyDb = new SetOrderFulfillmentQtyDb();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();                
                param.Add("@Qty", qty);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@IsDeleted", isDeleted);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                param.Add("@StockID", id);

                var res = con.Query<SetOrderFulfillmentQtyDb>("uspInvFulfillmentUpdateExisting", param,
                    commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", OFDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                setQtyDb = res.First();

                con.Close();
            }

            return setQtyDb;
        }

        public int GetWarehouseBinIdByExternalUUID(string externalId)
        {
            int warehouseBinId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);

                warehouseBinId = con.Query<int>("SELECT WarehouseBinID FROM WarehouseBins WHERE ExternalUUID=@ExternalId", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return warehouseBinId;
        }

        public int GetQtyAllocatedForLine(int soLineId)
        {
            int qty = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@SOLineID", soLineId);

                qty = con.Query<int>("SELECT sum(qty) FROM mapSOInvFulfillment WHERE SOLineID=@SOLineID AND IsDeleted = 0", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return qty;
        }


        public int GetInTransitBinByWarehouseExternalUUID(string externalUUID)
        {
            int warehouseBinId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalUUID);

                warehouseBinId = con.Query<int>("SELECT WB.WarehouseBinID FROM WarehouseBins WB " +
                    "INNER JOIN Warehouses w ON w.WarehouseID = wb.WarehouseID WHERE w.ExternalUUID=@ExternalId and wb.ExternalUUID IS NULL", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return warehouseBinId;
        }

        public int GetWarehouseBinIdByExternalID(string externalId)
        {
            int warehouseBinId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);

                warehouseBinId = con.Query<int>("SELECT WarehouseBinID FROM WarehouseBins WHERE ExternalID=@ExternalId", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return warehouseBinId;
        }

        public int GetWarehouseBinIdByExternalIdWarehouseExternalId(string externalId, string warehouseExternalId)
        {
            int warehouseBinId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                param.Add("@WarehouseExternalId", warehouseExternalId);

                warehouseBinId = con.Query<int>("SELECT WarehouseBinID FROM WarehouseBins wb " +
                    "INNER JOIN Warehouses wh ON wb.WarehouseId = wh.WarehouseId " +
                    "WHERE wb.ExternalID=@ExternalId AND wh.ExternalID=@WarehouseExternalId", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return warehouseBinId;
        }

        public int GetSoLineIdOnStock(int stockId)
        {
            int soLineId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@StockID", stockId);

                soLineId = con.Query<int>("SELECT SOLineID FROM mapSOInvFulfillment WHERE StockID=@StockID AND IsDeleted = 0", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return soLineId;
        }

        public int GetInvStatusIdFromExternal(string externalId)
        {
            int invStatusId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);

                invStatusId = con.Query<int>("SELECT InvStatusID FROM lkpItemInvStatuses WHERE ExternalID=@ExternalId", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return invStatusId;
        }

        public ItemInventoryDb GetItemInventoryFromBinExternal(string stockExternalId, string warehouseBinExternalId)
        {
            int invId = 0;
            ItemInventoryDb invDb = null;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", stockExternalId);
                param.Add("@WarehouseBinExternalId", warehouseBinExternalId);

                invId = con.Query<int>("SELECT InventoryID FROM ItemInventory ii " +
                    "INNER JOIN WarehouseBins wb ON wb.WarehouseBinID = ii.WarehouseBinID " +
                    "INNER JOIN ItemStock ist ON ist.StockID = ii.StockID " +
                    "WHERE ist.ExternalID = @ExternalId AND wb.ExternalUUID = @WarehouseBinExternalId AND ist.IsDeleted = 0 AND ii.IsDeleted = 0", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            if (invId > 0)
            {
                invDb = GetItemInventory(invId);
            }

            return invDb;
        }

        public ItemInventoryDb GetItemInventoryFromCompoundSapKey(string stockExternalId, string warehouseBinExternalId, bool isInspection, bool isRestricted)
        {
            int invId = 0;
            ItemInventoryDb invDb = null;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", stockExternalId);
                param.Add("@WarehouseBinExternalId", warehouseBinExternalId);
                param.Add("@IsInspection", isInspection);
                param.Add("@IsDeleted", isRestricted);

                invId = con.Query<int>("SELECT InventoryID FROM ItemInventory ii " +
                    "INNER JOIN WarehouseBins wb ON wb.WarehouseBinID = ii.WarehouseBinID " +
                    "INNER JOIN ItemStock ist ON ist.StockID = ii.StockID " +
                    "WHERE ist.ExternalID = @ExternalId AND wb.ExternalUUID = @WarehouseBinExternalId " +
                    "AND ii.IsDeleted = @IsDeleted AND ii.IsInspection = @IsInspection", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            if (invId > 0)
            {
                invDb = GetItemInventory(invId);
            }

            return invDb;
        }

        public int GetStockIDFromExternal(string externalId)
        {
            int stockId = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);

                stockId = con.Query<int>("SELECT StockID FROM ItemStock ist " +
                    "WHERE ist.ExternalID = @ExternalId AND ist.IsDeleted = 0", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return stockId;
        }

        public int GetStockQtyAllocated(int stockId)
        {
            int qty = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@StockID", stockId);

                qty = con.Query<int>("SELECT Qty FROM mapSOInvFulfillment soif " +
                    "WHERE soif.StockID = @StockID AND soif.IsDeleted = 0", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return qty;
        }

        public int GetQtyOfInventoryOnStock(int stockId)
        {
            int qty = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@StockID", stockId);

                var res = con.Query<int>("SELECT ISNULL(SUM(Qty), 0) FROM ItemInventory ii " +
                    "WHERE ii.StockID = @StockID AND ii.IsDeleted = 0", param, commandType: null);

                if(res != null)
                {
                    qty = res.First();
                }

                con.Close();
            }

            return qty;
        }


    }   
}
