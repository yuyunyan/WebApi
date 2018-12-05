using System.Collections.Generic;
using System.Runtime.Serialization;
using Nest;

namespace Sourceportal.Domain.Models.DB.Items
{
    [DataContract]
    public class ItemIhs
    {
        [DataMember(Name = "abstractProduct")]
        public AbstractProduct AbstractProduct { get; set; }

    }

    [DataContract]
    public class Parts
    {
        [DataMember(Name = "abstractProduct")]
        public AbstractProduct AbstractProduct { get; set; }

    }

    [DataContract]
    public class AbstractProduct
    {
        [DataMember(Name = "mpn")]
        public string Part { get; set; }

        [DataMember(Name = "ihsId")]
        public string Id { get; set; }

        [DataMember(Name = "manufacturer")]
        public Manufacturer Mfr { get; set; }

        [DataMember(Name = "categories")]
        public Categories Categories { get; set; }

        [DataMember(Name = "technicalData")]
        public List<TechnicalData> TechnicalData { get; set; }


    }

    [DataContract]
    public class TechnicalData
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }

    [DataContract]
    public class Manufacturer
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
    
    [DataContract]
    public class Categories
    {
        [DataMember(Name = "en_US")]
        public IEnumerable<string> Names { get; set; }
    }
}
