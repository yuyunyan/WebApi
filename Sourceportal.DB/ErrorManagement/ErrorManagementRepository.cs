using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Sourceportal.Domain.Models.API.Requests.ErrorLog;
using Sourceportal.Domain.Models.DB.ErrorManagement;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.DB.ErrorManagementService
{
    public class ErrorManagementRepository : IErrorManagementRepository
    {
        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public int SaveLogDb(ExceptionLogSave excLogSave)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@AppID", excLogSave.ApplicationId);
                param.Add("@URL", excLogSave.Url);
                param.Add("@PostData", excLogSave.PostData);
                param.Add("@ExceptionType", excLogSave.ExceptionType);
                param.Add("@ErrorMessage", excLogSave.ErrorMessage);
                param.Add("@InnerExceptionMessage", excLogSave.InnerException);
                param.Add("@StackTrace", excLogSave.StackTrace);
                param.Add("@UserID", excLogSave.UserId);
                param.Add("@TimeStamp", excLogSave.TimeStamp);

                var errorId = con.Query<int>("uspErrorLogSet", param, commandType: CommandType.StoredProcedure);
                
                con.Close();
                return errorId.First();
            }
        }

        public List<ErrorLogDb> ErrorLogListGet(ErrorLogListRequest errorLogListRequest)
        {
            List<ErrorLogDb> errorLogListDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                if (errorLogListRequest.AppId > 0)
                {
                    param.Add("@AppID", errorLogListRequest.AppId);
                }
                param.Add("@SearchString", errorLogListRequest.SearchString);
                param.Add("@RowLimit", errorLogListRequest.RowLimit);
                param.Add("@RowOffset", errorLogListRequest.RowOffset);
                param.Add("@SortBy", errorLogListRequest.SortBy);
                param.Add("@DescSort", errorLogListRequest.DescSort ? 1 : 0);
                param.Add("@DateStart", errorLogListRequest.DateStart);
                param.Add("@DateEnd", errorLogListRequest.DateEnd);

                errorLogListDbs = con.Query<ErrorLogDb>("uspErrorLogGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return errorLogListDbs;
        }

        public ErrorLogDetailDb ErrorLogDetailGet(int errorId)
        {
            ErrorLogDetailDb errorLogDetailDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ErrorID", errorId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<ErrorLogDetailDb>("uspErrorLogDetailGet", param, commandType: CommandType.StoredProcedure);

                var errorCode = param.Get<int>("@ret");
                if (errorCode!= 0)
                {
                    errorLogDetailDb = new ErrorLogDetailDb
                    {
                        ErrorMessage = "Error ID is required."
                    };
                    con.Close();
                    return errorLogDetailDb;
                }

                if (!res.Any())
                {
                    errorLogDetailDb = new ErrorLogDetailDb
                    {
                        ErrorMessage = $"Error ID {errorId} not found."
                    };
                    con.Close();
                    return errorLogDetailDb;
                }

                errorLogDetailDb = res.First();
                con.Close();
            }
            return errorLogDetailDb;
        }
    }
}
