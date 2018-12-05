using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Documents
{
    public class DocumentDownloadResponse
    {
        [DataMember(Name = "downloadName")]
        public string DownloadName { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }
        

    }
}
