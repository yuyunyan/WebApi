using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.Quotes;

namespace Sourceportal.Domain.Models.API.Requests.Quotes
{
    public class QuoteToSORequest
    {
        public int QuoteId { get; set; }
        public string CustomerPO { get; set; }
        public PartDetails[] LinesToCopy { get; set; }
        public ExtraListResponse[] ExtrasToCopy { get; set; }
    }
}
