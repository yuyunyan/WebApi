using Sourceportal.Domain.Models.API.Requests.Shipments;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;
using SourcePortal.Services.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Sourceportal.API.Controllers
{
    public class ShipmentController : ApiController
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpPost]
        [Route("api/shipments/outbounddelivery")]
        public BaseResponse OutboundDeliveryRelease(OutboundDeliverySapRequest request)
        {
            UserHelper.SetMiddlewareUser();
            BaseResponse response = new BaseResponse();

            try
            {
                response = _shipmentService.HandleOutboundDelivery(request);
            }
            catch (Exception e)
            {
                return new BaseResponse { ErrorMessage = String.Format("Exception caught in Web Api Update: {0} | | STACK TRACE: {1}", e.Message, e.StackTrace), IsSuccess = false };
            }

            return response;
        }
    }
}