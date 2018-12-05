using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    public class QuoteDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            //QuoteDetailGet/Set
            {-1, "Missing QuoteID or VersionID to get Detail"},
            {-2, "New version insert failed"},
            {-3, "New quote insert failed"},
            {-4, "Update failed, check QuoteID and VersionID"},
            {-5, "Insert of new quote or new version failed, no provided or default Status"},
            {-6, "UserID is required"},
            {-7, "Invalid QuoteID for new version insert"},
            //QuoteLienDelete
            {-8, "Missing JSON list of Quote Lines to be deleted"},
            //QuoteLineSet
            {-9, "Missing QuoteID or QuoteVersionID for new quote line record"},
            {-10, "Error inserting new quote line"},
            {-11, "Missing both ItemID and PartNumber, at least one must be provided"},
            {-12, "Error updating quote line record"},
            {-13, "QuoteVersionID is not the latest version for the given QuoteID"},
            {-14 , "Line items on old versions of a quote cannot be updated"},
            {-15, "StatusID is required"},
            //QuoteExtraSet
            {-16, "Error on Insert Quote Extra"},
            {-17, "Quote Extras that are not on the latest version of the quote cannot be updated"},
            {-18, "Error on Update Quote Extra"},
            {-19, "@ItemExtraID is required"},
            //QuoteToSO
            {-20, "QuoteID is required to create Sales order from quote"},
            {-21, "Cannot find valid verison of QuoteID provided"},
            {-22, "No default Status is configured for Sales Orders"},
            {-23, "No default Status is configured for Sales Order Lines"},
            {-24, "No default Status is configured for Sales Order Extras"}
        };
    }
}
