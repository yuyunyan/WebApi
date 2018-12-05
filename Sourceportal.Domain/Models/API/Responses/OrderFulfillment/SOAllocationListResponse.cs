using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.OrderFulfillment
{
    [DataContract]
    public class SOAllocationListResponse
    {
        [DataMember(Name = "soAllocations")]
        public List<SOAllocationResponse> SOAllocations{ get; set; }
    }

    [DataContract]
    public class SOAllocationResponse
    {
        [DataMember(Name = "soLineId")]
        public int SOLineID { get; set; }
        [DataMember(Name = "soId")]
        public int SalesOrderID { get; set; }
        [DataMember(Name = "soVersionId")]
        public int SOVersionID { get; set; }
        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }
        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }
        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }
        [DataMember(Name = "mfrName")]
        public string MfrName { get; set; }
        [DataMember(Name = "soQty")]
        public int Qty { get; set; }
        [DataMember(Name = "neededQty")]
        public int Needed { get; set; }
        [DataMember(Name = "allocatedQty")]
        public int Allocated { get; set; }
        [DataMember(Name = "price")]
        public decimal Price { get; set; }
        [DataMember(Name = "shipDate")]
        public DateTime ShipDate { get; set; }
        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }
        [DataMember(Name = "sellers")]
        public string Sellers { get; set; }
        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }
        [DataMember(Name = "externalId")]
        public string ExternalID { get; set; }

    }
}
