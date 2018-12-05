using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.SalesOrders
{
    [DataContract]
    public class SetSalesOrderExtraResponse
    {
        [DataMember(Name = "soExtraId")]
        public int SOExtraId { get; set; }
    }
}
