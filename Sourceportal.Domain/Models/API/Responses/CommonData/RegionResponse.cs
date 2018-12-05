using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sourceportal.Domain.Models.API.Responses.CommonData
{
    [DataContract]
    public class RegionResponse
    {
        [DataMember(Name = "regions")]
        public IList<Regions> Regions { get; set; }
    }

    [DataContract]
    public class Regions
    {
        [DataMember(Name = "shipfromRegionID")]
        public int ShipfromRegionID { get; set; }

        [DataMember(Name = "regionName")]
        public string RegionName { get; set; }

        [DataMember(Name = "organizationID")]
        public int OrganizationID { get; set; }

        [DataMember(Name = "countryID")]
        public int CountryID { get; set; }
    }
}
