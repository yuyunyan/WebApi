using Sourceportal.Domain.Models.API.Requests.Carrier;
using Sourceportal.Domain.Models.DB.Carrier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Carrier
{
   public interface ICarrierRepository
    {
        List<AccountCarrierDb> GetAccountCarriers(int accountId);
        bool DeleteAccountCarrier(DeleteCarrierRequest deleteCarrierRequest);
        List<AccountCarrierDb> GetCarrier();
        int CarrierSet(AccountCarrierInsertUpdateRequest accountCarrierSet);
        AccountCarrierDb GetAccountNumber(int accountId, int carrierId);
    }
}
