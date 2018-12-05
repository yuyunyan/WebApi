using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using OfficeOpenXml;
using Sourceportal.DB.BOMs;
using Sourceportal.DB.Comments;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.BOMs;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.API.Responses.BOMs;
using Sourceportal.Domain.Models.API.Responses.OrderFulfillment;
using Sourceportal.Domain.Models.DB.BOMs;
using Sourceportal.Domain.Models.Shared;
using SourcePortal.Services.Images;
using EMSLineBom = Sourceportal.Domain.Models.API.Responses.BOMs.EMSLineBom;
using PurchaseOrderLineBom = Sourceportal.Domain.Models.API.Responses.BOMs.PurchaseOrderLineBom;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Sourceportal.DB.OrderFillment;

namespace SourcePortal.Services.BOMs
{
    public class BOMsService : IBOMsService
    {
        public readonly IBOMsRepository _BOMsRepository;
        public readonly ICommentRepository _commentRepository;
        private readonly IFileService _fileService;

        public BOMsService(IBOMsRepository BOMsRepository, ICommentRepository CommentRepository, IFileService fileService)
        {
            _BOMsRepository = BOMsRepository;
            _commentRepository = CommentRepository;
            _fileService = fileService;
        }

        public UploadBOMResponse UploadListXlsFile()
        {
            var httpRequest = HttpContext.Current.Request;
            var requestForm = httpRequest.Form;
            var dataMapJSON = requestForm["xlsDataMap"];
            var bomBodyJSON = requestForm["bomBody"];
            var dataMap = JsonConvert.DeserializeObject<XLSDataMapIDList>(dataMapJSON).dataMap;
            var bomBody = JsonConvert.DeserializeObject<BOMBody>(bomBodyJSON);
            if (bomBody.SaveLayout)
            {
                var saveLayoutSucess = SaveXlsDataAccountMap(bomBody, dataMap);
            }

            var itemListBOMLines = new List<ItemListLineBOMRequest>();
            var itemListExcessLines = new List<ItemListLineExcessRequest>();
            var ms = new MemoryStream();
            using (var excel = new ExcelPackage(httpRequest.Files[0].InputStream))
            {
                UploadBOMResponse response = new UploadBOMResponse();
                try
                {
                    var ws = excel.Workbook.Worksheets.First();
                    ValidateNumericCells(dataMap, ws);

                    switch (bomBody.ListTypeID)
                    {
                        case (int)Sourceportal.DB.Enum.ListTypeID.BOM:
                            itemListBOMLines = ParseBOMSheetToListObject(dataMap, ws);
                            response.ItemListId = UploadListBOMBodySet(bomBody, itemListBOMLines);
                            break;
                        case (int)Sourceportal.DB.Enum.ListTypeID.Excess:
                            itemListExcessLines = ParseExcessSheetToListObject(dataMap, ws);
                            response.ItemListId = UploadListExcessBodySet(bomBody, itemListExcessLines);
                            break;

                    }
                    ms.Position = 0;
                    excel.SaveAs(ms);
                }
                catch (Exception)
                {
                    var hssfwb = new HSSFWorkbook(httpRequest.Files[0].InputStream);
                    var ws = hssfwb.GetSheetAt(0);
                    switch (bomBody.ListTypeID)
                    {
                        case (int)Sourceportal.DB.Enum.ListTypeID.BOM:
                            itemListBOMLines = ParseOldBOMSheetToList(dataMap, ws);
                            var bomListId = UploadListBOMBodySet(bomBody, itemListBOMLines);
                            _fileService.SaveFile(DocumentPaths.BomUpload, ObjectType.ItemList, bomListId);

                            if (bomBody.Comment != "")
                            {
                                var commentId = SaveBOMComment(bomBody, bomListId);
                            }
                            response.ItemListId = bomListId;
                            break;

                        case (int)Sourceportal.DB.Enum.ListTypeID.Excess:
                            itemListExcessLines = ParseOldExcessSheetToList(dataMap, ws);
                            var excessListId = UploadListExcessBodySet(bomBody, itemListExcessLines);
                            _fileService.SaveFile(DocumentPaths.BomUpload, ObjectType.ItemList, excessListId);

                            if (bomBody.Comment != "")
                            {
                                var commentId = SaveBOMComment(bomBody, excessListId);
                            }
                            response.ItemListId = excessListId;
                            break;

                    }

                }

                return response;
            }
        }

