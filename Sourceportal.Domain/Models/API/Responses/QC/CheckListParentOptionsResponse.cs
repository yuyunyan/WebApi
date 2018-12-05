using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
   [DataContract]
   public class CheckListParentOptionsResponse
    {
        [DataMember(Name = "parentOptions")]
        public IList<ParentOptonsResponse> CheckListParent { get; set; }
    }

    [DataContract]
    public class ParentOptonsResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
