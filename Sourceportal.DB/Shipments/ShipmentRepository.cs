using Dapper;
using Sourceportal.Domain.Models.DB.Shipments;
using Sourceportal.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.DB.Shipments
{
    public class ShipmentRepository : IShipmentRepository
    {

        private static readonly string ConnectionString = ConfigurationManager
            .ConnectionStrings["SourcePortalConnection"].ConnectionString;

        public ShipmentRepository()
        {

        }

        public int ShipmentSet(ShipmentDb shipment)
        {
            int shipmentId;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                if(shipment.ShipmentID > 0)
                    param.Add("@ShipmentId", shipment.ShipmentID);
                param.Add("@ExternalID", shipment.ExternalID);
                param.Add("@ExternalUUID", shipment.ExternalUUID);
                param.Add("@CarrierName", shipment.CarrierName);
                param.Add("@TrackingNumber", shipment.TrackingNumber);
                param.Add("@TrackingURL", shipment.TrackingURL);
                param.Add("@ShipDate", shipment.ShipDate);
                param.Add("@IsDeleted", shipment.IsDeleted);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query("uspShipmentSet", param, commandType: CommandType.StoredProcedure);
                shipmentId = param.Get<int>("@ret");

                con.Close();
            }

            return shipmentId;
        }

        public MapSOLineShipmentsDB MapSOLineShipments(MapSOLineShipmentsDB mapSoLineShipments)
        {
            MapSOLineShipmentsDB mapSOLineShipmentsDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SOLineID", mapSoLineShipments.SOLineID);
                param.Add("@ShipmentId", mapSoLineShipments.ShipmentID);
                param.Add("@Qty", mapSoLineShipments.Qty);
                param.Add("@IsDeleted", mapSoLineShipments.IsDeleted);
                param.Add("@UserID", UserHelper.GetUserId());

                mapSOLineShipmentsDb = con.Query<MapSOLineShipmentsDB>("uspSalesOrderLineShipmentsSet", param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                con.Close();
            }

            return mapSOLineShipmentsDb;
        }


        public int GetShipmentIdFromExternal(string externalId)
        {
            int shipmentId = 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", externalId);

                var result = con.Query<int>("SELECT ShipmentID FROM Shipments WHERE ExternalID = @ExternalID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    shipmentId = result.First();
                }

                con.Close();
            }

            return shipmentId;
        }
    }
}
