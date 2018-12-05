using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Quotes
{
    public class QuoteHeaderDb
    {
        public decimal QuotePrice;
        public decimal QuoteCost;
        public decimal QuoteProfit;
        public double QuoteGPM;
        public int AccountID;
        public string Salesperson;
        public string SalespersonEmail;
        public string SentDate;
        public int UserID;
        public string Created;
    }
}
