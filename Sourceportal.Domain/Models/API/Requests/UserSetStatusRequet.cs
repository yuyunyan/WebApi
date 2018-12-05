using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests
{
    public class UserSetStatusRequet
    {
      public int userId { get; set; }
      public bool isEnabled { get; set; }
    }
}
