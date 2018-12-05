using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Carrier
{
   public class AccountCarrierInsertUpdateRequest
    {
        public int AccountID { get; set; }
        public int CarrierID { get; set; }
        public string AccountNumber { get; set; }
        public string CarrierName { get; set; }
        public bool IsDefault { get; set; }
    }
}
