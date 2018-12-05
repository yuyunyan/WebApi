using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using Sourceportal.DB.Comments;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Items;
using Sourceportal.DB.VendorRFQs;
using Sourceportal.Domain.Models.API.Requests.Comments;
using Sourceportal.Domain.Models.API.Requests.VendorRfqs;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.RFQ;
using Sourceportal.Domain.Models.DB.RFQ;

namespace SourcePortal.Services.VendorRFQs
{
    public class VendorRfqService : IVendorRfqService
    {
        private readonly IVendorRfqRepository _rfqRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IItemRepository _itemRepository;

        public VendorRfqService(IVendorRfqRepository rfqRepository, ICommentRepository commentRepository,
            IItemRepository itemRepository)
        {
            _rfqRepository = rfqRepository;
            _commentRepository = commentRepository;
            _itemRepository = itemRepository;
        }

        public RfqDetailsResponse GetRfqBasicDetails(int rfqId)
        {
            var rfqDb = _rfqRepository.GetRfqBasicDetails(rfqId);
            return new RfqDetailsResponse
            {
                ContactId = rfqDb.ContactId,
                StatusId = rfqDb.StatusId,
                SupplierId = rfqDb.AccountId,
                CurrencyId = rfqDb.CurrencyId
            };
        }

        public RfqLinesResponse GetRfqLines(VendorRfqLinesGetRequest vendorRfqLinesGetRequest)
        {
            var rfqLinesDbs = _rfqRepository.GetRfqLines(vendorRfqLinesGetRequest);
            var rfqLines = new List<RfqLines>();

            foreach (var rfqLinesDb in rfqLinesDbs)
            {
                rfqLines.Add(CreateRfqLine(rfqLinesDb));
            }

            return new RfqLinesResponse{RfqLines = rfqLines, TotalRowCount = rfqLinesDbs.Count > 0 ? rfqLinesDbs.First().RowCount : 0 };
        }

        private static RfqLines CreateRfqLine(RfqLinesDb value)
        {
            string buyerTrimmed = null;
            if (value.OwnerName != null)
                buyerTrimmed = value.OwnerName.TrimEnd(' ').TrimEnd(',');

            return new RfqLines
            {
                CommodityID = value.CommodityID,
                CommodityName = value.CommodityName,
                DateCode = value.DateCode,
                HasNoStock = value.HasNoStock,
                ItemID = value.ItemID,
                LineNum = value.LineNum,
                Manufacturer = value.Manufacturer,
                Note = value.Note,
                PackagingID = value.PackagingID,
                PackagingName = value.PackagingName,
                PartNumberStrip = value.PartNumberStrip,
                PartNumber = value.PartNumber,
                Qty = value.Qty,
                SourcesTotalQty = value.SourcesTotalQty,
                StatusID = value.StatusID,
                TargetCost = value.TargetCost,
                VRFQLineID = value.VRFQLineID,
                SupplierName = value.AccountName,
                ContactName = value.ContactName,
                SentDate = value.SentDate,
                Age = value.Age,
                OwnerName = buyerTrimmed,
                VRFQID =  value.VendorRFQID,
                AccountID = value.AccountID,
                ContactID = value.ContactID
            };
        }

