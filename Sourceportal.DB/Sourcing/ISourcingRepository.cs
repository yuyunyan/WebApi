using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.DB.Sourcing;
using Sourceportal.Domain.Models.API.Requests.Sourcing;
using Sourceportal.Domain.Models.API.Responses.Quotes;

namespace Sourceportal.DB.Sourcing
{
    public interface ISourcingRepository
    {
        List<SourcingQuoteListDb> GetSourcingQuoteList(SourcingQuoteLinesFilter sourceFilter);
        List<SourcingStatusesDb> GetSourcingStatuses();
        List<SourceTypesDb> GetSourceTypes();
        List<SourceListDb> GetSourceList(int itemId, string partNumber, int objectId, int objectTypeId , bool showAll,bool showInventory);
        SourceListDb SetSource(SetSourceRequest setSourceRequest);
        bool SetSourceStatus(SetSourceStatus setSourceStatus);
        SourceCommentUIDDb GetSourceCommentUID(SourceCommentUIDRequest sourceCommentUIdRequest);
        List<RouteStatusDb> GetRouteStatuses();
        int SetBuyerRoute(SetBuyerRouteRequest setBuyerRouteRequest);
        List<BuyerNameDb> QuoteLineRouteBuyersGet(int quoteLineId);
        int SourceToPurchaseOrder(SourceToPORequest request);
        List<SourceListDb> GetSourceByIds(PartDetails[] sourceLine);
        List<SourceLineDb> GetSourceLineByAccountId(int accountId,int? contactId=null);
    }
}
