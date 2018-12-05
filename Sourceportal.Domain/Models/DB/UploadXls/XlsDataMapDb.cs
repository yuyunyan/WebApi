using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.UploadXls
{
    public class XlsDataMapDb
    {
        public int XlsDataMapID { get; set; }
        public string FieldLabel { get; set; }
        public byte IsRequired { get; set; }
    }
}
