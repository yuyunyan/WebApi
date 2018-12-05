using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
    public class FreightPaymentMethodDb
    {
        public int FreightPaymentMethodID { get; set; }
        public string MethodName { get; set; }
        public int? ExternalID { get; set; }
        public bool UseAccountNum { get; set; }

    }
}
