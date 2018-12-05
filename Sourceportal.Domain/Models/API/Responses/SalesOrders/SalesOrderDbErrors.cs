using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.SalesOrders
{
    public class SalesOrderDbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            // uspSalesOrderLineSet
            {-1, "Missing SalesOrderID or VersionID to create new record"},
            {-2, "Error inserting new sales order line/extras"},
            {-3, "Missing ItemID/ItemExtraID"},
            {-4, "Error updating record"},
            {-5, "Version is not the latest version for given Sales Order"},
            {-6, "Line items on old versions of a sales-order cannot be updated"},
            {-7, "Missing UserID"},
            // uspSalesOrderSet
            {-8, "Error updating sales order"},
            {-9, "Only the latest version of a SalesOrder can be modified, or SalesOrderID does not exist/is deleted"},
            {-10, "Error creating new version"},
            {-11, "Invalid version number when attempting to create new version"},
            {-12, "SalesOrder Extras that are not on the latest version of the SalesOrder cannot be updated"},
            //uspSalesOrderLineDelete
            {-13, "SalesOrder Line Delete failed."},
            //uspSalesOrderExtraDelete
            {-14, "SalesOrder Extra Delete failed."}
        };
    }
}
