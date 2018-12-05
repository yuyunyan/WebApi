using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class ContactProjectsDb
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public int AccountID { get; set; }
        public int? ContactID { get; set; }
        public bool IsOption { get; set; }
    }
}
