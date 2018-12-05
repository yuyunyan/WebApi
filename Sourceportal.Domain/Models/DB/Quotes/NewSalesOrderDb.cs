using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Quotes
{
    public class NewSalesOrderDb
    {
        public int SalesOrderId { get; set; }
        public int VersionId { get; set; }
        public int LinesCopiedCount { get; set; }
        public int ExtrasCopiedCount { get; set; }
    }
}
