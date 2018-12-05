using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.RFQ
{
    [DataContract]
    public class RfqLinesResponse
    {
        [DataMember(Name = "rfqLines")]
        public IList<RfqLines> RfqLines { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount { get; set; }
    }

    [DataContract]
    public class RfqLines
    {
        [DataMember(Name = "vRfqLineId")]
        public int VRFQLineID { get; set; }

        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityID { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "targetCost")]
        public float TargetCost { get; set; }

        [DataMember(Name = "dateCode")]
        public string DateCode { get; set; }

        [DataMember(Name = "packagingId")]
        public int PackagingID { get; set; }

        [DataMember(Name = "packagingName")]
        public string PackagingName { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }

        [DataMember(Name = "sourcesTotalQty")]
        public int SourcesTotalQty { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusID { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "partNumberStrip")]
        public string PartNumberStrip { get; set; }

        [DataMember(Name = "hasNoStock")]
        public int HasNoStock { get; set; }

        [DataMember(Name = "supplierName")]
        public string SupplierName { get; set; }

        [DataMember(Name = "contactName")]
        public string ContactName { get; set; }

        [DataMember(Name = "sentDate")]
        public string SentDate { get; set; }

        [DataMember(Name = "age")]
        public int Age { get; set; }

        [DataMember(Name = "ownerName")]
        public string OwnerName { get; set; }
        [DataMember(Name = "vRfqId")]
        public int VRFQID { get; set; }
        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }
        [DataMember(Name = "contactId")]
        public int ContactID { get; set; }
    }
}
