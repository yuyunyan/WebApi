using Sourceportal.Domain.Models.DB.Quotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.Quotes;
using Sourceportal.Domain.Models.API.Responses.Quotes;
using Sourceportal.Domain.Models.API.Requests;

namespace Sourceportal.DB.Quotes
{
    public interface IQuoteRepository
    {
        QuoteDetailsDb GetQuoteDetails(int quoteId, int versionId);
        QuoteDetailsDb SetQuoteDetails(SetQuoteDetailsRequest setQuoteDetailsRequest);
        QuoteHeaderDb GetQuoteHeader(int quoteId, int versionId);
        IList<CustomerAccountDb> GetAllCustomers();
        IList<AccountContactDb> GetAccountTypeContacts(int accountId);
        IList<AccountShipAddressDb> GetAccountTypeAddress(int accountId);
        IList<QuoteStautsDb> GetAllQuoteStatus();
        IList<PartsListDb> GetAllParts(QuotePartsFilter filter);
        PartsListDb SetPartList(PartDetails partDetails);
        
        List<CommodityOptionsDb> GetCommodityOptions();
        List<PackagingOptionsDb> GetPackaingOptions();
        List<PackageConditionsDb> GetConditionOptions(); 
        List<QuoteExtraDb> GetQuoteExtra(int quoteId, int quoteVersionId, int rowOffset, int rowLimit);
        QuoteExtraDb setQuoteExtra(SetQuoteExtraRequest setQuoteExtraRequest);
        List<QuoteListDb> getQuoteList(SearchFilter searchfilter);
        int DeleteQuoteParts(List<int> quoteLineIds);
        NewSalesOrderDb QuoteToSalesOrder(QuoteToSORequest quoteToSoRequest);
        List<PartsListDb> SetPartLists(int quoteId, int versionId, List<SetPartsListRequest> quoteParts);
        int RouteQuoteLines(RouteQuoteLineRequest routeQuoteLineRequest);
        List<PackagingOptionsDb> GetPackagingOption(int packagingId);
        IList<QuoteTypesDb> GetAllQuoteTypes();
        int SetQuoteLinePrint(QuotePrint quotePrint);
        IList<QuoteLineHistoryDb> GetQuoteLineByAccountId(int accountId, int? contactId=null);
    }
}
