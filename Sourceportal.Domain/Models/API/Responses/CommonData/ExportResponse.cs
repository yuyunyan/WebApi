using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class ExportResponse
    {
        [DataMember(Name = "downloadUrl")]
        public string DownloadURL { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "errorMsg")]
        public string ErrorMsg { get; set; }
    }
}
