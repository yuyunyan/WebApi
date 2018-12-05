using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
  [DataContract]
  public class CheckListSetResponse
    {
        [DataMember(Name = "checklistId")]
        public int ChecklistId { get; set; }

    }
}
