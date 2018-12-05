using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Services.ErrorManagement
{
    public class ExceptionDTO
    {
        public Exception Exception { get; set; }
        public HttpRequestMessage Request { get; set; }
        public int? ApplicationId { get; set; }
    }
}
