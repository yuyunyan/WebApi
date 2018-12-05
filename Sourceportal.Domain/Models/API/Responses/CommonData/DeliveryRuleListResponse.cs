using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class DeliveryRuleListResponse
    {
        [DataMember(Name = "deliveryRuleList")]
        public List<DeliveryRuleResponse> DeliveryRuleList { get; set; }
    }

    [DataContract]
    public class DeliveryRuleResponse
    {
        [DataMember(Name = "deliveryRuleId")]
        public int DeliveryRuleId { get; set; }
        [DataMember(Name = "deliveryRuleName")]
        public string DeliveryRuleName { get; set; }
    }
}
