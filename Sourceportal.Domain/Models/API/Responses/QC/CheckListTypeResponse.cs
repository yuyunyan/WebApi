using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
   [DataContract]
   public class CheckListTypeResponse
    {
      [DataMember(Name = ("checkListTypes"))]
      public List<CheckListType> CheckListTypes { get; set; }
    }

    [DataContract]
    public class CheckListType
    {
        [DataMember(Name = "typeId")]
        public int TypeId { get; set; }

        [DataMember(Name = "typeName")]
        public string TypeName { get; set; }
    }
}
