using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Sourcing
{
    
    [DataContract]
    public class SourceTypesListResponse : BaseResponse
    {
        [DataMember(Name = "types")]
        public List<SourceTypesResponse> SourcingStatusesList;

    }
    public class SourceTypesResponse
    {

        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }

        [DataMember(Name = "sourceTypeId")]
        public int SourceTypeID { get; set; }

    }
    
}
