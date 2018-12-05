using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.DB.Accounts;

namespace Sourceportal.Domain.Models.DB.Quotes
{
    public class QuoteDetailsDb
    {
        public int QuoteId { get; set; }
        public int VersionId { get; set; }

        public int AccountId { get; set; }

        public int ContactId { get; set; }

        public string OfficePhone { get; set; }

        public string Email { get; set; }

        public int ShipLocationId { get; set; }

        public double ValidForHours { get; set; }

        public int StatusId { get; set; }

        public int OrganizationID { get; set; }

        public int IncotermID { get; set; }

        public int PaymentTermID { get; set; }

        public string CurrencyID { get; set; }

        public int ShippingMethodID { get; set; }

        public int QuoteTypeID { get; set; }

        public List<OwnerDb> OwnerList { get; set; }

        public string IncotermLocation { get; set; }

    }
}
