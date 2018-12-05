using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.DB.Items;
using System.Configuration;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Sourceportal.Domain.Models.API.Requests.Items;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses.Items;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Domain.Models.Shared;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.DB.QC;

namespace Sourceportal.DB.Items
{
    public class ItemRepository : IItemRepository
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;
        private static readonly string IhsConnectionString = ConfigurationManager.ConnectionStrings["IhsConnection"].ConnectionString;

        public ItemDb GetItemDetails(int ItemID)
        {
            ItemDb itemDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ItemID", ItemID);
                param.Add("@UserID", UserHelper.GetUserId());
                itemDb = con.Query<ItemDb>("uspItemsGet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return itemDb;
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

        public IList<ItemStatus> GetItemStatuses()
        {
            List<ItemStatus> itemstatuses;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                itemstatuses = con.Query<ItemStatus>("uspItemStatusesGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return itemstatuses;
        }

        public void DeleteItem(int itemId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                string cmdText = "uspItemDelete";
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", itemId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        public ItemDb CreateIhsItemInDb(long itemId)
        {
            IhsItem ihsItem = new IhsItem();

            var query =
                @"select ObjectID, PartNumber, Manufacturer, PartDescription,PackageDescription, ReachCompliant,EuRohs,CNRohs,DatasheetURL,PartStatus,GenericNumber, C.MiddleLevel AS Category
                        from PartsBasic P with (NOLOCK) Inner Join Categories C ON P.Category = C.Category
                        where objectid =" + itemId;
            try
            {
                using (var con = new SqlConnection(IhsConnectionString))
                {
                    con.Open();
                    //DynamicParameters param = new DynamicParameters();

                    ihsItem = con.Query<IhsItem>(query).First();
                    con.Close();
                }
            }
            catch (Exception ex)
            {

            }
            ItemDb itemDb;
            int returnedId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@MPN", ihsItem.PartNumber);
                param.Add("@Mfr", ihsItem.Manufacturer);
                param.Add("@PartDescription", ihsItem.PartDescription);
                param.Add("@MfrPackageDescription", ihsItem.PackageDescription);
                param.Add("@ReachCompliant", ihsItem.ReachCompliant);
                param.Add("@EuRohs", ihsItem.EuRohs);
                param.Add("@ChinaRohs", ihsItem.CNRohs);
                param.Add("@Status", ihsItem.PartStatus);
                param.Add("@GenericNumber", ihsItem.GenericNumber);
                param.Add("@ObjectID", ihsItem.ObjectId);
                param.Add("@Category", ihsItem.Category);
                param.Add("@DatasheetURL", ihsItem.DatasheetURL);
                param.Add("@UserID", UserHelper.GetUserId());

                returnedId = con.Query<int>("uspItemCreateFromIHS", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }

            itemDb = GetItemDetails(returnedId);
            return itemDb;
        }

        public IList<CommodityDb> GetItemCommodityList()
        {
            List<CommodityDb> itemcommodities;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                itemcommodities =
                    con.Query<CommodityDb>("uspItemCommoditiesGet", param, commandType: CommandType.StoredProcedure)
                        .ToList();
                con.Close();
            }
            return itemcommodities;
        }

        public IList<ItemGroupDb> GetItemGroupList()
        {
            List<ItemGroupDb> itemgroup;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                itemgroup = con.Query<ItemGroupDb>("uspItemGroupsGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return itemgroup;
        }

        public IList<ManufacturerDb> GetManufacturerList(string searchText)
        {
            List<ManufacturerDb> manufactuerDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SearchText", searchText);
                manufactuerDb =
                    con.Query<ManufacturerDb>("uspManufacturersGet", param, commandType: CommandType.StoredProcedure)
                        .ToList();
                con.Close();
            }
            return manufactuerDb;
        }

        public IList<ItemPOsDb> GetItemPOList(ItemPOsListGetRequest itemPOsListRequest)
        {
            List<ItemPOsDb> itemPOsDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ItemID", itemPOsListRequest.ItemID);
                param.Add("@StartDate", null);
                param.Add("@EndDate", null);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@RowOffset", itemPOsListRequest.RowOffset);
                param.Add("@RowLimit", itemPOsListRequest.RowLimit);
                param.Add("@SortBy", itemPOsListRequest.SortBy);
                param.Add("@DescSort", itemPOsListRequest.DescSort);
                itemPOsDb =
                    con.Query<ItemPOsDb>("uspPurchaseOrderLinesHistoryGet", param, commandType: CommandType.StoredProcedure)
                        .ToList();
                con.Close();
            }
            return itemPOsDb;
        }

        public IList<ItemExtraDb> GetItemExtraList()
        {
            List<ItemExtraDb> itemExtraDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                itemExtraDb = con.Query<ItemExtraDb>("uspItemExtrasGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return itemExtraDb;
        }

        public IList<ItemDb> GetItemList(SearchFilter searchfilter)
        {
            List<ItemDb> itemDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SearchString", searchfilter.SearchString);
                param.Add("@RowOffset", searchfilter.RowOffset);
                param.Add("@RowLimit", searchfilter.RowLimit);
                param.Add("@SortBy", searchfilter.SortCol);
                param.Add("@DescSort", searchfilter.DescSort);
                param.Add("@UserID", UserHelper.GetUserId());

                itemDbs = con.Query<ItemDb>("uspItemsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return itemDbs;
        }

        public SetItemDetailsDb SetItemDetails(SetItemDetailsRequest itemdetailsrequest)
        {
            SetItemDetailsDb setitemdetailsdb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ItemID", itemdetailsrequest.itemId);
                param.Add("@PartNumber", itemdetailsrequest.mpn);
                param.Add("@ItemStatusID", itemdetailsrequest.statusId);
                param.Add("@MfrID", itemdetailsrequest.manufacturerId);
                param.Add("@CommodityID", itemdetailsrequest.commodityId);
                param.Add("@PartDescription", itemdetailsrequest.description);
                param.Add("@ECCN", itemdetailsrequest.eccn);
                param.Add("@EURoHS", itemdetailsrequest.eurohs);
                param.Add("@CNRoHS", itemdetailsrequest.cnrohs);
                param.Add("@HTS", itemdetailsrequest.hts);
                param.Add("@MSL", itemdetailsrequest.msl);
                param.Add("@WeightG", itemdetailsrequest.weight);
                param.Add("@LengthCM", itemdetailsrequest.length);
                param.Add("@WidthCM", itemdetailsrequest.width);
                param.Add("@DepthCM", itemdetailsrequest.depth);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<SetItemDetailsDb>("uspItemSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", ItemDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                setitemdetailsdb = res.First();
                con.Close();
            }
            return setitemdetailsdb;
        }

        public IList<ItemDb> GetItems()
        {
            List<ItemDb> itemDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                itemDbs = con.Query<ItemDb>("uspItemListGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return itemDbs;
        }

        public void ItemSapDataSet(SetExternalIdRequest request)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ItemID", request.ObjectId);
                param.Add("@ExternalId", request.ExternalId);
                param.Add("@UserID", UserHelper.GetUserId());

                con.Query("uspItemSapDataSet", param,
                    commandType: CommandType.StoredProcedure);
                con.Close();
            }
        }

        public int CreateManufacturer(SetManufacturerRequest model)
        {
            int result;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@MfrName", model.MfrName);
                param.Add("@Code", 0);
                param.Add("@MfrUrl", model.MfrUrl);
                param.Add("@CreatedBy", model.CreatedBy);
                result = con.Query<int>("uspManufacturersSet", param,
                    commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return result;
        }

        public ItemInventory GetItemInventoryByExternalId(string requestInventoryExternalId)
        {
            ItemInventory itemInventory = null;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", requestInventoryExternalId);

                var inventoryList = con.Query<ItemInventory>("SELECT InventoryID FROM iteminventory WHERE ExternalID=@ExternalId", param, commandType: null);
                if (inventoryList != null && inventoryList.Count() > 0)
                {
                    itemInventory = inventoryList.First();
                }
                con.Close();
            }

            return itemInventory;
        }

        public int GetItemByExternalId(string externalId)
        {
            int itemId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);

                itemId = con.Query<int>("SELECT ItemID FROM Items WHERE ExternalID=@ExternalId", param, commandType: null).FirstOrDefault();

                con.Close();
            }
                
            return itemId;
        }

        public string GetItemExternalById(int itemId)
        {
            string externalId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ItemID", itemId);

                externalId = con.Query<string>("SELECT ExternalID FROM Items WHERE ItemID=@ItemID", param, commandType: null).FirstOrDefault();

                con.Close();
            }

            return externalId;
        }
        
