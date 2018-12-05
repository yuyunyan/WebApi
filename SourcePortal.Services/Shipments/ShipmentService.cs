using Sourceportal.DB.SalesOrders;
using Sourceportal.DB.Shipments;
using Sourceportal.Domain.Models.API.Requests.Shipments;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.Shipments
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepo;
        private readonly ISalesOrderRepository _salesRepo;

        public ShipmentService(IShipmentRepository shipmentRepo, ISalesOrderRepository salesRepo)
        {
            _shipmentRepo = shipmentRepo;
            _salesRepo = salesRepo;
        }

        public BaseResponse HandleOutboundDelivery(OutboundDeliverySapRequest request)
        {
            int existingShipId = _shipmentRepo.GetShipmentIdFromExternal(request.ExternalId);
   
            ShipmentDb shipment = new ShipmentDb();
            shipment.ExternalID = request.ExternalId;
            shipment.ExternalUUID = request.UUID;
            shipment.IsDeleted = false;
            shipment.TrackingNumber = request.TrackingNumber;
            shipment.ShipDate = DateTime.Parse(request.Date);

            if(existingShipId > 0)
            {
                shipment.ShipmentID = existingShipId;
            }

            int shipmentId = _shipmentRepo.ShipmentSet(shipment);

            if (shipmentId > 0)
            {

                foreach (var line in request.Lines)
                {
                    int soLineId = _salesRepo.GetSoLineIdFromExternal(line.SalesOrderExternalId, line.LineNumber);

                    if (soLineId > 0)
                    {
                        var soLineShipmentDb = new MapSOLineShipmentsDB();
                        soLineShipmentDb.IsDeleted = false;
                        soLineShipmentDb.Qty = (int)line.Qty;
                        soLineShipmentDb.ShipmentID = shipmentId;
                        soLineShipmentDb.SOLineID = soLineId;
                        var mapping = _shipmentRepo.MapSOLineShipments(soLineShipmentDb);
                        if(mapping == null)
                        {
                            return new BaseResponse { IsSuccess = false,
                                ErrorMessage = string.Format("Failure to create mapping for ShipmentID {0}, and SOLineID {1}", shipmentId, soLineId) };
                        }
                    }
                    else
                    {
                        return new BaseResponse { IsSuccess = false,
                            ErrorMessage = string.Format("Could not find SOLineID corresponding with Sales Order ExternalId {0}, and Line Number {1}", line.SalesOrderExternalId, line.LineNumber) };
                    }
                }

            }
            else
            {
                return new BaseResponse { IsSuccess = false, ErrorMessage = "Failure creating new Shipment in Shipments table" };
            }

            return new BaseResponse { IsSuccess = true };
        }
    }
}
