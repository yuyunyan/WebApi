using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Quotes
{
   public class SetQuoteExtraRequest
    {
        public int QuoteExtraId { get; set; }
        public   int QuoteId { get; set; }
        public int QuoteVersionId { get; set; }
        public int StatusId { get; set; }
        public int ItemExtraId { get; set; }
        public  int LineNum { get; set; }
        public  int RefLineNum { get; set; }
        public int Qty { get; set; }
        public decimal   Price { get; set; }
        public decimal Cost { get; set; }
        public   int PrintOnQuote { get; set; }
        public  string Note { get; set; }
        public bool IsDeleted { get; set; }
    }
}
