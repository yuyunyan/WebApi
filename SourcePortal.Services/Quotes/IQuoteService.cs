using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Requests.Quotes;
using Sourceportal.Domain.Models.API.Responses.Quotes;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.CommonData;

namespace SourcePortal.Services.Quotes
{
    public interface IQuoteService
    {
        QuoteDetailsResponse GetQuoteDetails(int quoteId, int versionId);
        SetQuoteDetailsResponse SetQuoteDetails(SetQuoteDetailsRequest setQuoteDetailsRequest);
        QuoteHeaderResponse GetQuoteHeader(int quoteId, int versionId);
        List<CustomerAccount> GetAllCustomers();
        List<AccountContact> GetAccountTypeContacts(int accountId);
        QuoteListResponse GetQuoteList(SearchFilter searchfilter);
        List<AccountShipAddress> GetAccountTypeAddress(int accountId);
        List<Status> GetAllQuoteStatus();
        List<QuoteType> GetAllQuoteTypes();
        PartsResponse GetPartsList(QuotePartsFilter filter);
        PartDetails SetPartList(PartDetails setPartsListRequest);
        CommodityOptionsResponse GetCommodityOptins();
        PackagingOptionsResponse GetPackagingOptions();
        PackagingOptionsResponse GetConditionOptions(); 
        QuoteExtraResponse GetQuoteExtra(int quoteId, int quoteVersionId, int rowOffset,int rowLimit);
        SetQuoteExtraResponse SetQuoteExtra(SetQuoteExtraRequest setQuoteExtraRequest);
        QuotePartsDeleteResponse DeleteQuoteParts(List<int> quoteLineIds);
        QuoteToSOResponse QuoteToSalesOrder(QuoteToSORequest quoteToSoRequest);
        SetQuoteDetailsResponse SetQuoteExistingCustomer(SetQuoteExistingCustomerRequest setQuoteExistingCustomerRequest);
        SetQuoteDetailsResponse SetQuoteNewCustomer(SetQuoteNewCustomerRequest setQuoteNewCustomerRequest);
        int RouteQuoteLines(RouteQuoteLineRequest routeQuoteLineRequest);
        int SetQuoteLinePrint(QuotePrint quotePrint);
        QuoteLineHistoryResponse GetQuoteLineByAccountId(int accountId, int? contactId=null);
    }
}
