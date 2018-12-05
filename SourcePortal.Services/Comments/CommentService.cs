using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.Comments;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.API.Responses.Comments;
using Sourceportal.Domain.Models.DB.Comments;

namespace SourcePortal.Services.Comments
{
    public class CommentService : ICommentService
    {
        public readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public CommentsResponse GetComments(int objectId, int objectTypeId, string searchString)
        {
            var dbCommentList = _commentRepository.GetComments(objectId, objectTypeId, searchString);
            var comments = new List<CommentResponse>();
            var response = new CommentsResponse();

            foreach (var dbComment in dbCommentList)
            {
                var comment = new CommentResponse();
                comment.AuthorName = dbComment.AuthorName;
                comment.CommentID = dbComment.CommentID;
                comment.CommentTypeID = dbComment.CommentTypeID;
                comment.Comment = dbComment.Comment;
                comment.Created = dbComment.Created;
                comment.TypeName = dbComment.TypeName;
                comment.ReplyToID = dbComment.ReplyToID;
                comment.ReplyToName = dbComment.ReplyToName;
                comment.CreatedBy = dbComment.CreatedBy;

                comments.Add(comment);
            }
            response.Comments = comments;
            return response;
        }

        public CommentResponse SetComment(SetCommentRequest setCommentRequest)
        {
            var dbComment = _commentRepository.SetComment(setCommentRequest);
            var response = new CommentResponse();

            response.AuthorName = dbComment.AuthorName;
            response.CommentID = dbComment.CommentID;
            response.CommentTypeID = dbComment.CommentTypeID;
            response.Comment = dbComment.Comment;
            response.Created = dbComment.Created;
            response.TypeName = dbComment.TypeName;
            response.ReplyToID = dbComment.ReplyToID;
            response.ReplyToName = dbComment.ReplyToName;
            response.CreatedBy = dbComment.CreatedBy;

            return response;
        }
    }
}
