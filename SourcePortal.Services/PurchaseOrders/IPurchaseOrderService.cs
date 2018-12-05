using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.PurchaseOrders;
using Sourceportal.Domain.Models.API.Responses.PurchaseOrders;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Domain.Models.API.Responses;

namespace SourcePortal.Services.PurchaseOrders
{
    public interface IPurchaseOrderService
    {
        PurchaseOrderListResponse GetPurchaseOrderList(SearchFilter searchFilter);
        PurchaseOrderResponse GetPurchaseOrderDetails(int purchaseOrderId, int versionId, bool checkForPendingTransactions);
        GetPurchaseOrderLinesResponse GetPurchaseOrderLines(int poId, int poVersionId, SearchFilter searchFilter);
        PurchaseOrderLineDetail SetPurchaseOrderLine(SetPurchaseOrderLineRequest setPurchaseOrderLineRequest);
        PurchaseOrderLinesDeleteResponse DeletePurchaseOrderLines(List<int> poLineIds);
        PurchaseOrderExtraResponse GetPurchaseOrderExtra(int poId, int poVersionId, int rowOffset, int rowLimit);
        SetPurchaseOrderExtraResponse SetPurchaseOrderExtra(SetPurchaseOrderExtraRequest setPurchaseOrderExtraRequest);
        PurchaseOrderExtraDeleteResponse DeletePurchaseOrderExtras(List<int> poExtraIds);
        PurchaseOrderDetailsSetResponse SetPurchaseOrderDetails(SetPurchaseOrderDetailsRequest setPoRequest);
        CurrencyListResponse GetCurrencies();
        PurchaseOrderDetailsSetResponse PurchaseOrderFromFlaggedSet(SetPurchaseItemsFlaggedRequest setPurchaseItemsFlaggedRequest);
        ItemMfrResponse GetManufactuerItem(int itemId);
        SyncResponse Sync(int poId, int poVersionId);
        void SetPurchaseOrderSapData(SetPurchaseOrderSapDataRequest request);
        BaseResponse HandlingIncomingPurchaseOrderSapUpdate(PurchaseOrderIncomingSapResponse request);
        string ValidateSync(int purchaseOrderId, int versionId);
        PurchaseOrderLineHistoryResponse GetPurchaseOrderByAccountId(int accountId,int? contactId=null);
    }
}
