using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Quotes
{
   public class QuoteExtraDb
    {
        public int QuoteExtraId { get; set; }
        public int LineNum { get; set; }
        public int RefLineNum { get; set; }
        public int ItemExtraId { get; set; }
        public string ExtraName { get; set; }
        public string ExtraDescription { get; set; }
        public string Note { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal Gpm { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int CommentCount { get; set; }
        public int PrintOnQuote { get; set; }
        public int TotalRows { get; set; }
        public int Comments { get; set; }
    }
}