        public BOMListResponse GetBOMList(SearchFilter searchFilter)
        {
            var result = _BOMsRepository.GetBOMList(searchFilter);
            var list = new List<BOMList>();
            var rowCount = 0;
            foreach (var value in result)
            {
                list.Add(new BOMList
                {
                    ItemListId = value.ItemListID,
                    ListName = value.ListName,
                    AccountID = value.AccountID,
                    AccountName = value.AccountName,
                    ContactID = value.ContactID,
                    ContactName = value.ContactName,
                    ItemCount = value.ItemCount,
                    FileName = value.FileNameOriginal,
                    StatusID = value.StatusID,
                    StatusName = value.StatusName,
                    UserName = value.UserName,
                    LoadDate = value.Created,
                    OrganizationName = value.OrganizationName,
                    Comments = value.Comments
                });
            }
            if (result.Count() > 0)
            {
                rowCount = result[0].RowCount;
            }
            return new BOMListResponse { BomList = list, RowCount = rowCount };
        }

        public BomProcessMatchResponse ProcessMatch(ProcessMatchRequest processMatchRequest)
        {
            var result = _BOMsRepository.ProcessMatch(processMatchRequest);
            return new BomProcessMatchResponse { SearchId = result };
        }

        public SalesOrderResponse GetSalesOrder(BomSearchRequest bomSearchRequest)
        {
            var dbSalesOrders = _BOMsRepository.GetSalesOrder(bomSearchRequest);
            var list = new List<SalesOrderLine>();
            var totalCount = 0;
            foreach (var value in dbSalesOrders)
            {
                list.Add(new SalesOrderLine
                {
                    SalesOrderId = value.SalesOrderId,
                    Mfg = value.Mfg,
                    SoDate = value.SoDate.ToString("dd/MM/yyyy"),
                    Customer = CleanLists(value.Customer),
                    PartNumber = CleanLists(value.PartNumber),
                    QtySold = value.QtySold,
                    SoldPrice = value.SoldPrice,
                    DateCode = CleanLists(value.DateCode),
                    DueDate = value.DueDate.ToString("dd/MM/yyyy"),
                    ShippedQty = value.ShippedQty,
                    OrderStatus = value.OrderStatus,
                    UnitCost = value.UnitCost,
                    SalesPerson = CleanLists(value.SalesPerson),
                    GrossProfitTotal = value.GrossProfitTotal,
                    ItemId = value.ItemId,
                    BomQty = value.BomQty,
                    BomPrice = value.BomPrice,
                    PriceDelta = value.PriceDelta,
                    Potential = value.Potential,
                    BomIntPartNumber = value.BomIntPartNumber,
                    BomPartNumber = value.BomPartNumber,
                    BomMfg = value.BomMfg
                });
            }
            if (dbSalesOrders.Count() > 0)
            {
                totalCount = dbSalesOrders[0].TotalRows;
            }
            return new SalesOrderResponse { SalesOrderLine = list, TotalRowCount = totalCount };

        }

        public InventoryBomResponse GetInventory(BomSearchRequest bomSearchRequest)
        {
            var dbSalesOrders = _BOMsRepository.GetInventory(bomSearchRequest);
            var list = new List<InventoryLineBom>();
            var totalCount = 0;
            foreach (var value in dbSalesOrders)
            {
                list.Add(new InventoryLineBom
                {
                    POLineID = value.POLineID,
                    Mfg = value.Manufacturer,
                    MfgPartNumber = CleanLists(value.PartNumber),
                    Warehouse = value.WarehouseCode,
                    InvQty = value.InventoryQty,
                    DateCode = CleanLists(value.DateCode),
                    Cost = value.Cost,
                    ResQty = value.ReservedQty,
                    AvailQty = value.AvailableQty,
                    ItemId = value.ItemId,
                    BomQty = value.BomQty,
                    PriceDelta = value.PriceDelta,
                    Potential = value.Potential,
                    BomPrice = value.BomPrice,
                    BomPartNumber = value.BomPartNumber,
                    BomIntPartNumber = value.BomIntPartNumber,
                    BomMfg = value.BomMfg
                });
            }
            if (dbSalesOrders.Count() > 0)
            {
                totalCount = dbSalesOrders[0].TotalRows;
            }
            return new InventoryBomResponse { InvLines = list, TotalRows = totalCount };
        }

