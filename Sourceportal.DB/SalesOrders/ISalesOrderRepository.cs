using System.Collections.Generic;
using Sourceportal.Domain.Models.DB.SalesOrders;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.SalesOrders;
using Sourceportal.Domain.Models.API.Responses.SalesOrders;

namespace Sourceportal.DB.SalesOrders
{
    public interface ISalesOrderRepository
    {
        int GetItemIdFromExternal(string externalId);
        List<SalesOrderDb> GetSalesOrderList(SearchFilter searchFilter);
        SalesOrderDetailsDb SetSalesOrderDetails(SalesOrderDetailsRequest request);
        List<SalesOrderLinesDb> GetSalesOrderLines(int salesOrderId, int salesOrderVersionId, SearchFilter searchFilter);
        SalesOrderLinesDb SetSalesOrderLines(SalesOrderLineDetail setSalesOrderLinesRequest);
        int DeleteSalesOrderLines(List<int> soLineIds);
        string GetProductSpecId(int soLineId);
        SalesOrderDetailsDb GetSalesOrderFromLine(int soLineId);
        List<SalesOrderExtraDb> GetSalesOrderExtra(int salesOrderId, int salesOrderVersionId, int rowOffset, int rowLimit);
        SalesOrderExtraDb SetSalesOrderExtra(SetSalesOrderExtraRequest setSalesOrderExtraRequest);
        int DeleteSalesOrderExtras(List<int> soExtraIds);
        void SetSalesOrderLinesSapData(SalesOrderLineDetail line);
        SalesOrderDetailsDb GetSalesOrderDetails(int soId, int versionId);
        SalesOrderOrganizationDb GetSalesOrderOrganization(int soId, int versionId);
        SalesOrderDetailsDb GetSalesOrderFromExternal(string externalId);
        int GetSoLineIdFromExternal(string soExternalId, string soLineNum);
        int GetSoLineIdFromProductSpec(string productSpec);
        List<SalesOrderLinesDb> GetSoLinesFromProductSpec(string productSpec);
        IList<SalesOrderLineHistoryDb> GetSalesOrderLineByAccountId(int accountId,int? contactId=null);
        SalesOrderLinesDb GetSalesOrderLine(int soLineId);
    }
}
