using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
   public class CarrierMethodDb
    {
        public int MethodId { get; set; }
        public string MethodName { get; set; }
        public int CarrierId { get; set; }
    }
}