        public OutsideOffersBomResponse GetOutsideOffers(BomSearchRequest bomSearchRequest)
        {
            var dbSalesOrders = _BOMsRepository.GetOutsideOffers(bomSearchRequest);
            var list = new List<OutsideOffersLine>();
            var totalCount = 0;
            foreach (var value in dbSalesOrders)
            {
                list.Add(new OutsideOffersLine
                {
                    SourceID = value.SourceID,
                    OfferDate = value.Created.ToString("dd/MM/yyyy"),
                    Vendor = value.AccountName,
                    Mfg = value.Manufacturer,
                    MfgPartNumber = CleanLists(value.PartNumber),
                    Qty = value.Qty,
                    DateCode = CleanLists(value.DateCode),
                    Cost = value.Cost,
                    Buyer = value.Buyer,
                    LeadTimeDays = value.LeadTimeDays,
                    ItemId = value.ItemId,
                    BomQty = value.BomQty,
                    PriceDelta = value.PriceDelta,
                    Potential = value.Potential,
                    BomPrice = value.BomPrice,
                    BomPartNumber = value.BomPartNumber,
                    BomIntPartNumber = value.BomIntPartNumber,
                    BomMfg = value.BomMfg

                });
            }
            if (dbSalesOrders.Count() > 0)
            {
                totalCount = dbSalesOrders[0].TotalRows;
            }
            return new OutsideOffersBomResponse { OutsideOffersLines = list, TotalRows = totalCount };
        }

        public VendorQuotesBomResponse GetVendorQuotes(BomSearchRequest bomSearchRequest)
        {
            var dbSalesOrders = _BOMsRepository.GetVendorQuotes(bomSearchRequest);
            var list = new List<VendorQuoteLine>();
            var totalCount = 0;
            foreach (var value in dbSalesOrders)
            {
                list.Add(new VendorQuoteLine
                {
                    SourceID = value.SourceID,
                    OfferDate = value.Created.ToString("dd/MM/yyyy"),
                    Vendor = value.AccountName,
                    Mfg = value.Manufacturer,
                    MfgPartNumber = CleanLists(value.PartNumber),
                    Qty = value.Qty,
                    DateCode = CleanLists(value.DateCode),
                    Cost = value.Cost,
                    Buyer = value.Buyer,
                    LeadTimeDays = value.LeadTimeDays,
                    ItemId = value.ItemId,
                    Note = value.Note,
                    Moq = value.Moq,
                    Spq = value.Spq,
                    BomQty = value.BomQty,
                    PriceDelta = value.PriceDelta,
                    Potential = value.Potential,
                    BomPrice = value.BomPrice,
                    BomPartNumber = value.BomPartNumber,
                    BomIntPartNumber = value.BomIntPartNumber,
                    BomMfg = value.BomMfg
                });
            }
            if (dbSalesOrders.Count() > 0)
            {
                totalCount = dbSalesOrders[0].TotalRows;
            }
            return new VendorQuotesBomResponse { VendorQuoteLines = list, TotalRows = totalCount };
        }

        private static string CleanLists(string stringToClean)
        {
            return !string.IsNullOrEmpty(stringToClean) ? stringToClean.Trim() : null;
        }

        public void ValidateNumericCells(List<int> dataMap, ExcelWorksheet ws)
        {
            var numberRegex = new Regex(@"(?<=^| )\$?\d+(\,\d+)?(\.\d+)?(?=$| )|(?<=^| )\$?\.\d+(?=$| )", RegexOptions.ECMAScript);

            for (int i = 0; i < dataMap.Count; i++)
            {
                if (dataMap[i] == 0)
                {
                    continue;
                }
                if (dataMap[i] == (int)XlsDataMap.ItemListBOMQty || dataMap[i] == (int)XlsDataMap.ItemListExcessQty)
                {
                    for (int rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsCell = ws.Cells[rowNum, i + 1];
                        var cellValue = wsCell.GetValue<string>();
                        if (cellValue == null || !numberRegex.IsMatch(cellValue))
                        {
                            wsCell.Value = 0;
                        }
                        else if (cellValue[0] == '$')
                        {
                            wsCell.Value = Convert.ToInt32(cellValue.Replace("$", ""));
                        }
                    }
                }
            }
        }

