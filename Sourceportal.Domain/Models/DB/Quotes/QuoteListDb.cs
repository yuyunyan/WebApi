using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Quotes
{
   public class QuoteListDb
    {
        public int QuoteId { get; set; }
        public int VersionId { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public int ContactId { get; set; }

        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int OrganizationId { get; set; }
        public DateTime? SentDate { get; set; }
        public string Owners { get; set; }
        public string CountryName { get; set; }
        public int TotalRows { get; set; }
    }
}
