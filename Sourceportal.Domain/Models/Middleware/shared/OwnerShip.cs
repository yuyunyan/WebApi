namespace Sourceportal.Domain.Models.Middleware
{
    using System.Runtime.Serialization;
    [DataContract]
    public class OwnerShip
    {
        [DataMember(Name = "leadOwner")]
        public Owner LeadOwner { get; set; }

        [DataMember(Name = "secondOwner")]
        public Owner SecondOwner { get; set; }
    }
}
