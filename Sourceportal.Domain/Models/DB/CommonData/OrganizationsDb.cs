using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
    public class OrganizationsDb
    {
        public int OrganizationID { get; set; }
        public int ParentOrgID { get; set; }
        public string Name { get; set; }
        public string ExternalID { get; set; }

    }
}
