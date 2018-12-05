using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class ContactFocusesDb
    {
        public int FocusID { get; set; }
        public string FocusName { get; set; }
        public string ObjectName { get; set; }
        public bool IsOption { get; set; }
    }
}
