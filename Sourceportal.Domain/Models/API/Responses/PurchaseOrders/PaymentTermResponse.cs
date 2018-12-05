using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    [DataContract]

    public class PaymentTermsListResponse : BaseResponse
    {
        [DataMember(Name = "paymentTerms")]
        public IList<PaymentTermResponse> PaymentTerms { get; set; }
    }
    public class PaymentTermResponse
    {
        [DataMember(Name = "paymentTermId")]
        public int PaymentTermID { get; set; }

        [DataMember(Name = "termName")]
        public string TermName { get; set; }

        [DataMember(Name = "termDesc")]
        public string TermDesc { get; set; }

        [DataMember(Name = "netDueDays")]
        public string NetDueDays { get; set; }

        [DataMember(Name = "discountDays")]
        public string DiscountDays { get; set; }

        [DataMember(Name = "discountPercent")]
        public double DiscountPercent { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

    }
}
