using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
   public class ChecklistResponse
    {
        [DataMember(Name = "checkLists")]
        public IList<Checklist> CheckLists { get; set; }

    }

    [DataContract]
    public class Checklist
    {
        [DataMember(Name = "checklistId")]
        public int ChecklistId { get; set; }

        [DataMember(Name = "parentChecklistId")]
        public int ParentChecklistId { get; set; }

        [DataMember(Name = "checklistName")]
        public string ChecklistName { get; set; }

        [DataMember(Name = "checklistDescription")]
        public string ChecklistDescription { get; set; }

        [DataMember(Name = "checklistTypeId")]
        public int ChecklistTypeId { get; set; }

        [DataMember(Name = "checklistTypeName")]
        public string TypeName { get; set; }

        [DataMember(Name = "sortOrder")]
        public int SortOrder { get; set; }

        [DataMember(Name = "effectiveStartDate")]
        public string EffectiveStartDate { get; set; }

        [DataMember(Name = "isDeleted")]
        public bool IsDeleted { get; set; }
        [DataMember(Name = "childCheckList")]
        public List<Checklist> ChildCheckList { get; set; }


    }
}
