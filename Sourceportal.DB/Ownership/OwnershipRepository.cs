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
using Sourceportal.Domain.Models.API.Requests.Ownership;
using Sourceportal.Domain.Models.DB.Accounts;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;

namespace Sourceportal.DB.Ownership
{
    public class OwnershipRepository : IOwnershipRepository
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public IList<OwnerDb> GetObjectOwnership(GetOwnershipRequest getOwnershipRequest)
        {
            var objectOwnership = new List<OwnerDb>();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ObjectID", getOwnershipRequest.ObjectID);
                param.Add("@ObjectTypeID", getOwnershipRequest.ObjectTypeID);

                objectOwnership = con.Query<OwnerDb>("uspOwnershipGet", param,
                    commandType: CommandType.StoredProcedure).ToList();

                con.Close();
            }

            return objectOwnership;
        }

        public IList<OwnerDb> SetObjectOwnership(SetOwnershipRequest setOwnershipRequest)
        {
            var objectOwnership = new List<OwnerDb>();

            var ownerList = setOwnershipRequest.OwnerList.Select(x => new { userId = x.UserID, percentage = x.Percentage }).ToList();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@ObjectID", setOwnershipRequest.ObjectID);
                param.Add("@ObjectTypeID", setOwnershipRequest.ObjectTypeID);
                param.Add("@OwnerList", JsonConvert.SerializeObject(ownerList));
                param.Add("@CreatedBy", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                objectOwnership = con.Query<OwnerDb>("uspOwnershipSet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = "Database error occured: Setting ownership failed.";
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }

            return objectOwnership;
        }
    }
}
