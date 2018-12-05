using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Enum
{
    public enum XlsDataMap
    {
        ItemListBOMPartNumber = 1,
        ItemListBOMManufacturer = 2,
        ItemListBOMCustomerPartNum = 3,
        ItemListBOMQty = 4,
        ItemListBOMTargetPrice = 5,
        ItemListBOMTargetDateCode = 6,
        ItemListBOMAssocAccountID = 7,
        ItemListExcessPartNumber = 8,
        ItemListExcessManufacturer = 9,
        ItemListExcessCustomerPartNum = 10,
        ItemListExcessQty = 11,
        ItemListExcessCost = 12,
        ItemListExcessDateCode = 13,
        ItemListExcessMOQ = 14,
        ItemListExcessSPQ = 15,
    }

    public enum ListTypeID
    {
        BOM = 1,
        Excess = 2
    }
}
