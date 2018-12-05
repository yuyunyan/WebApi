using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class WarehouseResponse
    {
        [DataMember(Name = "warehouses")]
        public IList<Warehouse> Warehouses { get; set; }
    }

    [DataContract]
    public class Warehouse
    {
        [DataMember(Name = "warehouseId")]
        public int WarehouseID { get; set; }
        
        [DataMember(Name = "warehouseName")]
        public string WarehouseName { get; set; }

        [DataMember(Name = "locationId")]
        public int LocationID { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
    }
}
