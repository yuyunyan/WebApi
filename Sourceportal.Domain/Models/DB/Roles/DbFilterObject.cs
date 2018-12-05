using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Roles
{
    public class DbFilterObject
    {
        public int ObjectID { get; set; }
        public int ObjectTypeID { get; set; }
        public string ObjectName { get; set; }
    }
}
