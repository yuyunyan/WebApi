using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ManufacturerListResponse : BaseResponse
    {
        [DataMember(Name = "manufacturers")]
        public IList<ManufacturerResponse> Manufacturers { get; set; }
    }

    [DataContract]
    public class ManufacturerResponse
    {

        [DataMember(Name = "mfrId")]
        public int MfrId { get; set; }

        [DataMember(Name = "mfrName")]
        public string MfrName { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "mfrUrl")]
        public string MfrURL { get; set; }
        
        [DataMember(Name = "errorMessage")]
        public string ErrorMessage;
    }
}
