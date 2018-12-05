using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class AccountTypesDataDb
    {
        public int AccountTypeID { get; set; }
        
        public int AccountStatusID { get; set; }
        
        public string StatusName { get; set; }
        
        public int PaymentTermID { get; set; }
        
        public string PaymentTermName { get; set; }

        public string EPDSID { get; set; }

    }
}
