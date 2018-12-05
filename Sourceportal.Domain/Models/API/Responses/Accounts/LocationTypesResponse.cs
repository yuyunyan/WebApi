using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Accounts
{
    [DataContract]
    public class LocationTypesResponse
    {
        [DataMember(Name = "locationTypes")]
        public List<LocationType> LocationTypes;
    }
}
