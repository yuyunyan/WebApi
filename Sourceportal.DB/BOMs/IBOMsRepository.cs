using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.BOMs;
using Sourceportal.Domain.Models.DB.BOMs;
using Sourceportal.Domain.Models.DB.PurchaseOrders;

namespace Sourceportal.DB.BOMs
{
    public interface IBOMsRepository
    {
        bool SaveXlsDataAccountMap(BOMBody bomBody, List<int> xlsDataMapIdList);
        int UploadListBOMBodySet(BOMBody bomBody, List<ItemListLineBOMRequest> itemListLines);
        int UploadListExcessBodySet(BOMBody bomBody, List<ItemListLineExcessRequest> itemListLines);
        List<BOMListDbs> GetBOMList(SearchFilter searchFilter);
        int ProcessMatch(ProcessMatchRequest processMatchRequest);
        IList<SalesOrderDbs> GetSalesOrder(BomSearchRequest bomSearchRequest);
        IList<InventoryDbs> GetInventory(BomSearchRequest bomSearchRequest);
        IList<OutsideOffersDbs> GetOutsideOffers(BomSearchRequest bomSearchRequest);
        IList<VendorQuotesDbs> GetVendorQuotes(BomSearchRequest bomSearchRequest);
        IList<PurchaseOrderLineBom> GetPurchaseOrders(BomSearchRequest bomSearchRequest);
        IList<EMSLineBom> GetEMSs(BomSearchRequest bomSearchRequest);
        IList<CustomerRFQLineBom> GetCustomerQuotes(BomSearchRequest bomSearchRequest);
        IList<CustomerRFQLineBom> GetCustomerRfqs(BomSearchRequest bomSearchRequest);
        IList<ResultSummaryDbs> GetResultSummary(BomSearchRequest bomSearchRequest);
        IList<BomSearchResultDbs> GetPartSearchResult(string searchString, string searchType);
        IList<AvailabilityPartDbs> GetAvailabilityPart(int itemId);
    }
}
