using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.API.Responses.Comments;
using Sourceportal.Domain.Models.DB.Comments;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;

namespace Sourceportal.DB.Comments
{
    public class CommentRepository : ICommentRepository
    {
        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public List<CommentDb> GetComments(int objectID, int objectTypeID, string searchString)
        {
            List<CommentDb> commentdbList;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ObjectID", objectID);
                param.Add("@ObjectTypeID", objectTypeID);
                param.Add("@SearchString", searchString);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                commentdbList = con.Query<CommentDb>("uspCommentsGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", CommentDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                con.Close();
            }
            return commentdbList;
        }

        public CommentDb SetComment(SetCommentRequest setCommentRequest)
        {
            CommentDb commentDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@CommentID", setCommentRequest.CommentID);
                param.Add("@CommentTypeID", setCommentRequest.CommentTypeID);
                param.Add("@ObjectID", setCommentRequest.ObjectID);
                param.Add("@ObjectTypeID", setCommentRequest.ObjectTypeID);
                param.Add("@Comment", setCommentRequest.Comment);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@IsDeleted", setCommentRequest.IsDeleted);
                param.Add("@ReplyToID", setCommentRequest.ReplyToID);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<CommentDb>("uspCommentSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", CommentDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                commentDb = res.First();
                con.Close();
            }

            return commentDb;
        }
    }
}
