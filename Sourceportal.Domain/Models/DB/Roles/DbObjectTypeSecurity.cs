using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Roles
{
    public class DbObjectTypeSecurity
    {
        public int FilterTypeID { get; set; }
        public int FilterObjectTypeID { get; set; }
        public int TypeSecurityID { get; set; }
        public int ObjectTypeID { get; set; }
        public string TypeDescription { get; set; }
    }
}
