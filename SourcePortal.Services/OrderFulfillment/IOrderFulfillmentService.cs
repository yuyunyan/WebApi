using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sourceportal.Domain.Models.API.Responses.OrderFulfillment;
using Sourceportal.Domain.Models.API.Requests.OrderFulfillment;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB.SalesOrders;
using Sourceportal.Domain.Models.Shared;
using Sourceportal.Domain.Models.DB.OrderFulfillment;

namespace SourcePortal.Services.OrderFulfillment
{
   public interface IOrderFulfillmentService
   {
        OrderFulfillmentListResponse GetOrderFulfillmentList(OrderFulfillmentListSearchFilter searchFilter);

        OrderFulfillmentAvailabilityListResponse GetOrderFulfillmentAvailability(int soLineId);

        SetOrderFulfillmentQtyResponse SetOrderFulfillmentQty(int soLineId, int id, string idType, int qty, bool isDeleted);

        SOAllocationListResponse GetUnallocatedSOLines(UnallocatedSOLinesGetRequest unallocatedSoLinesGetRequest);

        List<OFGridExportLine> MapSOLinesToExport(List<OrderFillmentResponse> soLines);

        OrderFulfillmentListResponse RequestToPurchaseListGet(RequestToPurchaseListRequest searchFilter);

        BaseResponse HandleInboundDelivery(InboundDeliverySapRequest request);

        BaseResponse HandleLogisticsExecution(LogisticsExecutionSapRequest request);
        BaseResponse HandleProductionLot(ProductionLotSapRequest request);

        SourceOfSupply GetSourceOfSupply(SalesOrderDetailsDb salesOrderDb, int soLineId, List<SoSWarehouseDetailsDB> sosWarehouseDetails);
        List<SoSWarehouseDetailsDB> GetWarehouseSoSDetails(int soLineId);
   }
}
