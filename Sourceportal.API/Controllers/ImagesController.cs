using System.Web.Http;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Images;
using Sourceportal.Domain.Models.Shared;
using SourcePortal.Services.Images;

namespace Sourceportal.API.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly IFileService _fileService;

        public ImagesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/images/saveAnswerImage")]
        public BaseResponse SaveImage(int answerId, int inspectionId)
        {
            if (!_fileService.RequestContainsImages())
                return new BaseResponse { IsSuccess = false, ErrorMessage = "File type must be an image. " };

            var path = string.Format(DocumentPaths.InspectionAnswerImages, inspectionId, answerId );
            var status = _fileService.SaveFile(path, ObjectType.Answer, answerId, true);
            return new BaseResponse { IsSuccess = status, ErrorMessage = status ? null: "Image save failed"};
        }

        [Authorize]
        [HttpPost]
        [Route("api/images/saveInspectionImage")]
        public BaseResponse SaveImage(int inspectionId)
        {
            if (!_fileService.RequestContainsImages())
                return new BaseResponse { IsSuccess = false, ErrorMessage = "File type must be an image. " };

            var path = string.Format(DocumentPaths.InspectionAdditionalImages, inspectionId);
            var status = _fileService.SaveFile(path, ObjectType.Inspection, inspectionId);
            return new BaseResponse { IsSuccess = status, ErrorMessage = status ? null : "Image save failed" };
        }

        [Authorize]
        [HttpGet]
        [Route("api/images/getAnswerImages")]
        public FileResponse GetAnswerImages(int inspectionId, int answerId)
        {
            var path = string.Format(DocumentPaths.InspectionAnswerImages, inspectionId, answerId);
            return _fileService.GetFiles(path, ObjectType.Answer, answerId, (int)DocumentType.Image);
        }

        [Authorize]
        [HttpGet]
        [Route("api/images/getAdditionalImages")]
        public FileResponse GetAdditionalImages(int inspectionId)
        {
            var path = string.Format(DocumentPaths.InspectionAdditionalImages, inspectionId);
            return _fileService.GetFiles(path, ObjectType.Inspection, inspectionId, (int)DocumentType.Image);
        }

        [Authorize]
        [HttpPost]
        [Route("api/images/deleteImage")]
        public BaseResponse DeleteImages(int imageId)
        {
            var result = _fileService.DeleteDocument(imageId);
            return new BaseResponse{ErrorMessage = result ? null : "delete failed"};
        }

        //[HttpPost]
        //[Route("api/images/saveImage")]
        //public async Task<HttpResponseMessage> Post(int questionId)
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

        //    var provider = new MultipartFormDataStreamProvider(HostingEnvironment.MapPath("~/App_Data"));

        //    var files = await Request.Content.ReadAsMultipartAsync(provider);

        //    // Do something with the files if required, like saving in the DB the paths or whatever
        //    //await DoStuff(files);

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //    ;
        //}
    }
}
