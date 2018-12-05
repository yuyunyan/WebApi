using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests.VendorRfqs;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.RFQ;

namespace SourcePortal.Services.VendorRFQs
{
    public interface IVendorRfqService
    {
        RfqDetailsResponse GetRfqBasicDetails(int rfqId);
        RfqLinesResponse GetRfqLines(VendorRfqLinesGetRequest vendorRfqLinesGetRequest);
        int SaveBasicDetails(VendorRfqSaveRequest vendorRfqSaveRequest);
        RfqListResponse GetAllRfqs(VendorRfqLineResponsesGetRequest request);
        int CreateNewRfqAndLines(VendorRfqCreateNewRequest request);
        RfqLines SaveRfqLine(RfqLineSaveRequest rfqLineSaveRequest);
        BaseResponse DeleteRfqLines(List<int> toList);
        RfqLineResponsesResponse GetRfqLineResponses(VendorRfqLineResponsesGetRequest vendorRfqLinesGetRequest);
        RfqLineResponse SaveRfqLineResponse(RfqLineResponseSaveRequest rfqLineResponseSaveRequest);
        BaseResponse DeleteRfqLineResponses(List<int> sourceIds, int rfqLineId);
        RfqDetailsResponse RfqFromFlaggedSet(SetRfqItemsFlaggedRequest setRfqItemsFlaggedRequest);
        bool SendRfqAndLines(RfqSendToSupplierRequest rfqSendRequest);
    }
}
