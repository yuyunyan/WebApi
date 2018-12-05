using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Quotes
{
    public class QuoteLineHistoryDb
    {
        public int QuoteLineId { get; set; }
        public int QuoteId { get; set; }
        public int LineNum { get; set; }
        public int VersionId { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactFullName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string PartNumber { get; set; }
        public int ItemID { get; set; }
        public string Manufacturer { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal GPM { get; set; }
        public string DateCode { get; set; }
        public string PackagingName { get; set; }
        public DateTime? SentDate { get; set; }
        public string Owners { get; set; }
    }
}
