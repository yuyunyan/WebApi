using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class AccountByTypeDb
    {
        public string AccountName { get; set; }
        public int AccountId { get; set; }
        public int AccountTypeID { get; set; }
    }
}
