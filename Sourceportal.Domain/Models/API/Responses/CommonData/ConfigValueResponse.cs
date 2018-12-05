using System.Runtime.Serialization;


namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class ConfigValueResponse
    {
        [DataMember(Name = "configValue")]
        public string ConfigValue { get; set; }
    }
}
