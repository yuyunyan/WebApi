using Sourceportal.Domain.Models.API.Requests.Carrier;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Carrier;
using SourcePortal.Services.Carrier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Sourceportal.Domain.Models.API.Responses.Carrier.CarrierListRespose;

namespace Sourceportal.API.Controllers
{
    public class CarrierController : ApiController
    {
        private readonly ICarrierService _carrierService;
        public CarrierController (ICarrierService carrierService)
        {
            _carrierService = carrierService;
        }

        [HttpGet]
        [Route("api/carrier/AccountCarrierList")]
        public AccountCarrierListResponse AccountCarrierList(int accountId)
        {
            return _carrierService.GetAccountCarrierList(accountId);
        }
        [HttpPost]
        [Route("api/carrier/DeleteAccountCarrier")]
        public BaseResponse DeleteAccountCarrier(DeleteCarrierRequest deleteCarrierRequest)
        {
            return _carrierService.DeleteAccountCarrier(deleteCarrierRequest);
        }
        [HttpPost]
        [Route("api/carrier/AccountCarrierSet")]
        public BaseResponse AccountCarrierSet(AccountCarrierInsertUpdateRequest accountCarrierSet)
        {
             return _carrierService.AccountCarrierSet(accountCarrierSet);
           
        }

        [HttpGet]
        [Route("api/carrier/CarrierList")]
        public CarriersListResponse CarrierList()
        {
            return _carrierService.GetCarrierList();
        }


        [HttpGet]
        [Route("api/carrier/getAccountNumber")]
        public AccountCarrierResponse GetAccountNumber(int accountId, int carrierId)
        {
            return _carrierService.GetAccountNumber(accountId, carrierId);
        }




    }
}
