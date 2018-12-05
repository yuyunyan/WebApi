using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests.VendorRfqs;
using Sourceportal.Domain.Models.DB.RFQ;

namespace Sourceportal.DB.VendorRFQs
{
    public interface IVendorRfqRepository
    {
        RfqDetailsDb GetRfqBasicDetails(int rfqId);
        IList<RfqLinesDb> GetRfqLines(VendorRfqLinesGetRequest vendorRfqLinesGetRequest);
        List<RfqDetailsDb> GetAllRfqs(VendorRfqLineResponsesGetRequest request);
        RfqDetailsDb SaveBasicDetails(VendorRfqSaveRequest vendorRfqSaveRequest);
        RfqLinesDb SaveRfqLine(RfqLineSaveRequest rfqLineSaveRequest);
        bool DeleteRfqLines(List<int> rfqLineIds);
        IList<RfqLineResponseDb> GetRfqLineResponses(VendorRfqLineResponsesGetRequest vendorRfqLinesGetRequest);
        RfqLineResponseDb SaveRfqLineResponse(RfqLineResponseSaveRequest rfqLineResponseSaveRequest);
        bool DeleteRfqLineResponses(List<int> sourceIds, int rfqLineId);
        List<RfqLinesDb> SetRfqLineList(int rfqId, List<RfqLineSaveRequest> rfqLineList);
    }
}
