using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class UploadBOMResponse
    {
        [DataMember(Name = "itemListId")]
        public int ItemListId { get; set; }
    }
}
