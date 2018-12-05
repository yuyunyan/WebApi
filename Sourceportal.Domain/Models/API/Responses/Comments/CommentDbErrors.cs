using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Comments
{
    public class CommentDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            // CommentsGet
            {-1, "Missing ObjectID or ObjectTypeID"},
            {-2, "XlsType is missing"},
            {-3, "@ObjectTypeID is required"},
            {-4, "@CommentTypeID is required if multiple CommentTypes match with given @ObjectTypeID"},
            {-5, "@ObjectTypeID is invalid"},
            {-6, "@UserID is required"},
            {-7, "@ObjectID is required"}
        };
    }
}
