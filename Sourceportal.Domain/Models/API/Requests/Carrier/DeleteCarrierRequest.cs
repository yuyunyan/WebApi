using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Carrier
{
   public class DeleteCarrierRequest
    {
        public int CarrierID { get; set; }
        public int AccountID { get; set; }
    }
}
