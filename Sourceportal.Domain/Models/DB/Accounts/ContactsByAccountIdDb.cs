using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
   public class ContactsByAccountIdDb
    {
        public int ContactId { get; set; }
        public string ExternalID { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OfficePhone { get; set; }
        public string Email { get; set; }
        public int IsActive { get; set; }
        public int? JobFunctionID { get; set; }
    }
}
