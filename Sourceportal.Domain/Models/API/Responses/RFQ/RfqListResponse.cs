using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.RFQ
{
    [DataContract]
    public class RfqListResponse
    {
        [DataMember(Name = "rfqList")]
        public List<RfqDetailsResponse> RfqList;
        [DataMember(Name = "rowCount")]
        public int RowCount;
    }
}
