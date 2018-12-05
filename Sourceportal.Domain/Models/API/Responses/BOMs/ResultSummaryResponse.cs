using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class ResultSummaryResponse
    {
        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount { get; set; }

        [DataMember(Name = "resultSummaries")]
        public IList<ResultSummary> ResultSummaries { get; set; }
    }
    [DataContract]
    public class ResultSummary
    {
        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "salersOrders")]
        public int SalersOrders { get; set; }

        [DataMember(Name = "inventory")]
        public int Inventory { get; set; }

        [DataMember(Name = "purchaseOrders")]
        public int PurchaseOrders { get; set; }

        [DataMember(Name = "vendorQuotes")]
        public int VendorQuotes { get; set; }

        [DataMember(Name = "customerQuotes")]
        public int CustomerQuotes { get; set; }

        [DataMember(Name = "customerRfq")]
        public int CustomerRfq { get; set; }

        [DataMember(Name = "outsideOffers")]
        public int OutsideOffers { get; set; }

        [DataMember(Name = "bom")]
        public int Bom { get; set; }
    }

}
