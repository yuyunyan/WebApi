using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class CheckListAssociationsResponse
    {
        [DataMember(Name = "checklistAssociations")]
        public IList<CheckListAssociation> ChecklistAssociations { get; set; }
    }

    [DataContract]
    public class CheckListAssociation
    {
        [DataMember(Name = "linkType")]
        public string LinkType { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "objectTypeID")]
        public int ObjectTypeID { get; set; }

        [DataMember(Name = "objectID")]
        public int ObjectID { get; set; }

    }
}
