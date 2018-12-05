using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Shared
{
    public class AccountTypeDetails
    {
        public int PaymentTermID { get; set; }
        public string CurrencyID { get; set; }
        public decimal CreditLimit { get; set; }
        public string EpdsId { get; set; }
    }
}
