using System;

namespace Sourceportal.DB.Enum
{
    [Flags]
    public enum ObjectType
    {
        Accounts = 1,
        Contact = 2,
        Deleted = 4,
        Navigation = 8,
        Salesorder = 16,
        SalesorderDetail = 17,
        SalesorderExtra = 18,
        Quote = 19,
        QuoteDetail = 20,
        QuoteExtra = 21,
        Purchaseorder = 22,
        PurchaseorderLine = 23,
        PurchaseorderExtra = 24,
        VendorRfq = 27,
        VendorRfqLine = 28,
        User = 32,
        Usergroup = 64,
        ObjectTypeId = 19,
        AccountTypeCustomerId = 4,
        Inspection = 104,
        Answer = 105,
        Source = 106,
        SourcesJoin = 108,
        Inventory = 107,
        ItemList = 25,
        Item = 103
    }
}
