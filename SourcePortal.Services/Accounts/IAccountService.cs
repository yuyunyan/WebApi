using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using Sourceportal.Domain.Models.API.Responses.Sync;

namespace SourcePortal.Services.Accounts
{
    public interface IAccountService
    {
        ContactListResponse GetContactsList(ContactsFilter filter);
        AccountBasicDetails GetBasicDetails(int accountId);
        BasicDetailsOptions GetBasicDetailOptions();
        AccountBasicDetails SetAccountBasicDetails(AccountBasicDetails accountBasicDetails);
        List<AccountStatus> GetAllAccountStatuses();
        List<AccountType> GetAllAccountTypes();
        List<CompanyType> GetAllCompanyTypes();
        IList<Location> GetAccountLocations(int accountId);
        IList<Location> GetLocations();
        Location SaveAccountLocation(Location location);
        Location DeleteAccountLocation(Location location);
        List<LocationType> GetLocationTypes();
        List<Country> GetCountryList();
        List<State> GetStateList(int countryId);
        ContactDetailsResponse GetContactDetails(int contactId);
        ContactDetailsResponse SetContactDetails(SetContactDetailsRequest setContactDetailsRequest);
        IList<Location> GetAccountLocationsForContact(int contactId);
        IList<ContactStatus> GetContactStatuses();
        IList<ContactMethod> GetContactMethods();
        ContactDetailsResponse SetContactOwnership(SetContactOwnershipRequest ownershipRequest);
        Location GetLocationDetails(int locationId);
        Location GetAccountBillingAddress(int accountId);
        IList<Location> GetAccountNonBillingLocations(int accountId);
        List<AccountBasicDetails> GetSuppliers();
        List<AccountBasicDetails> GetSuppliersAndVendorsList(int? selectedAccountId);
        ContactByAccountIdResponse GetContactsByAccountId(int accountId);
        AccountBasicDetails SetNewAccountWithLocation(NewAccountDetails accountDetails);
        AccountsListResponse GetAllAccounts(AccountFilter filter);
        AccountsListResponse GetAccountsByNameNum(AccountsFilter filter);
        AccountsListResponse GetAllAccountsByUserId(int selectedUserId);
        IList<Location> GetShipToLocations(int isDropShip, bool filterUnique);
        IList<ContactJobFunction> ContactJobFunctionListGet();
        ContactProjectsGetResponse ContactProjectsGet(int accountId, int contactId);
        BaseResponse ContactProjectsSet(SetContactProjectsRequest setContactProjectsRequest);
        ContactFocusesGetResponse ContactFocusesGet(int accountId, int contactId);
        BaseResponse ContactFocusesSet(SetContactFocusesRequest setContactFocusesRequest);
        AccountHierarchiesGetResponse AccountHierarchiesGet();
        List<FocusObjectTypeResponse> AccountFocusObjectTypesGet();
        List<FocusTypeResponse> AccountFocusTypesGet();
        List<AccountFocusResponse> AccountFocusesGet(int accountId);
        BaseResponse SetAccountFocus(SetAccountFocusRequest setAccountFocusRequest);
        BaseResponse DeleteAccountFocus(DeleteAccountFocusRequest deleteAccountFocusRequest);
        AccountTypesDataResponse AccountTypesDataGet(int accountId);
        AccountsListResponse SupplierLineCardMatch(SupplierLineCardMatchRequest supplierLineCardMatchRequest);
        AccountGroupListResponse AccountGroupListGet();
        BaseResponse UserAccountGroupDelete(int accountGroupId);
        AccountGroupDetailResponse AccountGroupDetailGet(int accountGroupId);
        AccountGroupDetailResponse AccountGroupSet(AccountGroupSetRequest accountGroupSetRequest);
        AccountGroupDetailResponse SuppliersAccountGroupMatch(int accountGroupId);
        AccountGroupListResponse SuppliersAccountGroupListGet();
        void AccountSapDataSet(SetAccountExternalIdsRequest account);
        SyncResponse Sync(int accountId);
        IList<Location> GetAccountShippingAddresses(int accountId);
    }
}