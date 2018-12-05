using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses.SalesOrders;
using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Domain.Models.API.Responses;


namespace SourcePortal.Services.SalesOrder
{
    public interface ISalesOrderService
    {
        SalesOrderDetailsResponse SetSalesOrderDetails(SalesOrderDetailsRequest request);
        SalesOrderListResponse GetSalesOrderList(SearchFilter searchfilter);
        GetSalesOrderLinesResponse GetSalesOrderLines(int soId, int soVersionId, SearchFilter searchFilter);
        SalesOrderLineDetail SetSalesOrderLines(SalesOrderLineDetail setSalesOrderLinesRequest);
        SalesOrderLinesDeleteResponse DeleteSalesOrderLines(List<int> soLineIds);
        SalesOrderExtraResponse GetSalesOrderExtra(int soId, int soVersionId, int rowOffset, int rowLimit);
        SetSalesOrderExtraResponse SetSalesOrderExtra(SetSalesOrderExtraRequest setSalesOrderExtraRequest);
        SalesOrderExtrasDeleteResponse DeleteSalesOrderExtras(List<int> soExtraIds);
        SalesOrderDetailsResponse GetSalesOrderDetails(int soId, int versionId);
        AccountsResponse GetAccountsList();
        SyncResponse Sync(int soId, int soVersionId);
        void SetSalesOrderDetailsFromSap(SalesOrderDetailsRequest request);
        BaseResponse HandlingIncomingSalesOrderSapUpdate(SalesOrderIncomingSapResponse request);
        SalesOrderLineListResponse GetSalesOrderLineByAccountId(int accountId, int? contactId=null);
    }
}
