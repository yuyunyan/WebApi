using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sourceportal.Domain.Models.API.Requests.ItemStock
{
    [DataContract]
    public class CreateStockRequestForSap : MiddlewareSyncBase
    {
        public CreateStockRequestForSap(int id, string externalId) : base(id, externalId)
        {
        }

        [DataMember(Name = "originalItemStockExternalID")]
        public string OriginalItemStockExternalID { get; set; }

        [DataMember(Name = "originalWarehouseExternalID")]
        public string originalWarehouseExternalID { get; set; }

        [DataMember(Name = "qcBinExternalUUID")]
        public string qcBinExternalUUID { get; set; }

        [DataMember(Name = "originalItemStock")]
        public StockDetailsRequest OriginalItemStock { get; set; }

        [DataMember(Name = "newItemStocks")]
        public List<StockDetailsRequest> NewItemStocks { get; set; }

    }

    [DataContract]
    public class StockDetailsRequest
    {
        [DataMember(Name = "itemStockExternalID")]
        public string ItemStockExternalID { get; set; }

        [DataMember(Name = "warehouseBinExternalID")]
        public string BinExternalID { get; set; }

        [DataMember(Name = "qty")]
        public int QtyToAllocate { get; set; }

        [DataMember(Name = "itemExternalId")]
        public string MaterialId { get; set; }

        [DataMember(Name = "expiry")]
        public DateTime? ExpirationDataTime { get; set; }

        [DataMember(Name = "coo")]
        public string CountryOfOrigin { get; set; }

        [DataMember(Name = "dateCode")]
        public string ManuDateCode { get; set; }

        [DataMember(Name = "stockDescription")]
        public string Description { get; set; }

        [DataMember(Name = "euRohs")]
        public string ROHS { get; set; }

        [DataMember(Name = "id")]
        public int LocalId { get; set; }

        [DataMember(Name = "productSpecId")]
        public string ProductSpecId { get; set; }
    }

    [DataContract]
    public class CreateStockResponse : BaseResponse
    {
        [DataMember(Name = "originalItemStockExternalId")]
        public string OriginalItemStockExternalId { get; set; }


        [DataMember(Name = "newItemStocks")]
        public List<NewItemStocksResponse> NewItemStocks { get; set; }
    }

    [DataContract]
    public class NewItemStocksResponse
    {
        [DataMember(Name = "itemStockExternalId")]
        public string ItemStockExternalId { get; set; }

        [DataMember(Name = "qty")]
        public int Qty { get; set; }

        [DataMember(Name = "localId")]
        public int LocalId { get; set; }
    }
}
