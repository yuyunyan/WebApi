using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class ItemsResponse
    {
        [DataMember(Name = "items")]
        public List<Items> Items { get; set; }
    }

    [DataContract]
    public class Items
    {
        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }
        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }
    }
}
