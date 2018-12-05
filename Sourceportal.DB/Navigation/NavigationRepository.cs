using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.DB.Navigation;
using Sourceportal.Utilities;

namespace Sourceportal.DB.Navigation
{
    public class NavigationRepository : INavigationRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public List<DbNavigation> NavigationListGet()
        {
            List<DbNavigation> dbNavigations;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                dbNavigations = con.Query<DbNavigation>("uspNavigationsGet").ToList();

                con.Close();
            }

            return dbNavigations;
        }

        public List<DbGeneralSecurity> GeneralSecuritiesGet()
        {
            List<DbGeneralSecurity> dbGeneralSecurities;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", UserHelper.GetUserId());
                dbGeneralSecurities = con.Query<DbGeneralSecurity>("uspUserGeneralSecurityGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return dbGeneralSecurities;
        }

        public List<DbUserField> UserObjectSecurityGet(int objectId, int objectTypeId)
        {
            List<DbUserField> dbUserFields;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@objectId", objectId);
                param.Add("@objectTypeId", objectTypeId);
                param.Add("@UserID", UserHelper.GetUserId());

                dbUserFields = con.Query<DbUserField>("uspUserObjectSecurityGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return dbUserFields;
        }

        public bool UserObjectLevelSecurityGet(int objectId, int objectTypeId)
        {
            IEnumerable<dynamic> result;
            var objectType = (ObjectType)objectTypeId;

            var spDictionary = new Dictionary<ObjectType, string>
            {
               {ObjectType.Salesorder, "uspSalesOrderSecurityGet"},
               {ObjectType.Purchaseorder, "uspPurchaseOrderSecurityGet"},
               {ObjectType.Accounts, "uspAccountSecurityGet"},
               {ObjectType.Quote, "uspQuoteSecurityGet"},
               {ObjectType.ItemList, "uspItemListSecurityGet"},
               {ObjectType.VendorRfq, "uspVendorRFQSecurityGet"},
               {ObjectType.Inspection, "uspQCInspectionSecurityGet"},
               {ObjectType.Contact, "uspContactSecurityGet"},
               {ObjectType.Item, "uspItemSecurityGet"},
            };

            var spParams = new Dictionary<ObjectType, string>
            {
               {ObjectType.Salesorder, "@SalesOrderID"},
               {ObjectType.Purchaseorder, "@PurchaseOrderID"},
               {ObjectType.Accounts, "@AccountID"},
               {ObjectType.Quote, "@QuoteID"},              
               {ObjectType.ItemList, "@ItemListID"},
               {ObjectType.VendorRfq, "@VendorRFQID"},
               {ObjectType.Inspection, "@InspectionID"},
               {ObjectType.Contact, "@ContactID"},
               {ObjectType.Item, "@ItemID"},
            };

            var sp = spDictionary[objectType];
            var param1 = spParams[objectType];

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add(param1, objectId);
                param.Add("@UserID", UserHelper.GetUserId());
                result = con.Query(sp, param, commandType: CommandType.StoredProcedure);
                con.Close();
            }

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
