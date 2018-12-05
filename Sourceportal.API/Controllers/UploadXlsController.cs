using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Sourceportal.Domain.Models.API.Responses.UploadXls;
using SourcePortal.Services.UploadXls;

namespace Sourceportal.API.Controllers
{
    public class UploadXlsController : ApiController
    {
        private readonly IUploadXlsService _uploadXlsService;

        public UploadXlsController(IUploadXlsService UploadXlsService)
        {
            _uploadXlsService = UploadXlsService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/uploads/xlsDataMapGet")]
        public XlsDataMapsGetResponse XlsDataMapGet(string xlsType, int itemListTypeID)
        {
            return _uploadXlsService.XlsDataMapGet(xlsType, itemListTypeID);
        }

        [Authorize]
        [HttpGet]
        [Route("api/uploads/accountDataMapsGet")]
        public XlsAccountGetResponse XlsAccountGet(int accountId, string xlsType)
        {
            return _uploadXlsService.XlsAccountGet(accountId, xlsType);
        }
    }
}