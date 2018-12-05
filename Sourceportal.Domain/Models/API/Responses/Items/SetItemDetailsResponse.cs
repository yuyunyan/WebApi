using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class SetItemDetailsResponse
    {
        [DataMember(Name = "itemId")]
        public int ItemID { get; set; }
    }
}
