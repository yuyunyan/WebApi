using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.Quotes;
using Sourceportal.Domain.Models.API.Responses.Quotes;
using SourcePortal.Services.Quotes;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Comments;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using SourcePortal.Services.CommonData;
using Telerik.Reporting;

namespace Sourceportal.API.Controllers
{
    public class QuotesController : ApiController
    {
        private readonly IQuoteService _quoteService;
        private readonly ICommonDataService _commonDataService;

        public QuotesController(IQuoteService quoteService, ICommonDataService commonDataService)
        {
            _quoteService = quoteService;
            _commonDataService = commonDataService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/quote/getQuotesList")]
        public QuoteListResponse GetAllQuoteList(SearchFilter searchFilter)
        {
            return _quoteService.GetQuoteList(searchFilter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuotesExportList")]
        public ExportResponse GetQuoteExportList()
        {
            //Aquire full list
            SearchFilter searchfilter = new SearchFilter { RowLimit = 999999999 };
            List<QuoteResponse> quoteList = _quoteService.GetQuoteList(searchfilter).QuoteList;

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_QuoteList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<QuoteResponse>(quoteList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
            
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteHeader")]
        public QuoteHeaderResponse GetQuoteHeader(int quoteId, int versionId)
        {
            return _quoteService.GetQuoteHeader(quoteId, versionId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteDetails")]
        public QuoteDetailsResponse GetQuoteDetails(int quoteId, int versionId)
        {
            return _quoteService.GetQuoteDetails(quoteId, versionId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/quote/setQuoteDetails")]
        public SetQuoteDetailsResponse SetQuoteDetails(SetQuoteDetailsRequest setQuoteDetailsRequest)
        {
            return _quoteService.SetQuoteDetails(setQuoteDetailsRequest);

        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteOptions")]
        public QuoteOptionsResponse GetQuoteOptions(int accountId)
        {
            //accountId=4 lkpAccountTypes table 
            return new QuoteOptionsResponse
            {
                Customers = _quoteService.GetAllCustomers(),
                Contacts = _quoteService.GetAccountTypeContacts(accountId),
                ShipAddress = _quoteService.GetAccountTypeAddress(accountId),
                Status = _quoteService.GetAllQuoteStatus()
            };
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getAccountsStatusList")]
        public QuoteOptionsResponse GetAccountsList()
        {
            return new QuoteOptionsResponse
            {
                Customers = _quoteService.GetAllCustomers(),
                Status = _quoteService.GetAllQuoteStatus()
            };
        }


        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteTypesList")]
        public QuoteTypesResponse GetQuoteTypes()
        {
            return new QuoteTypesResponse
            {
                QuoteType = _quoteService.GetAllQuoteTypes()
            };
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getPartList")]
        public PartsResponse GetPartLists([FromUri] QuotePartsFilter filter)
        {
            return _quoteService.GetPartsList(filter);
        }


        [Authorize]
        [HttpGet]
        [Route("api/quote/getPartExportList")]
        public ExportResponse GetPartExportLists(int quoteId, int versionId)
        {
            //Aquire full list
            var filter = new QuotePartsFilter();
            filter.QuoteID = quoteId;
            filter.VersionID = versionId;
            SearchFilter searchfilter = new SearchFilter { RowLimit = 999999999 };
            List<PartDetails> quoteList = _quoteService.GetPartsList(filter).PartsListResponse;

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_QuotePartList.xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<PartDetails>(quoteList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;

        }

        [Authorize]
        [HttpPost]
        [Route("api/quote/setPartsDetails")]
        public PartDetails SetPartList(PartDetails setPartsListRequest)
        {
            return _quoteService.SetPartList(setPartsListRequest);
        }


        [Authorize]
        [HttpPost]
        [Route("api/quote/setQuoteLinePrint")]
        public int SetQuoteLinePrint(QuotePrint quotePrint)
        {
            return _quoteService.SetQuoteLinePrint(quotePrint);
        }

        [Authorize]
        [HttpPost]
        [Route("api/quote/deleteParts")]
        public QuotePartsDeleteResponse DeleteQuoteParts(PartDetails[] quoteParts)
        {
            return _quoteService.DeleteQuoteParts(quoteParts.ToList().Select(x => x.QuoteLineId).ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getCommodities")]
        public CommodityOptionsResponse GetCommodities()
        {
            return _quoteService.GetCommodityOptins();
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getPackagingTypes")]
        public PackagingOptionsResponse GetPackagingTypes()
        {
            return _quoteService.GetPackagingOptions();
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getConditionTypes")]
        public PackagingOptionsResponse GetConditionTypes()
        {
            return _quoteService.GetConditionOptions();
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/GetQuoteExtra")]
        public QuoteExtraResponse GetQuoteExtra(int quoteId, int quoteVersionId,int rowOffset,int rowLimit)
        {
            return _quoteService.GetQuoteExtra(quoteId, quoteVersionId,rowOffset,rowLimit);
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/GetQuoteExtraExport")]
        public ExportResponse GetQuoteExtraExport(int quoteId, int quoteVersionId)
        {
            //Aquire full list
            List<ExtraListResponse> quoteList = _quoteService.GetQuoteExtra(quoteId, quoteVersionId, 0, 999999999).ExtraListResponse;

            //Turn list into excel
            string path = "";   //Will get transformed 
            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_QuoteExtra_" + quoteId + "_Version" + quoteVersionId + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<ExtraListResponse>(quoteList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;

            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpPost]
        [Route("api/quote/SetQuoteExtra")]
        public SetQuoteExtraResponse SetQuoteExtra(SetQuoteExtraRequest setQuoteExtraRequest)
        {
            return _quoteService.SetQuoteExtra(setQuoteExtraRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/quote/QuoteToSalesOrder")]
        public QuoteToSOResponse QuoteToSalesOrder(QuoteToSORequest quoteToSoRequest)
        {
            return _quoteService.QuoteToSalesOrder(quoteToSoRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteDetailStatuses")]
        public StatusListResponse Get()
        {
            return _commonDataService.GetStatusesResponse(ObjectType.QuoteDetail);
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteObjectTypeId")]
        public ObjectTypeIdResponse GetQuoteObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int) ObjectType.Quote;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteLineObjectTypeId")]
        public ObjectTypeIdResponse GetQuoteLineObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int) ObjectType.QuoteDetail;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteExtraObjectTypeId")]
        public ObjectTypeIdResponse GetQuoteExtraObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.QuoteExtra;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteCommentTypeIds")]
        public CommentTypeIdsResponse GetQuoteCommentTypeIds()
        {
            var response = new CommentTypeIdsResponse();

            var commentTypeIds = new List<CommentTypeMap>();

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int) CommentType.SalesQuote,
                TypeName = "Sales"
            });

            commentTypeIds.Add(new CommentTypeMap()
            {
                CommentTypeId = (int)CommentType.PurchaseQuote,
                TypeName = "Purchasing"
            });

            response.CommentTypeIds = commentTypeIds;
            response.IsSuccess = true;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuotePartCommentTypeId")]
        public CommentTypeIdResponse GetQuotePartCommentTypeId()
        {
            var response = new CommentTypeIdResponse();
            response.CommentTypeId = (int) CommentType.QuotePart;
            response.IsSuccess = true;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteExtraCommentTypeId")]
        public CommentTypeIdResponse GetQuoteExtraCommentTypeId()
        {
            var response = new CommentTypeIdResponse();
            response.CommentTypeId = (int)CommentType.QuoteExtra;
            response.IsSuccess = true;
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("api/quote/newQuoteExistingCustomerSet")]
        public SetQuoteDetailsResponse SetQuoteExistingCustomer(SetQuoteExistingCustomerRequest setQuoteExistingCustomerRequest)
        {
            return _quoteService.SetQuoteExistingCustomer(setQuoteExistingCustomerRequest);
        }

        [Authorize]
        [HttpPost]
        [Route("api/quote/newQuoteNewCustomerSet")]
        public SetQuoteDetailsResponse SetQuoteNewCustomer(SetQuoteNewCustomerRequest setQuoteNewCustomerRequest)
        {
            return _quoteService.SetQuoteNewCustomer(setQuoteNewCustomerRequest);
        }

        [HttpPost]
        [Route("api/quote/routeQuoteLinesToUsers")]
        public int RouteQuoteLines(RouteQuoteLineRequest routeQuoteLineRequest)
        {
            return _quoteService.RouteQuoteLines(routeQuoteLineRequest);
        }

        [HttpGet]
        [Route("api/quote/sendQuoteEmail")]
        public bool SendQuoteEmail(int quoteId, int quoteVersionId, int accountId, string date, string salesPerson, string salesPersonEmail, string emailTo, string subject, string ccList, string bccList, string body)
        {
            string reportAssembly = "Sourceportal.Reports.QuoteDetails, Sourceportal.Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string fileName = quoteId + "_" + quoteVersionId;
            string exportedPath = null;
            Parameter[] paramList = new Parameter[8];
            string[] ccArr = String.IsNullOrEmpty(ccList)? null : ccList.Split(',');
            string[] bccArr = String.IsNullOrEmpty(bccList) ? null : bccList.Split(',');
            string emailBody = SourcePortal.Services.Mail.MailTemplates.Quote.QuoteAttachment(emailTo, Utilities.UserHelper.GetUserFullName(), body, true);
            paramList[0] = new Parameter()
            {
                Name = "QuoteID",
                Value = quoteId
            };

            paramList[1] = new Parameter()
            {
                Name = "VersionID",
                Value = quoteVersionId
            };
            paramList[2] = new Parameter()
            {
                Name = "UserID",
                Value = Sourceportal.Utilities.UserHelper.GetUserId()
            };
            paramList[3] = new Parameter()
            {
                Name = "AccountID",
                Value = accountId
            };
            paramList[4] = new Parameter()
            {
                Name = "Date",
                Value = String.IsNullOrEmpty(date) ? "" : date
            };
            paramList[5] = new Parameter()
            {
                Name = "Salesperson",
                Value = String.IsNullOrEmpty(salesPerson) ? "" : salesPerson
            };
            paramList[6] = new Parameter()
            {
                Name = "SalespersonEmail",
                Value = String.IsNullOrEmpty(salesPersonEmail) ? "" : salesPersonEmail
            };
            paramList[7] = new Parameter()
            {
                Name = "ContactDisclaimer",
                Value = "'For questions regarding this quote please contact: " + salesPerson + " 123 - 123 - 1234 or " + salesPersonEmail
            };

            //prepare report
            exportedPath = Reports.Utilities.ExportReport(reportAssembly, fileName, paramList);

            //email report
            SourcePortal.Services.Mail.EmailService.SendEmail(Utilities.UserHelper.GetUserEmail(), Utilities.UserHelper.GetUserFullName(), emailTo, subject, emailBody, ccArr, bccArr, exportedPath);

            return (string.IsNullOrEmpty(exportedPath) ? false : true);

        }

        [Authorize]
        [HttpGet]
        [Route("api/quote/getQuoteLineByAccountId")]
        public QuoteLineHistoryResponse GetQuoteLineByAccountId (int accountId, int? contactId=null)
        {
            return _quoteService.GetQuoteLineByAccountId(accountId, contactId);

        }
    }
}
