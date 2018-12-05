using System;
using System.Collections.Generic;

namespace Sourceportal.Domain.Models.DB.QC
{
    
   public class ChecklistDb
    {
        public int ChecklistId { get; set; }
        public int ParentChecklistId { get; set; }
        public string ChecklistName { get; set; }
        public string ChecklistDescription { get; set; }
        public int ChecklistTypeId { get; set; }
        public int SortOrder { get; set; }
        public string EffectiveStartDate { get; set; }
        public string ChecklistTypeName { get; set; }
        public bool IsDeleted { get; set; }
        public bool AddedByUser { get; set; }
        public IList<InspectionQuestionDb> Questions {get;set;}

    }
}
