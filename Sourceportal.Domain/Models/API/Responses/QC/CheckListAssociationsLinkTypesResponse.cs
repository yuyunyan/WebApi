using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class CheckListAssociationsLinkTypesResponse
    {
        [DataMember(Name = "checklistAssociationsLinkTypes")]
        public IList<ChecklistAssociationsLinkType> ChecklistAssociationsLinkTypes { get; set; }
    }

    [DataContract]
    public class ChecklistAssociationsLinkType
    {
        [DataMember(Name = "objectName")]
        public string ObjectName { get; set; }

        [DataMember(Name = "objectTypeID")]
        public int ObjectTypeID { get; set; }

        [DataMember(Name = "accountTypeID")]
        public int AccountTypeID { get; set; }
    }
}
