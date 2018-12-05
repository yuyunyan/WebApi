using Sourceportal.Domain.Models.API.Requests.Carrier;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Carrier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sourceportal.Domain.Models.API.Responses.Carrier.CarrierListRespose;

namespace SourcePortal.Services.Carrier
{
   public interface ICarrierService
    {
        AccountCarrierListResponse GetAccountCarrierList(int accountId);
        BaseResponse DeleteAccountCarrier(DeleteCarrierRequest deleteCarrierRequest);
        CarriersListResponse GetCarrierList();
        BaseResponse AccountCarrierSet(AccountCarrierInsertUpdateRequest accountCarrierSet);
        AccountCarrierResponse GetAccountNumber(int accountId, int carrierId);
    }
}
