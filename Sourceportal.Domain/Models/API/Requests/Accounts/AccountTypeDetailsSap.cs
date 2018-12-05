using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    [DataContract]
    public class AccountTypeDetailsSap
    {
        [DataMember(Name = "currencyID")]
        public string CurrencyID { get; set; }

        [DataMember(Name = "paymentTermID")]
        public string PaymentTermID { get; set; }

        [DataMember(Name = "creditLimit")]
        public decimal CreditLimit { get; set; }

        [DataMember(Name = "epdsId")]
        public string EpdsId { get; set; }
    }
}