        public bool SendRfqAndLines(RfqSendToSupplierRequest rfqSendRequest)
        {
            var rfqIds = new List<int>();
            foreach (var rfqLine in rfqSendRequest.Lines)
            {
                if (rfqLine.IsIHS && rfqLine.ItemId != null)
                {
                    var itemDb = _itemRepository.CreateIhsItemInDb((int) rfqLine.ItemId);
                    rfqLine.ItemId = itemDb.ItemID;
                }
            }
            foreach (var supplier in rfqSendRequest.Suppliers)
            {
                var newRfqRequest = new VendorRfqCreateNewRequest();
                newRfqRequest.SupplierId = supplier.SupplierId;
                newRfqRequest.ContactId = supplier.ContactId;
                newRfqRequest.StatusId = supplier.StatusId;
                newRfqRequest.CurrencyId = supplier.CurrencyId;
                newRfqRequest.Lines = rfqSendRequest.Lines;
                var rfqId = CreateNewRfqAndLines(newRfqRequest);
                if (rfqSendRequest.Comment.Length > 0)
                {
                    var commentRequest = new SetCommentRequest
                    {
                        ObjectID = rfqId,
                        ObjectTypeID = 27,
                        CommentTypeID = 27,
                        Comment = rfqSendRequest.Comment,
                        IsDeleted = 0,
                        ReplyToID = 0
                    };
                    var commentDb = _commentRepository.SetComment(commentRequest);
                }
                
                rfqIds.Add(rfqId);
            }
            return (rfqIds.Count == rfqSendRequest.Suppliers.Count);
        }

        public int CreateNewRfqAndLines(VendorRfqCreateNewRequest request)
        {
            var newRfqDetails = new VendorRfqSaveRequest();
            newRfqDetails.RfqId = 0;
            newRfqDetails.ContactId = request.ContactId;
            newRfqDetails.SupplierId = request.SupplierId;
            newRfqDetails.StatusId = request.StatusId;
            newRfqDetails.CurrencyId = request.CurrencyId;

            int rfqId = SaveBasicDetails(newRfqDetails);
            if (rfqId < 1)
            {
                return rfqId;
            }

            foreach (var line in request.Lines) { 
                var newLine = new RfqLineSaveRequest();
                newLine.VrfqId = rfqId;
                newLine.DateCode = line.DateCode;
                newLine.CommodityId = line.CommodityId;
                newLine.Manufacturer = line.Manufacturer;
                newLine.Note = line.Note;
                newLine.PackagingId = line.PackagingId;
                newLine.PartNumber = line.PartNumber;
                newLine.Qty = line.Qty;
                newLine.TargetCost = line.TargetCost;
                newLine.VrfqLineId = 0;
                newLine.ItemId = line.ItemId;

                var lineResponse = SaveRfqLine(newLine);
            }
            return rfqId;
        }

        public RfqListResponse GetAllRfqs(VendorRfqLineResponsesGetRequest request)
        {
            var response = new List<RfqDetailsResponse>();
            var rfqListDb = _rfqRepository.GetAllRfqs(request);

            foreach (var rfq in rfqListDb)
            {
                string buyerTrimmed = null;
                if (rfq.Buyer != null)
                    buyerTrimmed = rfq.Buyer.TrimEnd(' ').TrimEnd(',');

                response.Add(new RfqDetailsResponse
                {
                    VendorRFQID = rfq.VendorRFQID,
                    StatusId = rfq.StatusId,
                    Buyer = buyerTrimmed,
                    ContactId = rfq.ContactId,
                    ContactName = rfq.Contact,
                    SentDate = rfq.SentDate,
                    StatusName = rfq.StatusName,
                    SupplierId = rfq.AccountId,
                    SupplierName = rfq.AccountName
                });
            }

            return new RfqListResponse { RfqList = response, RowCount = rfqListDb.Count > 0 ? rfqListDb.First().RowCount : 0 };
        }

        public int SaveBasicDetails(VendorRfqSaveRequest vendorRfqSaveRequest)
        {
            var details = _rfqRepository.SaveBasicDetails(vendorRfqSaveRequest);
            return details.VendorRFQID;
            //return new BaseResponse{ ErrorMessage = status.ErrorMessage };
        }

        public RfqLines SaveRfqLine(RfqLineSaveRequest rfqLineSaveRequest)
        {
            var rfqLineDb = _rfqRepository.SaveRfqLine(rfqLineSaveRequest);
            return CreateRfqLine(rfqLineDb);
        }

