using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.OrderFulfillment
{
    [DataContract]
    public class ProductionLotSapRequest 
    {
        [DataMember(Name = "itemExternalId")]
        public string ItemExternalId { get; set; }

        [DataMember(Name = "newIdentifiedStockExternalId")]
        public string NewIdentifiedStockExternalId { get; set; }

        [DataMember(Name = "oldIdentifiedStockExternalId")]
        public string OldIdentifiedStockExternalId { get; set; }

        [DataMember(Name = "recvDate")]
        public DateTime RecvDate { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "coo")]
        public string COO { get; set; }

        [DataMember(Name = "rohs")]
        public string ROHS { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "warehouseBinExternalId")]
        public string WarehouseBinExternalId { get; set; }

        [DataMember(Name = "productSpec")]
        public string ProductSpec { get; set; }
    }
}
