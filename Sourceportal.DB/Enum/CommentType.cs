using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Enum
{
    [Flags]
    public enum CommentType
    {
        Introduction = 1,
        TestResult = 2,
        Conclusion = 3,
        Financial = 4,
        Account = 5,
        Comment = 6,
        SalesSalesOrder = 7,
        PurchasingSalesOrder = 8,
        WarehouseSalesOrder = 9,
        SalesOrderLine = 10,
        SalesOrderExtra = 11,
        SalesQuote = 12,
        PurchaseQuote = 13,
        QuotePart = 14,
        QuoteExtra = 15,
        SalesPurchasing = 16,
        PurchasingPurchasing = 17,
        WarehousePurchasing = 18,
        PurchasingLine = 19,
        PurchasingExtra = 20,
        Sourcing = 22,
        SourcesJoin = 25,
        ItemList = 28,
        SalesItemList = 29,
        PurchaseItemList = 30,
        WarehouseItemList = 31,
        CommentRfqResponse = 27,
        SalesRfqResponse = 33,
        PurchaseRfqResponse = 34,
        WarehouseRfqResponse = 35,
    }
}
