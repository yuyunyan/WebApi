using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class AccountGroupDetailDb
    {
        public int GroupLineID { get; set; }
        public int AccountGroupID { get; set; }
        public int AccountID { get; set; }
        public int ContactID { get; set; }
        public string GroupName { get; set; }
        public int UserID { get; set; }
        public string AccountName { get; set; }
        public int AccountStatusId { get; set; }
        public string ContactName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OfficePhone { get; set; }
        public string Email { get; set; }
        public string AccountStatus { get; set; }
        public string AccountTypeIds { get; set; }
    }
}
