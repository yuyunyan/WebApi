using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses;


namespace Sourceportal.API.Controllers
{
    public class ObjectTypeController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("api/objectTypes/getQuoteObjectTypeId")]
        public ObjectTypeIdResponse GetObjectTypeId()
        {
            return new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.Quote) };
        }

        [Authorize]
        [HttpGet]
        [Route("api/objectTypes/getSoObjectTypeId")]
        public ObjectTypeIdResponse GetSoObjectTypeId()
        {
            return new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.Salesorder) };
        }

        [Authorize]
        [HttpGet]
        [Route("api/objectTypes/getSoLinesObjectTypeId")]
        public ObjectTypeIdResponse GetSoLinesObjectTypeId()
        {
            return new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.SalesorderDetail) }; //nothing but sales order lines
        }

        [Authorize]
        [HttpGet]
        [Route("api/objectTypes/getPoObjectTypeId")]
        public ObjectTypeIdResponse GetPoObjectTypeId()
        {
            return new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.Purchaseorder) };
        }

        [Authorize]
        [HttpGet]
        [Route("api/objectTypes/getRfqObjectTypeId")]
        public ObjectTypeIdResponse GetRfqObjectTypeId()
        {
            return new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.VendorRfq) };
        }

        [Authorize]
        [HttpGet]
        [Route("api/objectTypes/getSourceObjectTypeId")]
        public ObjectTypeIdResponse GetSourceObjectTypeId()
        {
            return new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.Source) };
        }

        [Authorize]
        [HttpGet]
        [Route("api/objectTypes/getAccountObjectTypeId")]
        public ObjectTypeIdResponse GetAccountObjectTypeId()
        {
            var response = new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.Accounts) };
            return new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.Accounts) };
        }

        [Authorize]
        [HttpGet]
        [Route("api/objectTypes/getUserObjectTypeId")]
        public ObjectTypeIdResponse GetUserObjectTypeId()
        {
            var response = new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.User) };
            return response;
          //  return new ObjectTypeIdResponse { ObjectTypeId = Convert.ToInt32(ObjectType.User) };
        }

    }
}
