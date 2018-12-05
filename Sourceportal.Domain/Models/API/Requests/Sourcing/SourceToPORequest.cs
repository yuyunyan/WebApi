using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.Quotes;

namespace Sourceportal.Domain.Models.API.Requests.Sourcing
{
    public class SourceToPORequest
    {
        public int AccountID { get; set; }
        public int ContactID { get; set; }
        public int ShipTo { get; set; }
        public int ShipFrom { get; set; }
        public int PaymentTermID { get; set; }
        public int IncotermID { get; set; }
        public int BuyerID { get; set; }
        public PartDetails[] LinesToCopy { get; set; }
    }
}
