using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Navigation
{
    public class DbNavigation
    {
        public int NavID { get; set; }
        public int ParentNavID { get; set; }
        public string Interface { get; set; }
        public string NavName { get; set; }
        public string Icon { get; set; }
        public int SortOrder { get; set; }
    }
}
