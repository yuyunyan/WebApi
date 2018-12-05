using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.QC
{
    public class ChecklistAssociationsDb
    {
        public string LinkType { get; set; }
        public string Value { get; set; }
        public int ObjectTypeID { get; set; }
        public int ObjectID { get; set; }
    }
}
