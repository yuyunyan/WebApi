using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.PurchaseOrders;
using Sourceportal.Domain.Models.DB.PurchaseOrders;

namespace Sourceportal.DB.PurchaseOrders
{
    public interface IPurchaseOrderRepository
    {
        List<PurchaseOrderDb> GetPurchaseOrderList(SearchFilter searchFilter);
        PurchaseOrderDb GetPurchaseOrderDetails(int purchaseOrderId, int versionId);
        PurchaseOrderDb GetPurchaseOrderFromExternal(string externalId);
        List<PurchaseOrderLinesDb> GetPurchaseOrderLines(int purchaseOrderId, int purchaseOrderVersionId, SearchFilter searchFilter);
        PurchaseOrderLinesDb SetPurchaseOrderLine(SetPurchaseOrderLineRequest setPurchaseOrderLineRequest);
        int DeletePurchaseOrderLines(List<int> poLineIds);
        List<PurchaseOrderExtraDb> GetPurchaseOrderExtras(int poId, int poVersionId, int rowOffset, int rowLimit);
        PurchaseOrderExtraDb SetPurchaseOrderExtra(SetPurchaseOrderExtraRequest setPurchaseOrderExtraRequest);
        int DeletePurchaseOrderExtras(List<int> poExtraIds);
        PurchaseOrderDb SetPurchaseOrderDetails(SetPurchaseOrderDetailsRequest setPoRequest);
        void UpdateWarehouseOnPurchaseOrderLines(int warehouseId, int purchaseOrderId);
        List<CurrencyDb> GetCurrencies();
        List<PurchaseOrderLinesDb> SetPoLines(int poId, int versionId, List<SetPurchaseOrderLineRequest> poLines);
        string GetManufactuerItem(int itemId);
        void SetExternalId(int poId, string externalId);
        PurchaseOrderDb GetPurchaseOrderDetailsFromLineId(int poLineId);
        string GetProductSpecForPoLine(int poLineId);
        int GetPoLineIdFromExternal(string poExternalId, string poLineNum);
        PurchaseOrderSOAllocation GetPOAllocationFromProductSpec(string productSpec, int poLineId);
        PurchaseOrderSOAllocation GetPOAllocationFromLine(int poLineId);
        IList<PurchaseOrderLineHistoryDb> GetPurchaseOrderByAccountId(int accountId, int? contactId = null);
    }
}
