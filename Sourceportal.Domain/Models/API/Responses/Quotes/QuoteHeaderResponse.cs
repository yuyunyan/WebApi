using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.Quotes
{
    public class QuoteHeaderResponse
    {
        [DataMember(Name = "quoteTotal")]
        public decimal QuotePrice;
        [DataMember(Name = "quoteCost")]
        public decimal QuoteCost;
        [DataMember(Name = "grossProfit")]
        public decimal QuoteProfit;
        [DataMember(Name = "marin")]
        public double QuoteGPM;
        [DataMember(Name = "accountId")]
        public double AccountID;
        [DataMember(Name = "salesperson")]
        public string Salesperson;
        [DataMember(Name = "salespersonEmail")]
        public string SalespersonEmail;
        [DataMember(Name = "sentDate")]
        public string SentDate;
        [DataMember(Name = "created")]
        public string Created;
        [DataMember(Name = "userId")]
        public int UserID;
    }
}
