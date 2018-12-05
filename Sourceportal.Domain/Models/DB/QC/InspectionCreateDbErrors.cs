using System.Collections.Generic;

namespace Sourceportal.Domain.Models.DB.QC
{
    public class InspectionCreateDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            {-1, "InventoryID is required"},
            {-2, "Given InventoryID already has an Inspection"},
            {-3, "No default status is configured for QC Inspections"}
        };
    }
}
