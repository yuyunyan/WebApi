using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    [DataContract]
   public class PackagingOptionsResponse
    {
      [DataMember(Name = "packagingTypes")]
      public List<PackagingResponse> PackagingList { get; set; }

      [DataMember(Name = "conditionTypes")]
      public List<PackagingResponse> ConditionList { get; set; }
    }

    [DataContract]
    public class PackagingResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
