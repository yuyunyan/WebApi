using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests
{
    public class UserUpdateRequest
    {
        public int userid { get; set; }
        public string emailaddress { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phoneNumber { get; set; }
        public int organizationid { get; set; }
        public string timezonename { get; set; }
        public bool isenabled { get; set; }
    }
}
