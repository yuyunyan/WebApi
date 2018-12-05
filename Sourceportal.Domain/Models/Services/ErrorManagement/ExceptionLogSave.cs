using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Services.ErrorManagement
{
    public class ExceptionLogSave
    {
        public DateTime TimeStamp { get; set; }
        public string Url { get; set; }
        public string PostData { get; set; }
        public int UserId { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionType { get; set; }
        public string InnerException { get; set; }
        public int ApplicationId { get; set; }
    }
}
