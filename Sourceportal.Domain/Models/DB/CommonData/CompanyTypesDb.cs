using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.CommonData
{
    [DataContract]
    public class CompanyTypesDb
    {
        public int CompanyTypeID { get; set; }
        
        public string Name { get; set; }
        
        public string ExternalId { get; set; }
    }
}
