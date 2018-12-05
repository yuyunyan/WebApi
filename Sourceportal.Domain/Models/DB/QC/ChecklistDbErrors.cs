using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.QC
{
    public class ChecklistDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            //ChecklistSet
            {-1, "UserID is required."},
            {-2, "Error in checklist insert."},
            {-3, "Error in checklist update."},
            //ChecklistQuestionSet
            {-4, "Invalid QuestionID for new version insert."},
            {-5, "New qc question version insert failed."},
            {-6, "New qc question insert failed."},
            //Checklist Association Set
            {-7, "New checklist association insert failed."},
            //Checklist Association Delete
            {-8, "Missing ObjectID or ObjectTypeID for delete checklist association."},
            {-9, "Missing checklistId for delete checklist." },
            {-10, "Delete checklist association failed."}
        };
    }
}
