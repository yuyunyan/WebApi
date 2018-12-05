using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.QC
{
    public class InpsectionConclusionRequest
    {
        public int InspectionID { get; set; }
        public int ResultID { get; set; }
        public string Conclusion { get; set; }
        public List<StockDetails> StockDetailsList { get; set; }
    }

    public class StockDetails
    {
        public int id { get; set; }
        public int warehouseBinId { get; set; }
    }
}