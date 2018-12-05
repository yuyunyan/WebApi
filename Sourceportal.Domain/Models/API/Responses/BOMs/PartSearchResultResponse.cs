using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.BOMs
{
    [DataContract]
    public class PartSearchResultResponse
    {
        [DataMember(Name = "resultList")]
        public IList<BomSearchResult> ResultList { get; set; }
    }

    [DataContract]
    public class BomSearchResult
    {
        [DataMember(Name = "itemID")]
        public int ItemID { get; set; }

        [DataMember(Name = "partNumber")]
        public String PartNumber { get; set; }

        [DataMember(Name = "partNumberStrip")]
        public string PartNumberStrip { get; set; }

        [DataMember(Name = "mfrID")]
        public int MfrID { get; set; }

        [DataMember(Name = "mfrName")]
        public string MfrName { get; set; }

        [DataMember(Name = "commodityID")]
        public int CommodityID { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "onOrder")]
        public int OnOrder { get; set; }

        [DataMember(Name = "onHand")]
        public int OnHand { get; set; }

        [DataMember(Name = "available")]
        public int Available { get; set; }

        [DataMember(Name = "reserved")]
        public int Reserved { get; set; }

    }
}
