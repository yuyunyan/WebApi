using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.QC
{
    public class CheckListLinkTypeDb
    {
        public string ObjectName { get; set; }

        public int ObjectTypeID { get; set; }

        public int AccountTypeID { get; set; }
    }
}
