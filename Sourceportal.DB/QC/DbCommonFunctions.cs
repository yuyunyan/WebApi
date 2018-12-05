using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sourceportal.Domain.Models.DB.shared;

namespace Sourceportal.DB.QC
{
    public class DbCommonFunctions
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public static BaseDbResult ExecuteStoreProcedure(DynamicParameters parameters, string spName, Dictionary<int, string> errorList)
        {
            int id = 0;
            IEnumerable<dynamic> result;
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                   result = con.Query(spName, parameters, commandType: CommandType.StoredProcedure);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return new BaseDbResult { ErrorMessage = ex.Message };
            }

            var errorId = parameters.ParameterNames.Contains("@ret") ? parameters.Get<int>("@ret") : 0;
            if (errorId != 0)
            {
                var errorMessage = string.Format("Database error occured: {0}", errorList !=null && errorList.ContainsKey(errorId) ? errorList[errorId]: errorId.ToString());
                return new BaseDbResult { ErrorMessage = errorMessage };
            }

            int idReturned = 0;

            if (result.Count() > 0)
            {
                var castedResult = result.First() as IDictionary<string, object>;
                id = int.TryParse(castedResult.ElementAt(0).Value.ToString(), out idReturned) ? idReturned : 0;
            }
            
            return new BaseDbResult {CreatedId = id };
        }
       
    }
}