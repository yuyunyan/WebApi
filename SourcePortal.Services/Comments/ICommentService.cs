using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.API.Responses.Comments;

namespace SourcePortal.Services.Comments
{
    public interface ICommentService
    {
        CommentsResponse GetComments(int objectId, int objectTypeId, string searchString);
        CommentResponse SetComment(SetCommentRequest setCommentRequest);
    }
}
