using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Sourceportal.Domain.Models.API.Requests.BOMs;
using Sourceportal.Domain.Models.API.Responses.UploadXls;
using Sourceportal.Domain.Models.DB.UploadXls;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.DB.UploadXls
{
    public class UploadXlsRepository : IUploadXlsRepository
    {
        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public List<XlsDataMapDb> XlsDataMapGet(string xlsType, int itemListTypeID)
        {
            List<XlsDataMapDb> xlsDataMapDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                var param = new DynamicParameters();
                param.Add("@XlsType", xlsType);
                param.Add("@ItemListTypeID", itemListTypeID);
                xlsDataMapDbs = con.Query<XlsDataMapDb>("uspXlsDataMapGet", param,
                    commandType: CommandType.StoredProcedure).ToList();

                con.Close();
            }

            return xlsDataMapDbs;
        }

        public List<XLSDataMapObject> XlsAccountGet(int accountId, string xlsType)
        {
            List<XLSDataMapObject> xlsDataMapObjectDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                var param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@XlsType", xlsType);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                xlsDataMapObjectDbs = con
                    .Query<XLSDataMapObject>("uspXlsAccountGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", UploadDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                con.Close();
            }

            return xlsDataMapObjectDbs;
        }
    }
}
