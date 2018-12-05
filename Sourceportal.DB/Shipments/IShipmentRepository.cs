using Sourceportal.Domain.Models.DB.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Shipments
{
    public interface IShipmentRepository
    {
        int ShipmentSet(ShipmentDb shipment);
        MapSOLineShipmentsDB MapSOLineShipments(MapSOLineShipmentsDB mapSoLineShipments);
        int GetShipmentIdFromExternal(string externalId);
    }
}