        public BaseResponse DeleteRfqLines(List<int> rfqLindIds)
        {
            var status = _rfqRepository.DeleteRfqLines(rfqLindIds);
            var response = new BaseResponse{ErrorMessage = status ? null: "Delete failed" };

            return response;
        }

        public RfqLineResponsesResponse GetRfqLineResponses(VendorRfqLineResponsesGetRequest vendorRfqLinesGetRequest)
        {
            var rfqLineResDbs = _rfqRepository.GetRfqLineResponses(vendorRfqLinesGetRequest);
            var responses = new List<RfqLineResponse>();

            foreach (var rfqLineResDb in rfqLineResDbs)
            {
                responses.Add(CreateRfqLineResponse(rfqLineResDb));
            }

            return new RfqLineResponsesResponse { RfqLineResponses = responses, TotalRowCount = rfqLineResDbs.Count > 0 ? rfqLineResDbs.First().RowCount : 0 };
        }

        private RfqLineResponse CreateRfqLineResponse(RfqLineResponseDb rfqLineResDb)
        {
            return new RfqLineResponse
            {
                DateCode = rfqLineResDb.DateCode,
                Manufacturer = rfqLineResDb.Manufacturer,
                PartNumber = rfqLineResDb.PartNumber,
                ItemID = rfqLineResDb.ItemID,
                PackagingId = rfqLineResDb.PackagingId,
                Cost = rfqLineResDb.Cost,
                LeadTimeDays = rfqLineResDb.LeadTimeDays,
                LineNum = rfqLineResDb.LineNum,
                Moq = rfqLineResDb.Moq,
                OfferQty = rfqLineResDb.OfferQty,
                PackagingName = rfqLineResDb.PackagingName,
                SourceId = rfqLineResDb.SourceId,
                Spq = rfqLineResDb.Spq, 
                ValidforHours = rfqLineResDb.ValidForHours,
                Comments = rfqLineResDb.Comments,
                IsNoStock = rfqLineResDb.IsNoStock
            };
        }

        public RfqLineResponse SaveRfqLineResponse(RfqLineResponseSaveRequest rfqLineResponseSaveRequest)
        {
            var rfqLineResponseDb = _rfqRepository.SaveRfqLineResponse(rfqLineResponseSaveRequest);
            return CreateRfqLineResponse(rfqLineResponseDb);
        }

        public BaseResponse DeleteRfqLineResponses(List<int> sourceIds, int rfqLineId)
        {
            var status = _rfqRepository.DeleteRfqLineResponses(sourceIds, rfqLineId);
            var response = new BaseResponse { ErrorMessage = status ? null : "Delete failed" };

            return response;
        }

        public RfqDetailsResponse RfqFromFlaggedSet(
            SetRfqItemsFlaggedRequest setRfqItemsFlaggedRequest)
        {
            var rfqDetailRequest = setRfqItemsFlaggedRequest.RfqDetails;
            rfqDetailRequest.CurrencyId = "USD";
            rfqDetailRequest.StatusId = 1;

            var rfqDetailDb =
                _rfqRepository.SaveBasicDetails(rfqDetailRequest);

            var rfqId = rfqDetailDb.VendorRFQID;

            var rfqLines = setRfqItemsFlaggedRequest.RfqLines;
            var rfqLineDbs = _rfqRepository.SetRfqLineList(rfqId, rfqLines);

            var commentId = _commentRepository.SetComment(new SetCommentRequest
            {
                ObjectID = rfqId,
                ObjectTypeID = (int)ObjectType.VendorRfq,
                Comment = setRfqItemsFlaggedRequest.Comment,
                CommentTypeID = (int)CommentType.CommentRfqResponse
            }).CommentID;

            return new RfqDetailsResponse()
            {
                VendorRFQID = rfqId,
                IsSuccess =  (rfqLineDbs.Count == rfqLines.Count) && (setRfqItemsFlaggedRequest.Comment == null || commentId > 0)
            };
        }
    }
}
