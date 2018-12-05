using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Quotes
{
    public class SetQuoteDetailsRequest
    {
        public int QuoteId { get; set; }
        public int VersionId { get; set; }
        public int AccountId { get; set; }
        public int ItemListId { get; set; }
        public int ShipLocationId { get; set; }
        public int StatusId { get; set; }
        public int OrganizationId { get; set; }
        public int ValidForDays { get; set; }
        public int ContactId { get; set; }
        public int IncotermId { get; set; }
        public int PaymentTermId { get; set; }
        public string CurrencyId { get; set; }
        public int ShippingMethodId { get; set; }
        public int QuoteTypeId { get; set; }

        public string IncotermLocation { get; set; }

    }
}
