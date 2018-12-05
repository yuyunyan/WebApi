using Sourceportal.Domain.Models.API.Requests.Shipments;
using Sourceportal.Domain.Models.API.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.Shipments
{
    public interface IShipmentService
    {
        BaseResponse HandleOutboundDelivery(OutboundDeliverySapRequest request);
    }
}
