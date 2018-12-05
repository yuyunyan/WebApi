using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.UploadXls
{
    [DataContract]
    public class XlsAccountGetResponse
    {
        [DataMember(Name = "xlsAccounts")]
        public List<XlsAccountObject> XlsAccounts;
    }

    [DataContract]
    public class XlsAccountObject
    {
        [DataMember(Name = "xlsDataMapId")]
        public int XlsDataMapID { get; set; }

        [DataMember(Name = "columnIndex")]
        public int ColumnIndex { get; set; }
    }
}
