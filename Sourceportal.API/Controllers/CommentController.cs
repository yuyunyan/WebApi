using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.API.Responses.Comments;
using SourcePortal.Services.Comments;

namespace Sourceportal.API.Controllers
{
    public class CommentController : ApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/comment/getComments")]
        public CommentsResponse GetComments(int objectId, int objectTypeId, string searchString)
        {
            return _commentService.GetComments(objectId, objectTypeId, searchString);
        }

        [Authorize]
        [HttpPost]
        [Route("api/comment/setComment")]
        public CommentResponse SetComment(SetCommentRequest setCommentRequest)
        {
            return _commentService.SetComment(setCommentRequest);
        }
    }
}