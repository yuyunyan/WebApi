using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    [DataContract]
    public class SourcingStatusesListResponse : BaseResponse
    {
        [DataMember(Name = "statuses")]
        public List<SourcingStatusesResponse> SourcingStatusesList;

    }
    public class SourcingStatusesResponse
    {

        [DataMember(Name = "statusId")]
        public int StatusID { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "isDefault")]
        public int IsDefault { get; set; }

    }
}
