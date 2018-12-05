using System;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses
{   
    [DataContract]    
    public class UserDetailsResponse
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public int OrganizationID { get; set; }
        [DataMember]
        public string TimezoneName { get; set; }
        [DataMember]
        public int CreatorID { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public DateTime dateCreated { get; set; }
        [DataMember]
        public DateTime dateLastLogin { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string organizationName { get; set; }


    }
}