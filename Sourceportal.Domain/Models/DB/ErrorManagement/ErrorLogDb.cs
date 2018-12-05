using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.ErrorManagement
{
    public class ErrorLogDb
    {
        public int ErrorID { get; set; }
        public string URL { get; set; }
        public string ExceptionType { get; set; }
        public string ErrorMessage { get; set; }
        public string Application { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Timestamp { get; set; }
        public int TotalRows { get; set; }
    }
}
