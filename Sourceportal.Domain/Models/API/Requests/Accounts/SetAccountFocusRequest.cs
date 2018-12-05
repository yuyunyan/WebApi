using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    public class SetAccountFocusRequest
    {
        [DataMember(Name = "focusId")]
        public int FocusID { get; set; }
        [DataMember(Name = "accountId")]
        public int AccountID { get; set; }
        [DataMember(Name = "focusTypeId")]
        public int FocusTypeID { get; set; }
        [DataMember(Name = "focusObjectTypeId")]
        public int FocusObjectTypeID { get; set; }
        [DataMember(Name = "objectId")]
        public int ObjectID { get; set; }

    }
      public class DeleteAccountFocusRequest
        {
            [DataMember(Name = "focusId")]
            public int FocusID { get; set; }
    }
   

}
