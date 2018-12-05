using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.Quotes
{
   public class SetPartsListRequest
    {
      public int QuoteLineID { get; set; }
      public int QuoteID { get; set; }
      public int QuoteVersionID { get; set; }
      public int ItemListLineID { get; set; }
      public int StatusID { get; set; }
      public int ItemID { get; set; }
      public int AltFor { get; set; }
      public int CommodityID { get; set; }
      public int CustomerLine { get; set; }
      public string CustomerPartNum { get; set; }
      public string PartNumber { get; set; }
      public int    Qty { get; set; }
      public string Manufacturer { get; set; }
      public decimal TargetPrice { get; set; }
      public decimal Price { get; set; }
      public decimal Cost { get; set; }
      public string TargetDateCode { get; set; }
      public int PackagingID { get; set; }
      public DateTime ShipDate { get; set; }
      public string DateCode { get; set; }
      public bool IsRoutedToBuyers { get; set; }
      

    }
}
