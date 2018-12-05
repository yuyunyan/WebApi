using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.RFQ
{
    public class RfqDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            //rfqSet
            {-1, "Insert Rfq failed."},
            {-2, "Update rfq failed."},
            {-3, "Missing UserID" },
            {-4, "Missing AccountID or ContactID" },
            {-5, "StatusID is missing" },
            //rfqLineSet
            {-6, "Missing RFQID"},
            {-7, "Error in inserting new rfq line"},
            {-8, "ItemID is required"},
            {-9, "Error updating rfq line"},
            {-10, "Line items on old versions of a sales order cannot be updated"},
            //rfqDelete
            {-11, "Missing JSON list of Rfq Lines to be deleted"},
            //rfqResponseSet
            {-12, "Missing RFQID"},
            //rfqResponseDelete
            {-13, "Missing JSON list of vendor response Lines to be deleted"},
            {-14, "Missing RFQLineId"}
    };
    }
}
