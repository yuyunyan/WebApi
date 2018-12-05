using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.Middleware.Items
{
    [DataContract]
    public class ItemSync : MiddlewareSyncBase
    {
        public ItemSync(int id, string externalId) : base(id, externalId)
        {
        }

        [DataMember(Name = "mfg")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "commodityExternalId")]
        public string CommodityExternalId { get; set; }

        [DataMember(Name = "sourceDataId")]
        public double SourceDataId { get; set; }

        [DataMember(Name = "partNumber")]
        public string PartNumber { get; set; }

        [DataMember(Name = "partNumberStrip")]
        public string PartNumberStrip { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "eurohs")]
        public bool Eurohs { get; set; }

        [DataMember(Name = "eccn")]
        public string Eccn { get; set; }

        [DataMember(Name = "hts")]
        public string Hts { get; set; }

        [DataMember(Name = "msl")]
        public string Msl { get; set; }

        [DataMember(Name = "datasheetUrl")]
        public string DatasheetUrl { get; set; }

    }
}
