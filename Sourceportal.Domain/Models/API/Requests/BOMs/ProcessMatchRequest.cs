using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.BOMs
{
   public class ProcessMatchRequest
    {
        public int ItemListId { get; set; }
        public List<string> PartNumbers { get; set; }
        public string Manufacturer { get; set; }
        public string Account { get; set; }
        public string SearchType { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool MatchQuote { get; set; }
        public bool MatchSo { get; set; }
        public bool MatchPo { get; set; }
        public bool MatchOffers { get; set; }
        public bool MatchInventory { get; set; }
        public bool MatchRfq { get; set; }
        public bool MatchBom { get; set; }
        public bool MatchCustomerQuote { get; set; }
    
    }
}
