namespace Sourceportal.Domain.Models.Middleware
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CreatedBy
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        public int Id { get; set; }
    }
}
