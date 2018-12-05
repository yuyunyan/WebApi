using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.QC
{
    [DataContract]
    public class ChecklistAssociationSetRequest
    {
        [DataMember(Name = "checklistId")]
        public int ChecklistId { get; set; }

        [DataMember(Name = "objectID")]
        public int ObjectID { get; set; }

        [DataMember(Name = "objectTypeID")]
        public string ObjectTypeID { get; set; }
    }
}