        public List<ItemListLineBOMRequest> ParseBOMSheetToListObject(List<int> dataMap, ExcelWorksheet ws)
        {
            var itemListLines = new List<ItemListLineBOMRequest>();

            for (int rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var itemLine = new ItemListLineBOMRequest();
                for (int idx = 0; idx < dataMap.Count; idx++)
                {
                    if (dataMap[idx] == 0)
                    {
                        continue;
                    }
                    var wsCell = ws.Cells[rowNum, idx + 1];
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMQty)
                    {
                        itemLine.Qty = wsCell.GetValue<int>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMManufacturer)
                    {
                        itemLine.Manufacturer = wsCell.GetValue<string>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMPartNumber)
                    {
                        itemLine.PartNumber = wsCell.GetValue<string>();
                        var originalPartNumber = wsCell.GetValue<string>();
                        var stripRegex = new Regex(@"[^0-9a-zA-Z]");
                        itemLine.PartNumberStrip = stripRegex.Replace(originalPartNumber, "");
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMCustomerPartNum)
                    {
                        itemLine.CustomerPartNumber = wsCell.GetValue<string>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMTargetPrice)
                    {
                        itemLine.TargetPrice = wsCell.GetValue<float>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMTargetDateCode)
                    {
                        itemLine.TargetDateCode = wsCell.GetValue<string>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMAssocAccountID)
                    {
                        itemLine.AssocAccountID = wsCell.GetValue<string>();
                    }
                }
                itemListLines.Add(itemLine);
            }
            return itemListLines;
        }
        public List<ItemListLineExcessRequest> ParseExcessSheetToListObject(List<int> dataMap, ExcelWorksheet ws)
        {
            var itemListLines = new List<ItemListLineExcessRequest>();

            for (int rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var itemLine = new ItemListLineExcessRequest();
                for (int idx = 0; idx < dataMap.Count; idx++)
                {
                    if (dataMap[idx] == 0)
                    {
                        continue;
                    }
                    var wsCell = ws.Cells[rowNum, idx + 1];
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessQty)
                    {
                        itemLine.Qty = wsCell.GetValue<int>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessManufacturer)
                    {
                        itemLine.Manufacturer = wsCell.GetValue<string>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessPartNumber)
                    {
                        itemLine.PartNumber = wsCell.GetValue<string>();
                        var originalPartNumber = wsCell.GetValue<string>();
                        var stripRegex = new Regex(@"[^0-9a-zA-Z]");
                        itemLine.PartNumberStrip = stripRegex.Replace(originalPartNumber, "");
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessCustomerPartNum)
                    {
                        itemLine.CustomerPartNumber = wsCell.GetValue<string>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessCost)
                    {
                        itemLine.TargetPrice = wsCell.GetValue<float>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessDateCode)
                    {
                        itemLine.DateCode = wsCell.GetValue<string>();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessMOQ)
                    {
                        itemLine.MOQ = wsCell.GetValue<int>();
                        continue;
                    }

                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessSPQ)
                    {
                        itemLine.SPQ = wsCell.GetValue<int>();
                    }
                }
                itemListLines.Add(itemLine);
            }
            return itemListLines;
        }

        private List<ItemListLineBOMRequest> ParseOldBOMSheetToList(List<int> dataMap, ISheet ws)
        {
            var itemListLines = new List<ItemListLineBOMRequest>();
            for (int rowNum = 1; rowNum <= ws.LastRowNum; rowNum++)
            {
                var itemLine = new ItemListLineBOMRequest();
                var row = ws.GetRow(rowNum);
                for (int idx = 0; idx < dataMap.Count; idx++)
                {
                    if (dataMap[idx] == 0)
                    {
                        continue;
                    }
                    var wsCell = row.GetCell(idx);
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMQty)
                    {
                        if (wsCell != null)
                        {
                            itemLine.Qty = Convert.ToInt32(wsCell.ToString());
                        }
                        else
                            itemLine.Qty = null;
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMManufacturer)
                    {
                        itemLine.Manufacturer = wsCell.ToString();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMPartNumber)
                    {
                        itemLine.PartNumber = wsCell.ToString();
                        var originalPartNumber = wsCell.ToString();
                        var stripRegex = new Regex(@"[^0-9a-zA-Z]");
                        itemLine.PartNumberStrip = stripRegex.Replace(originalPartNumber, "");
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMCustomerPartNum)
                    {
                        if (wsCell != null)
                        {
                            itemLine.CustomerPartNumber = wsCell.ToString();
                        }
                        else
                            itemLine.CustomerPartNumber = null;
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMTargetPrice)
                    {
                        if (wsCell != null)
                        {
                            var numberRegex = new Regex(@"(?<=^| )\$?\d+(\,\d+)?(\.\d+)?(?=$| )|(?<=^| )\$?\.\d+(?=$| )", RegexOptions.ECMAScript);
                            var priceString = wsCell.ToString();
                            if (priceString == "" || !numberRegex.IsMatch(priceString))
                            {
                                priceString = "0";
                            }
                            if (priceString[0] == '$')
                            {

                            }

                            itemLine.TargetPrice = Convert.ToSingle(priceString.Replace("$", ""));
                        }
                        else
                            itemLine.TargetPrice = null;
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMTargetDateCode)
                    {
                        itemLine.TargetDateCode = wsCell.ToString();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListBOMAssocAccountID)
                    {
                        if (wsCell != null)
                            itemLine.AssocAccountID = wsCell.ToString();
                        else
                            itemLine.AssocAccountID = null;
                    }
                }
                itemListLines.Add(itemLine);
            }
            return itemListLines;
        }

        private List<ItemListLineExcessRequest> ParseOldExcessSheetToList(List<int> dataMap, ISheet ws)
        {
            var itemListLines = new List<ItemListLineExcessRequest>();
            for (int rowNum = 1; rowNum <= ws.LastRowNum; rowNum++)
            {
                var itemLine = new ItemListLineExcessRequest();
                var row = ws.GetRow(rowNum);
                for (int idx = 0; idx < dataMap.Count; idx++)
                {
                    if (dataMap[idx] == 0)
                    {
                        continue;
                    }
                    var wsCell = row.GetCell(idx);
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessQty)
                    {
                        if (wsCell == null)
                            itemLine.Qty = null;
                        else
                            itemLine.Qty = Convert.ToInt32(wsCell.StringCellValue);
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessManufacturer)
                    {
                        itemLine.Manufacturer = wsCell.ToString();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessPartNumber)
                    {
                        itemLine.PartNumber = wsCell.ToString();
                        var originalPartNumber = wsCell.ToString();
                        var stripRegex = new Regex(@"[^0-9a-zA-Z]");
                        itemLine.PartNumberStrip = stripRegex.Replace(originalPartNumber, "");
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessCustomerPartNum)
                    {
                        if (wsCell == null)
                            itemLine.CustomerPartNumber = null;
                        else
                            itemLine.CustomerPartNumber = wsCell.ToString();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessCost)
                    {
                        if (wsCell == null)
                            itemLine.TargetPrice = null;
                        else
                        {
                            var numberRegex = new Regex(@"(?<=^| )\$?\d+(\,\d+)?(\.\d+)?(?=$| )|(?<=^| )\$?\.\d+(?=$| )", RegexOptions.ECMAScript);
                            var priceString = wsCell.ToString();
                            if (priceString == "" || !numberRegex.IsMatch(priceString))
                            {
                                priceString = "0";
                            }
                            if (priceString[0] == '$')
                            {

                            }
                            itemLine.TargetPrice = Convert.ToSingle(priceString.Replace("$", ""));
                        }
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessDateCode)
                    {
                        itemLine.DateCode = wsCell.ToString();
                        continue;
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessMOQ)
                    {
                        if (wsCell == null)
                            itemLine.MOQ = null;
                        else
                            itemLine.MOQ = Convert.ToInt32(wsCell.ToString());
                    }
                    if (dataMap[idx] == (int)XlsDataMap.ItemListExcessSPQ)
                    {
                        if (wsCell == null)
                            itemLine.SPQ = null;
                        else
                            itemLine.SPQ = Convert.ToInt32(wsCell.ToString());
                    }
                }
                itemListLines.Add(itemLine);
            }
            return itemListLines;
        }

        private bool SaveXlsDataAccountMap(BOMBody bomBody, List<int> xlsDataMapIdList)
        {
            return _BOMsRepository.SaveXlsDataAccountMap(bomBody, xlsDataMapIdList);
        }

        private int UploadListBOMBodySet(BOMBody bomBody, List<ItemListLineBOMRequest> itemListLines)
        {
            if (bomBody.PublishToSources != true)
                bomBody.SourcingTypeID = 0;
            return _BOMsRepository.UploadListBOMBodySet(bomBody, itemListLines);
        }

        private int UploadListExcessBodySet(BOMBody bomBody, List<ItemListLineExcessRequest> itemListLines)
        {
            if (bomBody.PublishToSources != true)
                bomBody.SourcingTypeID = 0;
            return _BOMsRepository.UploadListExcessBodySet(bomBody, itemListLines);
        }

        private int SaveBOMComment(BOMBody bomBody, int itemListID)
        {
            var bomComment = new SetCommentRequest
            {
                Comment = bomBody.Comment,
                ObjectID = itemListID,
                ObjectTypeID = (int)ObjectType.ItemList,
                CommentTypeID = (int)CommentType.ItemList,
                IsDeleted = 0
            };

            var commentDb = _commentRepository.SetComment(bomComment);

            return commentDb.CommentID;
        }

        public PurchaseOrderBomResponse GetPurchaseOrders(BomSearchRequest bomSearchRequest)
        {
            var poLinesDbs = _BOMsRepository.GetPurchaseOrders(bomSearchRequest);
            var list = new List<PurchaseOrderLineBom>();

            foreach (var poLineDb in poLinesDbs)
            {
                list.Add(new PurchaseOrderLineBom
                {
                    PODate = poLineDb.PODate.ToString("dd/MM/yyyy"),
                    Vendor = poLineDb.Vendor != null ? poLineDb.Vendor.Trim() : null,
                    MfgPartNumber = poLineDb.MfgPartNumber != null ? poLineDb.MfgPartNumber.Trim() : null,
                    Mfg = poLineDb.Mfg,
                    POCost = poLineDb.POCost,
                    Buyer = poLineDb.Buyer,
                    ReceivedDate = poLineDb.ReceivedDate.ToString("dd/MM/yyyy"),
                    ReceivedQty = poLineDb.ReceivedQty,
                    OrderStatus = poLineDb.OrderStatus,
                    PurchaseOrderID = poLineDb.PurchaseOrderID,
                    POLineID = poLineDb.POLineID,
                    DateCode = poLineDb.DateCode != null ? poLineDb.DateCode.Trim() : null,
                    ItemId = poLineDb.ItemId,
                    BomQty = poLineDb.BomQty,
                    PriceDelta = poLineDb.PriceDelta,
                    Potential = poLineDb.Potential,
                    BomPrice = poLineDb.BomPrice,
                    BomPartNumber = poLineDb.BomPartNumber,
                    BomIntPartNumber = poLineDb.BomIntPartNumber,
                    BomMfg = poLineDb.BomMfg
                });
            }

            return new PurchaseOrderBomResponse { PoLines = list, TotalRows = poLinesDbs.Count > 0 ? poLinesDbs.First().TotalRows : 0 };
        }

        public CustomerQuoteBomResponse GetCustomerQuotes(BomSearchRequest bomSearchRequest)
        {
            var quoteLinesDbs = _BOMsRepository.GetCustomerQuotes(bomSearchRequest);
            var list = new List<RFQLineBom>();

            foreach (var rfqLineDb in quoteLinesDbs)
            {
                list.Add(new RFQLineBom
                {
                    QuoteID = rfqLineDb.QuoteID,
                    QuoteDate = rfqLineDb.QuoteDate.ToString("dd/MM/yyyy"),
                    Customer = rfqLineDb.Customer,
                    Contact = rfqLineDb.Contact,
                    ItemId = rfqLineDb.ItemId,
                    Owners = rfqLineDb.Owners,
                    DateCode = rfqLineDb.DateCode,
                    CustomerPartNum = rfqLineDb.CustomerPartNum,
                    BOMPartNumber = rfqLineDb.BOMPartNumber,
                    BOMIntPartNumber = rfqLineDb.BOMIntPartNumber,
                    BOMMfg = rfqLineDb.BOMMfg,
                    BOMPrice = rfqLineDb.BOMPrice,
                    BOMQty = rfqLineDb.BOMQty,
                    Qty = rfqLineDb.Qty,
                    Manufacturer = rfqLineDb.Manufacturer,
                    TargetPrice = rfqLineDb.TargetPrice,
                    PartNumber = rfqLineDb.PartNumber,
                    Potential = rfqLineDb.Potential,
                    PriceDelta = rfqLineDb.PriceDelta,
                });
            }

            return new CustomerQuoteBomResponse { QuoteLines = list, TotalRows = quoteLinesDbs.Count > 0 ? quoteLinesDbs.First().TotalRows : 0 };
        }

        public CustomerRFQBomResponse GetCustomerRfqs(BomSearchRequest bomSearchRequest)
        {
            var rfqLinesDbs = _BOMsRepository.GetCustomerRfqs(bomSearchRequest);
            var list = new List<RFQLineBom>();

            foreach (var rfqLineDb in rfqLinesDbs)
            {
                list.Add(new RFQLineBom
                {
                    QuoteID = rfqLineDb.QuoteID,
                    QuoteDate = rfqLineDb.QuoteDate.ToString("dd/MM/yyyy"),
                    Customer = rfqLineDb.Customer,
                    Contact = rfqLineDb.Contact,
                    ItemId = rfqLineDb.ItemId,
                    Owners = rfqLineDb.Owners,
                    DateCode = rfqLineDb.DateCode,
                    CustomerPartNum = rfqLineDb.CustomerPartNum,
                    BOMPartNumber = rfqLineDb.BOMPartNumber,
                    BOMIntPartNumber = rfqLineDb.BOMIntPartNumber,
                    BOMMfg = rfqLineDb.BOMMfg,
                    BOMPrice = rfqLineDb.BOMPrice,
                    BOMQty = rfqLineDb.BOMQty,
                    Qty = rfqLineDb.Qty,
                    Manufacturer = rfqLineDb.Manufacturer,
                    TargetPrice = rfqLineDb.TargetPrice,
                    PartNumber = rfqLineDb.PartNumber,
                    Potential = rfqLineDb.Potential,
                    PriceDelta = rfqLineDb.PriceDelta,
                });
            }

            return new CustomerRFQBomResponse { RFQLines = list, TotalRows = rfqLinesDbs.Count > 0 ? rfqLinesDbs.First().TotalRows : 0 };
        }

        public EMSBomResponse GetEMSs(BomSearchRequest bomSearchRequest)
        {
            var emsLinesDbs = _BOMsRepository.GetEMSs(bomSearchRequest);
            var list = new List<EMSLineBom>();

            foreach (var emsLineDb in emsLinesDbs)
            {
                list.Add(new EMSLineBom
                {
                    ItemListID = emsLineDb.ItemListID,
                    BOMDate = emsLineDb.BOMDate.ToString("dd/MM/yyyy"),
                    Customer = emsLineDb.Customer,
                    CustomerPartNum = emsLineDb.CustomerPartNum,
                    BOMPrice = emsLineDb.BOMPrice,
                    BOMQty = emsLineDb.BOMQty,
                    Qty = emsLineDb.Qty,
                    Manufacturer = emsLineDb.Manufacturer,
                    TargetPrice = emsLineDb.TargetPrice,
                    CreatedBy = emsLineDb.OwnerName,
                    PartNumber = emsLineDb.PartNumber,
                    Potential = emsLineDb.Potential,
                    PriceDelta = emsLineDb.PriceDelta,
                    ItemId = emsLineDb.ItemId
                });
            }

            return new EMSBomResponse { EMSLines = list, TotalRows = emsLinesDbs.Count > 0 ? emsLinesDbs.First().TotalRows : 0 };
        }

        public ResultSummaryResponse GetResultSummary(BomSearchRequest bomSearchRequest)
        {
            var resultSummaryDbs = _BOMsRepository.GetResultSummary(bomSearchRequest);
            var list = new List<ResultSummary>();
            foreach (var value in resultSummaryDbs)
            {
                list.Add(new ResultSummary
                {
                    ItemId = value.ItemId,
                    PartNumber = CleanLists(value.PartNumber),
                    Manufacturer = value.Manufacturer,
                    SalersOrders = value.SalersOrders,
                    Inventory = value.Inventory,
                    PurchaseOrders = value.PurchaseOrders,
                    VendorQuotes = value.VendorQuotes,
                    CustomerQuotes = value.CustomerQuotes,
                    CustomerRfq = value.CustomerRfq,
                    OutsideOffers = value.OutsideOffers,
                    Bom = value.Bom,
                });

            }
            return new ResultSummaryResponse() { ResultSummaries = list, TotalRowCount = resultSummaryDbs.Count > 0 ? resultSummaryDbs.First().RowCount : 0 };
        }

        public PartSearchResultResponse GetPartSearchResult(string searchString, string searchType)
        {
            var partSearchResultDbs = _BOMsRepository.GetPartSearchResult(searchString, searchType);
            List<BomSearchResult> response = new List<BomSearchResult>();

            foreach(var result in partSearchResultDbs)
            {
                response.Add(new BomSearchResult
                {
                    ItemID = result.ItemID,
                    PartNumber = result.PartNumber,
                    PartNumberStrip = result.PartNumberStrip,
                    MfrID = result.MfrID,
                    MfrName = result.MfrName,
                    CommodityID = result.CommodityID,
                    CommodityName = result.CommodityName,
                    OnHand = result.OnHand,
                    OnOrder = result.OnOrder,
                    Available = result.Available,
                    Reserved = result.Reserved
                });
            }
            return new PartSearchResultResponse() { ResultList = response };
        }

        public PartSearchAvailabilityResponse GetAvailabilityPart(int itemId)
        {
            var partAvailabilityDbs = _BOMsRepository.GetAvailabilityPart(itemId);
            List<AvailabilityList> response = new List<AvailabilityList>();
        
            foreach(var availability in partAvailabilityDbs)
            {
                response.Add(new AvailabilityList
                {
                    ItemID = availability.ItemID,
                    Location = availability.WarehouseName,
                    Type = availability.Type,
                    Supplier = availability.AccountName,
                    Allocated = MapAllocationJsonToObject(availability.Allocations),
                    Cost = availability.Cost,
                    DateCode = availability.DateCode,
                    Qty = availability.OrigQty,
                    Packaging = availability.PackagingName,
                    Buyer = availability.Buyers,
                    PackagingCondition = availability.ConditionName
                   
                });
            }
            return new PartSearchAvailabilityResponse { AvailabiltyList = response };
        }

        public List<AllocationsResponse> MapAllocationJsonToObject(string allocaionJson)
        {
            var responseList = new List<AllocationsResponse>();
            if(allocaionJson == null)
            {
                return responseList;
            }

            var allocationToObjectList = JsonConvert.DeserializeObject<List<AllocationsJsonMapper>>(allocaionJson);
            foreach(var allocaionMap in allocationToObjectList)
            {
                responseList.Add(new AllocationsResponse
                {
                    SOLineID = allocaionMap.SOLineID,
                    SalesOrderID = allocaionMap.SalesOrderID,
                    SOVersionID = allocaionMap.SOVersionID,
                    LineNum = allocaionMap.LineNum,
                    AccountID = allocaionMap.AccountID,
                    AccountName = allocaionMap.AccountName,
                    OrderQty = allocaionMap.OrderQty,
                    ExternalID= allocaionMap.ExternalID
                });
               
            }
            return responseList;
        }

      

    }

}