        public IList<ItemInventoryDb> ItemInventory(int ItemID,bool ExcludePo, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            List<ItemInventoryDb> invDb;


            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                //param.Add("@SearchString", searchfilter.SearchString);
                param.Add("@ItemID", ItemID);
                param.Add("@ExcludePO", ExcludePo);
                param.Add("@RowOffset", RowOffset);
                param.Add("@RowLimit", RowLimit);
                param.Add("@SortBy", SortCol);
                param.Add("@DescSort", DescSort);
                //param.Add("@UserID", UserHelper.GetUserId());
                invDb = con.Query<ItemInventoryDb>("uspAvailableInvPOGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return invDb;
        }

        public IList<ItemAvailabilityDb> ItemAvailability(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            List<ItemAvailabilityDb> invDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                //param.Add("@SearchString", searchfilter.SearchString);
                param.Add("@ItemID", ItemID);
                param.Add("@RowOffset", RowOffset);
                param.Add("@RowLimit", RowLimit);
                param.Add("@SortBy", SortCol);
                param.Add("@DescSort", DescSort);
                param.Add("@ShowInventory", false);
                //param.Add("@UserID", UserHelper.GetUserId());
                invDb = con.Query<ItemAvailabilityDb>("uspSourcesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return invDb;
        }

        public IList<ItemSalesOrdersDb> ItemSalesOrders(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            List<ItemSalesOrdersDb> invDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                //param.Add("@SearchString", searchfilter.SearchString);
                param.Add("@ItemID", ItemID);
                param.Add("@RowOffset", RowOffset);
                param.Add("@RowLimit", RowLimit);
                param.Add("@SortBy", SortCol);
                param.Add("@DescSort", DescSort);
                param.Add("@UserID", UserHelper.GetUserId());
                invDb = con.Query<ItemSalesOrdersDb>("uspSalesOrderLinesHistoryGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return invDb;
        }

        public IList<ItemQuotesDb> ItemQuotes(int ItemID, int RowOffset, int RowLimit, string SortCol, bool DescSort)
        {
            List<ItemQuotesDb> invDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                //param.Add("@SearchString", searchfilter.SearchString);
                param.Add("@ItemID", ItemID);
                param.Add("@RowOffset", RowOffset);
                param.Add("@RowLimit", RowLimit);
                param.Add("@SortBy", SortCol);
                param.Add("@DescSort", DescSort);
                param.Add("@UserID", UserHelper.GetUserId());
                invDb = con.Query<ItemQuotesDb>("uspQuoteLinesHistoryGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }    
            return invDb;
        }
    }
}

