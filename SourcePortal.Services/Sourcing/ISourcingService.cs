using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.Sourcing;
using Sourceportal.Domain.Models.API.Requests.Sourcing;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.PurchaseOrders;

namespace SourcePortal.Services.Sourcing
{
    public interface ISourcingService
    {
        SourcingQuoteLinesListResponse GetSourcingQuoteLines(SourcingQuoteLinesFilter sourcingFilter);
        SourcingStatusesListResponse GetSourcingStatuses();
        SourceTypesListResponse GetSourceTypes();
        SourceListResponse GetSourceList(int itemId, string partNumber, int objectId, int objectTypeId , bool showAll,bool showInventory);
        SetSourceResponse SetSource(SetSourceRequest setSourceRequest);
        BaseResponse SetSourceStatus(SetSourceStatus setSourceStatus);
        SourceCommentUIDResponse GetSourceCommentUID(SourceCommentUIDRequest sourceCommentUidRequest);
        SourcingRouteStatusesResponse GetRouteSatatuses();
        BaseResponse SetBuyerRoutes(SetBuyerRouteRequest setBuyerRouteRequest);
        QuoteLineBuyersGetResponse QuoteLineBuyersGet(int quoteLineId);
        PurchaseOrderDetailsSetResponse SourceToPurchaseOrder(SourceToPORequest request);
        List<SourceGridExportLine> MapSourceLinesToExport(List<SourceResposne> sourceLines);
        List<RTPSourceExportLine> MapRTPSourceLinesToExport(List<SourceResposne> sourceLines, float soPrice);
        SourceHistoryResponse GetSourceLineByAccountId(int accountId,int? contactId=null);
    }
}
