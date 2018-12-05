using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
   public class AccountsDb
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNum { get; set; }
        public string AccountType { get; set; }
        public string AccountTypeIds { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public string Organization { get; set; }
        public string Owners { get; set; }
        public string AccountStatus { get; set; }
        public string TotalContactCount { get; set; }
        public int TotalRowCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OfficePhone { get; set; }
        public string ContactID { get; set; }

    }
}
