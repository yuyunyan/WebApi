using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
   public class PartSearchAvailabilityResponse
    {
        [DataMember( Name ="availabiltyList")]
        public IList<AvailabilityList> AvailabiltyList { get; set; }

    }

    [DataContract]
    public class AvailabilityList
    {
        [DataMember(Name = "itemID")]
        public int ItemID { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "supplier")]
        public string Supplier { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "allocated")]
        public List<AllocationsResponse> Allocated { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "packaging")]
        public string  Packaging { get; set; }

        [DataMember(Name = "packagingCondition")]
        public string  PackagingCondition { get; set; }

        [DataMember(Name = "buyer")]
        public string Buyer { get; set; }
    }

    [DataContract]
    public class AllocationsResponse
    {
        [DataMember(Name = "sOLineID")]
        public int SOLineID { get; set; }

        [DataMember(Name = "salesOrderID")]
        public int SalesOrderID { get; set; }

        [DataMember(Name = "sOVersionID")]
        public int SOVersionID { get; set; }

        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }

        [DataMember(Name = "accountID")]
        public int AccountID { get; set; }

        [DataMember(Name = "accountName")]
        public string AccountName { get; set; }

        [DataMember(Name = "orderQty")]
        public int OrderQty { get; set; }

        [DataMember(Name = "resvQty")]
        public int ResvQty { get; set; }

        [DataMember(Name = "externalID")]
        public string ExternalID { get; set; }

    }

    public class AllocationsJsonMapper
    {
        public int SOLineID { get; set; }
        public int SalesOrderID { get; set; }
        public int SOVersionID { get; set; }
        public int LineNum { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int OrderQty { get; set; }
        public int ResvQty { get; set; }
        public string ExternalID { get; set; }

    }

}
