using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests
{
    public class ValidatePasswordRequest
    {
        public string emailaddress { get; set; }
        public string password { get; set; }
    }
}
