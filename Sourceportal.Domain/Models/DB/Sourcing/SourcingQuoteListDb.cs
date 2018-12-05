using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.DB.Sourcing
{
    public class SourcingQuoteListDb
    {
        public int QuoteLineID { get; set; }

        public int QuoteID { get; set; }

        public int QuoteVersionID { get; set; }

        public int AccountID { get; set; }

        public string AccountName { get; set; }

        public int LineNum { get; set; }

        public string PartNumber { get; set; }

        public string PartNumberStrip { get; set; }

        public string Manufacturer { get; set; }

        public int CommodityID { get; set; }

        public string CommodityName { get; set; }

        public int StatusID { get; set; }

        public string StatusName { get; set; }

        public int Qty { get; set; }

        public int PackagingID { get; set; }

        public string PackagingName { get; set; }

        public string DateCode { get; set; }

        public int SourcesCount { get; set; }

        public int RFQCount { get; set; }

        public int TotalRows { get; set; }

        public int Comments { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int ItemID { get; set; }
        public int ItemListLineID { get; set; }
        public string DueDate { get; set; }
        public string ShipDate { get; set; }
        public int CustomerLine { get; set; }
        public string CustomerPartNumber { get; set; }
        public string TypeName { get; set; }
        public string Owners { get; set; }
    }
}
