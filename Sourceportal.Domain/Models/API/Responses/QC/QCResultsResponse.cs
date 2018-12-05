using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class QCResultsResponse
    {
        [DataMember(Name = "results")]
      public  IList<QCResult> Results { get; set; }
    }

    [DataContract]
    public class QCResult
    {
        [DataMember(Name = "id")]
        public int ResultID { get; set; }

        [DataMember(Name = "name")]
        public string ResultName { get; set; }
    }
}
