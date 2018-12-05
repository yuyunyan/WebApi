using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
   public class AccountByObjectTypeDb
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int AccountTypeId { get; set; }
        public string StatusName { get; set; }
        public string SupplierRating { get; set; }
    }
}
