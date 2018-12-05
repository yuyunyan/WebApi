using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Items
{
    [DataContract]
    public class PartSearchResponse : BaseResponse
    {
        [DataMember(Name = "suggestions")]
        public List<Suggestion> Suggestions { get; set; }
    }

    [DataContract]
    public class Suggestion
    {
        [DataMember(Name="id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "mfr")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "com")]
        public string Commodity { get; set; }

        [DataMember(Name = "isIHS")]
        public bool IsIhs { get; set; }

        [DataMember(Name = "data")]
        public string Data { get; set; }

        [ScriptIgnore]
        public string SourceDataId { get; set; }

    }
}
