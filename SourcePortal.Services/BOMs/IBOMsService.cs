using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.BOMs;
using Sourceportal.Domain.Models.API.Responses.BOMs;
using System.Collections.Generic;

namespace SourcePortal.Services.BOMs
{
    public interface IBOMsService
    {
        UploadBOMResponse UploadListXlsFile();
        BOMListResponse GetBOMList(SearchFilter searchFilter);
        BomProcessMatchResponse ProcessMatch(ProcessMatchRequest processMatchRequest);
        SalesOrderResponse GetSalesOrder(BomSearchRequest bomSearchRequest);
        InventoryBomResponse GetInventory(BomSearchRequest bomSearchRequest);
        OutsideOffersBomResponse GetOutsideOffers(BomSearchRequest bomSearchRequest);
        VendorQuotesBomResponse GetVendorQuotes(BomSearchRequest bomSearchRequest);
        PurchaseOrderBomResponse GetPurchaseOrders(BomSearchRequest bomSearchRequest);
        CustomerQuoteBomResponse GetCustomerQuotes(BomSearchRequest bomSearchRequest);
        CustomerRFQBomResponse GetCustomerRfqs(BomSearchRequest bomSearchRequest);
        EMSBomResponse GetEMSs(BomSearchRequest bomSearchRequest);
        ResultSummaryResponse GetResultSummary(BomSearchRequest bomSearchRequest);
        PartSearchResultResponse GetPartSearchResult(string searchString, string searchType);
        PartSearchAvailabilityResponse GetAvailabilityPart(int itemId);
        List<AllocationsResponse> MapAllocationJsonToObject(string allocaionJson);
    }
}
