using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class AccountFocusDb
    {
        public int FocusId { get; set; }
        public int AccountId { get; set; }
        public int FocusTypeId { get; set; }
        public int FocusObjectTypeId { get; set; }
        public int ObjectTypeId { get; set; }
        public int ObjectId { get; set; }
        public int? CommodityId { get; set; }
        public int? MfrId { get; set; }
        public string ObjectName { get; set; }
        public string FocusName { get; set; }
        public string ObjectValue { get; set; }
        public string CommodityName { get; set; }
        public string MfrName { get; set; }

    }
}
