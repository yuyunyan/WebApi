using Sourceportal.Domain.Models.API.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.QC
{
    [DataContract]
    public class InspectionConclusionResponse : BaseResponse
    {

        [DataMember(Name = "inventoryId")]
        public int InventoryID { get; set; }

        [DataMember(Name = "lotTotal")]
        public int LotTotal { get; set; }

        [DataMember(Name = "qtyPassed")]
        public int QtyPassed { get; set; }

        [DataMember(Name = "qtyFailedTotal")]
        public int QtyFailedTotal { get; set; }

        [DataMember(Name = "introduction")]
        public string Introduction { get; set; }

        [DataMember(Name = "testResults")]
        public string TestResults { get; set; }

        [DataMember(Name = "conclusion")]
        public string Conclusion { get; set; }

        [DataMember(Name = "inspectionQty")]
        public int InspectionQty { get; set; }
    }
}
