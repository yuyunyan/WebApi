using System;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class AccountFocusResponse
    {
        [DataMember(Name = "focusId")]
        public int FocusId { get; set; }
        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }
        [DataMember(Name = "focusTypeId")]
        public int FocusTypeId { get; set; }
        [DataMember(Name = "objectTypeId")]
        public int ObjectTypeId { get; set; }
        [DataMember(Name = "focusObjectTypeId")]
        public int FocusObjectTypeId { get; set; }
        [DataMember(Name = "objectId")]
        public int ObjectId { get; set; }
        [DataMember(Name = "commodityId")]
        public int? CommodityId { get; set; }
        [DataMember(Name = "mfrId")]
        public int? MfrId { get; set; }
        [DataMember(Name = "objectName")]
        public string ObjectName { get; set; }
        [DataMember(Name = "focusName")]
        public string FocusName { get; set; }
        [DataMember(Name = "objectValue")]
        public string ObjectValue { get; set; }
        [DataMember(Name = "commodityName")]
        public string CommodityName { get; set; }
        [DataMember(Name = "mfrName")]
        public string MfrName { get; set; }

    }
}
