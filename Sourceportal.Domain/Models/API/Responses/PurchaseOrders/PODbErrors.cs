using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    public class PODbErrors
    {
        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>
        {
            //PurchaseOrderSet
            {-1, "Invalid PurchaseOrderID for new PO version"},
            {-2, "Error creating new version"},
            {-3, "Error creating new PO"},
            {-4, "Error updating PO"},
            {-5, "No default status for new PO configured.  Update the lkpStatuses table with a default status for POs"},
            {-6, "UserID is required"},
            {-22, "Supplier and Contact are required." },
            {-23, "Ship to and Ship from address are required." },
            {-24, "Currency is required."},
            {-25, "Order Date is required." },
            //PurchaseOrderGet
            {-7, "-7 Invalid PurchaseOrderID or VersionID to get PO header"},
            //POLineSet
            {-8, "Missing PurchaseOrderID or POVersionID for new po line record"},
            {-9, "Error inserting new Purchase order line"},
            {-10, "Item is required"},
            {-11, "Error updating po line record"},
            {-12, "VersionID is not the latest version for the given Purchase Order"},
            {-13, "Line items on old versions of a Purchase order cannot be updated"},
            //POLinesDelete
            {-14, "Missing JSON list of Purchase Order Lines to be deleted"},
            //POExtraSet
            {-15, "Missing PurchaseOrderID or VersionID for to create a new po extra record"},
            {-16, "The POVersionID provided is not the latest POVersionID for the given PurchaseOrderID"},
            {-17, "Error on po extra Insert"},
            {-18, "Purchase Order Extras that are not on the latest version of the Purchase Order cannot be updated"},
            {-19, "Error on po extra Update"},
            {-20, "ItemExtraID is required"},
            //POExtraDelete
            {-21, "Missing JSON list of Purchase Order Extras to be deleted"}
        };
    }
}
