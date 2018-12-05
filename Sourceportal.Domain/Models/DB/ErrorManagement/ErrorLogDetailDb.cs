using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.ErrorManagement
{
    public class ErrorLogDetailDb : ErrorLogDb
    {
        public string StackTrace { get; set; }
        public string PostData { get; set; }
        public string InnerExceptionMessage { get; set; }
    }
}
