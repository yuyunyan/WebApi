using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.UploadXls
{
    [DataContract]
    public class XlsDataMapsGetResponse
    {
        [DataMember(Name = "xlsDataMaps")]
        public List<XlsDataMapGetObject> XlsDataMaps;
    }

    [DataContract]
    public class XlsDataMapGetObject
    {
        [DataMember(Name = "xlsDataMapId")]
        public int XlsDataMapID { get; set; }
        [DataMember(Name = "fieldLabel")]
        public string FieldLabel { get; set; }
        [DataMember(Name = "isRequired")]
        public byte IsRequired { get; set; }
    }
}
