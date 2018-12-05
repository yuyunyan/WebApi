using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class WarehouseBinResponse
    {
        [DataMember(Name = "warehouseBins")]
        public IList<WarehouseBin> WarehouseBins { get; set; }
    }

    [DataContract]
    public class WarehouseBin
    {
        [DataMember(Name = "warehouseBinId")]
        public int WarehouseBinID { get; set; }

        [DataMember(Name = "warehouseId")]
        public int WarehouseID { get; set; }

        [DataMember(Name = "binName")]
        public string BinName { get; set; }

        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }
    }
}
