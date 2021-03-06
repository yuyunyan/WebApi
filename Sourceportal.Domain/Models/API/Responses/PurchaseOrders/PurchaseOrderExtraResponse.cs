﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Responses.PurchaseOrders
{
    public class PurchaseOrderExtraResponse
    {
        [DataMember(Name = "extraListResponse")]
        public List<ExtraListResponse> ExtraListResponse;
    }

    public class ExtraListResponse
    {
        [DataMember(Name = "poExtraId")]
        public int POExtraId { get; set; }

        [DataMember(Name = "lineNum")]
        public int LineNum { get; set; }

        [DataMember(Name = "refLineNum")]
        public int RefLineNum { get; set; }

        [DataMember(Name = "itemExtraId")]
        public int ItemExtraId { get; set; }

        [DataMember(Name = "extraName")]
        public string ExtraName { get; set; }

        [DataMember(Name = "extraDescription")]
        public string ExtraDescription { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "cost")]
        public decimal Cost { get; set; }

        [DataMember(Name = "statusId")]
        public int StatusId { get; set; }

        [DataMember(Name = "statusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "comments")]
        public int Comments { get; set; }

        [DataMember(Name = "printOnPO")]
        public int PrintOnPO { get; set; }

        [DataMember(Name = "totalRows")]
        public int TotalRows { get; set; }
    }
}
