using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Navigation
{
    public class DbUserField
    {
        public int? FieldID { get; set; }
        public string Name { get; set; }
        public int CanEdit { get; set; }
    }
}
