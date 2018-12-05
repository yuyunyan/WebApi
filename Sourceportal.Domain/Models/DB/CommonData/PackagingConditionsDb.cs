using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
    public class PackagingConditionsDb
    {
        public int PackageConditionID { get; set; }
        public string ConditionName { get; set; }
        public string ExternalId { get; set; }
    }
}
