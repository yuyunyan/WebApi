using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemListResponse : BaseResponse
    {
        [DataMember(Name = "items")]
        public IList<ItemResponse> Items { get; set; }

        [DataMember(Name = "totalRowCount")]
        public int TotalRowCount;
    }

    [DataContract]
    public class ItemResponse
    {
        [DataMember(Name = "manufacturerName")]
        public string ManufacturerName { get; set; }

        [DataMember(Name = "manufacturerPartNumber")]
        public string ManufacturerPartNumber { get; set; }

        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }

        [DataMember(Name = "commodityId")]
        public int CommodityID { get; set; }

        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "statusId")]
        public int? StatusID { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "eccn")]
        public string Eccn;

        [DataMember(Name = "eurohs")]
        public bool Eurohs;

        [DataMember(Name = "cnrohs")]
        public bool Cnrohs;

        [DataMember(Name = "groupId")]
        public int GroupID;

        [DataMember(Name = "groupName")]
        public string GroupName;

        [DataMember(Name = "manufacturerId")]
        public int ManufacturerID;

        [DataMember(Name = "hts")]
        public string HTS;

        [DataMember(Name = "msl")]
        public string MSL;

        [DataMember(Name = "length")]
        public float Length;

        [DataMember(Name = "width")]
        public float Width;

        [DataMember(Name = "depth")]
        public float Depth;

        [DataMember(Name = "weight")]
        public float Weight;

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage;
    }
}
