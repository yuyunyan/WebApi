using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class StateListResponse
    {
        [DataMember(Name = "stateList")]
        public List<State> StateList;
    }
}
