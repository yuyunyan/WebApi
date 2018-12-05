using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Carrier
{
   public class AccountCarrierDb
    {
      public int AccountID { get; set; }
      public string CarrierName { get; set; }
      public int CarrierID { get; set; }
      public bool IsDefault { get; set; }
      public string AccountNumber { get; set; }
    }
}
