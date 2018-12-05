using Dapper;
using Sourceportal.Domain.Models.API.Requests.Carrier;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB.Carrier;
using Sourceportal.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Carrier
{
    public class CarrierRepository : ICarrierRepository
    {
        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public List<AccountCarrierDb> GetAccountCarriers(int accountId)
        {
            List<AccountCarrierDb> accountCarrierDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", accountId);

                accountCarrierDbs = con.Query<AccountCarrierDb>("uspAccountCarriersGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountCarrierDbs;
        }

        public bool DeleteAccountCarrier(DeleteCarrierRequest deleteCarrierRequest)
        {
            bool RowsDeleted;

            using (var con= new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", deleteCarrierRequest.AccountID);
                param.Add("@CarrierID", deleteCarrierRequest.CarrierID);
                param.Add("@UserID", UserHelper.GetUserId());
                RowsDeleted = Boolean.Parse(con.Query<string>("uspAccountCarriersDelete", param, commandType: CommandType.StoredProcedure).First());
                con.Close();
            }
            return RowsDeleted;
        }

        public List<AccountCarrierDb> GetCarrier()
        {
            List<AccountCarrierDb> carrierDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                carrierDbs = con.Query<AccountCarrierDb>("uspCarriersGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return carrierDbs;
        }

        public int CarrierSet(AccountCarrierInsertUpdateRequest accountCarrierSet)
        {
            int RowCount;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@CarrierID", accountCarrierSet.CarrierID);
                param.Add("@AccountID", accountCarrierSet.AccountID);
                param.Add("@AccountNumber", accountCarrierSet.AccountNumber);
                param.Add("@isDefault", accountCarrierSet.IsDefault);
                param.Add("@UserID", UserHelper.GetUserId());
                RowCount = con.Query<int>("uspAccountCarriersSet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return RowCount;
        }

        public AccountCarrierDb GetAccountNumber(int accountId, int carrierId)
        {
            var accountNumberDbs = new AccountCarrierDb();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@CarrierID", carrierId);
                accountNumberDbs = con.Query<AccountCarrierDb>("SELECT AccountNumber,AccountID,CarrierID FROM mapAccountCarriers " + "WHERE AccountID = @AccountID AND CarrierID = @CarrierID", param, commandType: null).FirstOrDefault();
                con.Close();
            }

            return accountNumberDbs;
        }
    }
}
