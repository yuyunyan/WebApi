using System.Web.Http;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using SourcePortal.Services.Accounts;
using System.Collections.Generic;
using System.Web.Http.Description;
using System.Web.Routing;
using SourcePortal.Services.CommonData;
using System.Linq;
using System;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Comments;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using Sourceportal.Domain.Models.API.Responses.Sync;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.Middleware.Accounts;

namespace Sourceportal.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountsController : ApiController
    {
        private readonly IAccountService _accountService;
        private readonly ICommonDataService _commonDataService;

        public AccountsController(IAccountService accountService, ICommonDataService commonDataService)
        {
            _accountService = accountService;
            _commonDataService = commonDataService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/accounts/contacts")]
        public ContactListResponse ContactList(ContactsFilter filter)
        {
            return _accountService.GetContactsList(filter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/accounts/contactsByAccountId")]
        public ContactByAccountIdResponse GetContactsByAccountId(int accountId)
        {
            return _accountService.GetContactsByAccountId(accountId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/accounts/getAllAccounts")]
        public AccountsListResponse GetAllAccounts(AccountFilter filter)
        {
            return _accountService.GetAllAccounts(filter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/accounts/getAllAccounts")]
        public AccountsListResponse GetAllAccountsFromUri([FromUri] AccountFilter filter)
        {
            return _accountService.GetAllAccounts(filter);
        }

        [Authorize]
        [HttpGet]
        [Route("api/accounts/getAllAccountsByUserId")]
        public AccountsListResponse GetAllAccountsByUserId(int userId)
        {
            return _accountService.GetAllAccountsByUserId(userId);
        }

        [HttpGet]
        [Route("api/accounts/getAccountsByNameNum")]
        public AccountsListResponse GetAccountsByNameNum([FromUri] AccountsFilter filter)
        {
            return _accountService.GetAccountsByNameNum(filter);
        }

        [Authorize]
        [HttpPost]
        [Route("api/accounts/suppliersLineCardMatch")]
        public AccountsListResponse SuppliersLineCardMatch(SupplierLineCardMatchRequest supplierLineCardMatchRequest)
        {
            return _accountService.SupplierLineCardMatch(supplierLineCardMatchRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/accounts/contactsExport")]
        public ExportResponse ContactExportList(string searchString)
        {
            //Aquire full list
            ContactsFilter filter = new ContactsFilter() { FreeTextSearch = searchString, RowLimit = 999999999 };
            List<ContactResponse> quoteList = _accountService.GetContactsList(filter).Contacts.ToList();

            //Turn list into excel
            string path = "";   //Will get transformed
            string searchName = "";

            //Add search parameter to file name
            if (!string.IsNullOrEmpty(filter.FreeTextSearch))
                searchName = "_Search_" + filter.FreeTextSearch;

            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_ContactsList" + searchName + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<ContactResponse>(quoteList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/accounts/accountsExport")]
        public ExportResponse AccountExportList(string searchString)
        {
            //Aquire full list
            AccountFilter filter = new AccountFilter() { SearchString = searchString, RowLimit = 999999999 };
            List<AllAccounts> quoteList = _accountService.GetAllAccounts(filter).AccountsList.ToList();

            //Turn list into excel
            string path = "";   //Will get transformed
            string searchName = "";

            //Add search parameter to file name
            if (!string.IsNullOrEmpty(filter.SearchString))
                searchName = "_Search_" + filter.SearchString;

            string fileName = DateTime.Now.Month.ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Year.ToString() + "_" + Sourceportal.Utilities.UserHelper.GetUserId() + "_ContactsList" + searchName + ".xlsx";
            ExportResponse export = new ExportResponse();
            string errorMsg = "";
            export.Success = Sourceportal.Utilities.CreateExcelFile.CreateExcelDocument<AllAccounts>(quoteList, ref path, fileName, ref errorMsg);
            export.ErrorMsg = errorMsg;
            //Return download URL
            if (export.Success)
                export.DownloadURL = path.Substring(1); //Remove beginning tilda
            return export;
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getBasicDetails")]
        public AccountBasicDetails GetAccountBasicDetails(int accountId)
        {
            return _accountService.GetBasicDetails(accountId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getBasicDetailsOptions")]
        public BasicDetailsOptions GetBasicDetailsOptions()
        {
            return new BasicDetailsOptions
            {
                StatusList = _accountService.GetAllAccountStatuses(),
                AccountTypes = _accountService.GetAllAccountTypes(),
                CompanyTypes = _accountService.GetAllCompanyTypes()
            };
        }

        [Authorize]
        [HttpPost]
        [Route("api/account/setBasicDetails")]
        public AccountBasicDetails SetBasicDetails(AccountBasicDetails accountBasicDetails)
        {
            return _accountService.SetAccountBasicDetails(accountBasicDetails);
        }

        [Authorize]
        [HttpPost]
        [Route("api/account/setNewAccount")]
        public AccountBasicDetails SetBasicDetails(NewAccountDetails accountBasicDetails)
        {
            return _accountService.SetNewAccountWithLocation(accountBasicDetails);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getLocations")]
        public AccountLocationResponse GetLocations()
        {
            var locations = _accountService.GetLocations();
            return new AccountLocationResponse { Locations = locations };
        }


        [Authorize]
        [HttpGet]
        [Route("api/account/getAccountlLocations")]
        public AccountLocationResponse GetAcccounLocations(int accountId)
        {
            var locations = _accountService.GetAccountLocations(accountId);
            return new AccountLocationResponse { Locations = locations };
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getLocationDetails")]
        public Location GetLocationDetails(int locationId)
        {
            return _accountService.GetLocationDetails(locationId);
            
        }
        [Authorize]
        [HttpGet]
        [Route("api/account/getAccountBillingAddress")]
        public Location GetAccountBillingAddress(int accountId)
        {
            return _accountService.GetAccountBillingAddress(accountId);

        }
        [Authorize]
        [HttpPost]
        [Route("api/account/setLocationDetails")]
        public Location SetLocationDetails(Location location)
        {
            return _accountService.SaveAccountLocation(location);
        }

        [Authorize]
        [HttpPost]
        [Route("api/account/deleteLocation")]
        public Location DeleteLocation(Location location)
        {
            return _accountService.DeleteAccountLocation(location);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/locationTypes")]
        public LocationTypesResponse GetAcccounLocations()
        {
            var locationTypes = _accountService.GetLocationTypes();
            return new LocationTypesResponse {LocationTypes = locationTypes};
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/GetCountryList")]
        public CountryListResponse GetCountryList()
        {
            var countryList = _accountService.GetCountryList();
            return new CountryListResponse { Countries = countryList };
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/GetStateList")]
        public StateListResponse GetStateList(int countryId)
        {
            var countryList = _accountService.GetStateList(countryId);
            return new StateListResponse { StateList = countryList };
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/GetContactDetails")]
        public ContactDetailsResponse GetContactDetails(int contactId)
        {
            return _accountService.GetContactDetails(contactId);
            
        }

        [Authorize]
        [HttpPost]
        [Route("api/account/SetContactDetails")]
        public ContactDetailsResponse SetContactDetails(SetContactDetailsRequest setContactDetailsRequest)
        {
            return _accountService.SetContactDetails(setContactDetailsRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/GetContactDetailOptions")]
        public ContactDetailOptionsResponse GetContactDetailOPtions(int accountId)
        {
            return new ContactDetailOptionsResponse
            {
                Locations = _accountService.GetAccountShippingAddresses(accountId),
                Statuses =  _accountService.GetContactStatuses(),
                PreferredContactMethods = _accountService.GetContactMethods(),
                ContactJobFunctions = _accountService.ContactJobFunctionListGet()
            };
        }

        [Authorize]
        [HttpPost]
        [Route("api/account/setContactOwnership")]
        public ContactDetailsResponse SetContactOwnership(SetContactOwnershipRequest ownershipRequest)
        {
            return _accountService.SetContactOwnership(ownershipRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getSuppliers")]
        public List<AccountBasicDetails> GetSuppliers()
        {
            return _accountService.GetSuppliers();
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getSuppliersandVendors")]
        public List<AccountBasicDetails> GetSuppliersAndVendors(int selectedAccountId = 0)
        {
            return _accountService.GetSuppliersAndVendorsList(selectedAccountId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/GetAccountNonBillingLocations")]
        public AccountLocationResponse GetAccountNonBillingLocations(int accountId)
        {
            var locations = _accountService.GetAccountNonBillingLocations(accountId);
            return new AccountLocationResponse { Locations = locations };

        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getShipToLocations")]
        public AccountLocationResponse GetShipToLocations(int isDropShip, bool filterUnique)
        {
            var locations = _accountService.GetShipToLocations(isDropShip, filterUnique);
            return new AccountLocationResponse { Locations = locations };

        }

        [Authorize]
        [HttpGet]
        [Route("api/contact/getContactProjects")]
        public ContactProjectsGetResponse ContactProjectsGet(int accountId, int contactId)
        {
            return _accountService.ContactProjectsGet(accountId, contactId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/contact/setContactProjects")]
        public BaseResponse ContactProjectsSet(SetContactProjectsRequest setContactProjectsRequest)
        {
            return _accountService.ContactProjectsSet(setContactProjectsRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/contact/getContactFocuses")]
        public ContactFocusesGetResponse ContactFocusesGet(int accountId, int contactId)
        {
            return _accountService.ContactFocusesGet(accountId, contactId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/contact/setContactFocuses")]
        public BaseResponse ContactFocusesSet(SetContactFocusesRequest setContactFocusesRequest)
        {
            return _accountService.ContactFocusesSet(setContactFocusesRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getAccountHierarchies")]
        public AccountHierarchiesGetResponse AccountHierarchiesGet()
        {
            return _accountService.AccountHierarchiesGet();
        }

        [Authorize]
        [HttpGet]
        [Route("api/accounts/getAccountFocusObjectTypes")]
        public List<FocusObjectTypeResponse> GetAccountFocusObjectTypes()
        {
            return _accountService.AccountFocusObjectTypesGet();
        }

        [Authorize]
        [HttpGet]
        [Route("api/accounts/getAccountFocusTypes")]
        public List<FocusTypeResponse> GetAccountFocusTypes()
        {
            return _accountService.AccountFocusTypesGet();
        }
        [Authorize]
        [HttpGet]
        [Route("api/accounts/getAccountFocuses")]
        public List<AccountFocusResponse> AccountFocusesGet(int accountId)
        {
            return _accountService.AccountFocusesGet(accountId);
        }
        [Authorize]
        [HttpPost]
        [Route("api/account/setAccountFocus")]
        public BaseResponse AccountFocusSet(SetAccountFocusRequest setAccountFocusRequest)
        {
            return _accountService.SetAccountFocus(setAccountFocusRequest);
        }
        [Authorize]
        [HttpPost]
        [Route("api/account/deleteAccountFocus")]
        public BaseResponse DeleteAccountFocus(DeleteAccountFocusRequest deleteAccountFocusRequest)
        {
            return _accountService.DeleteAccountFocus(deleteAccountFocusRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getAccountGroupList")]
        public AccountGroupListResponse AccountGroupListGet()
        {
            return _accountService.AccountGroupListGet();
        }

        [Authorize]
        [HttpPost]
        [Route("api/account/deleteAccountGroup")]
        public BaseResponse UserAccountGroupDelete(AccountGroupDeleteRequest accountGroupDeleteRequest)
        {
            return _accountService.UserAccountGroupDelete(accountGroupDeleteRequest.AccountGroup.AccountGroupID);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getAccountGroupDetail")]
        public AccountGroupDetailResponse AccountGroupDetailGet(int accountGroupId)
        {
            return _accountService.AccountGroupDetailGet(accountGroupId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getTypesData")]
        public AccountTypesDataResponse AccountTypesDataGet(int accountId)
        {
            return _accountService.AccountTypesDataGet(accountId);
        }

        [Authorize]
        [HttpPost]
        [Route("api/account/setAccountGroup")]
        public AccountGroupDetailResponse AccountGroupSet (AccountGroupSetRequest accountGroupSetRequest)
        {
            return _accountService.AccountGroupSet(accountGroupSetRequest);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/suppliersAccountGroupMatch")]
        public AccountGroupDetailResponse SuppliersAccountGroupMatch(int accountGroupId)
        {
            return _accountService.SuppliersAccountGroupMatch(accountGroupId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/suppliersAccountGroupListGet")]
        public AccountGroupListResponse SuppliersAccountGroupListGet()
        {
            return _accountService.SuppliersAccountGroupListGet();
        }

        [Authorize]
        [HttpPost]
        [Route("api/account/sync")]
        public SyncResponse SyncAccount(int accountId)
        {
            return _accountService.Sync(accountId);
        }

        //[Authorize]
        [HttpPost]
        [Route("api/accounts/updateExternalIds")]
        public BaseResponse UpdateExternalId(SetAccountExternalIdsRequest request)
        {
            try
            {
                UserHelper.SetMiddlewareUser();
                _accountService.AccountSapDataSet(request);
                return new BaseResponse();
            }catch(Exception e)
            {
                return new BaseResponse { ErrorMessage = String.Format("Exception caught in Web Api Update: {0} | | STACK TRACE: {1}", e.Message, e.StackTrace), IsSuccess = false };
            }
        }



        [Authorize]
        [HttpGet]
        [Route("api/account/getAccountCommentTypeIds")]
        public CommentTypeIdsResponse GetAccountCommentTypeIds()
        {
            var response = new CommentTypeIdsResponse();

            var commentTypeIds = new List<CommentTypeMap>
            {
                new CommentTypeMap()
                {
                    CommentTypeId = (int) CommentType.Financial,
                    TypeName = "Financial"
                },
                new CommentTypeMap()
                {
                    CommentTypeId = (int) CommentType.Account,
                    TypeName = "Account"
                }
            };

            response.CommentTypeIds = commentTypeIds;
            response.IsSuccess = true;
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("api/account/getContactCommentTypeIds")]
        public CommentTypeIdsResponse GetContactCommentTypeIds()
        {
            var response = new CommentTypeIdsResponse();

            var commentTypeIds = new List<CommentTypeMap>
            {
                new CommentTypeMap()
                {
                    CommentTypeId = (int) CommentType.Comment,
                    TypeName = "Comment"
                }
            };

            response.CommentTypeIds = commentTypeIds;
            response.IsSuccess = true;
            return response;
        }

    }
}
