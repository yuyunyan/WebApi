using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using Sourceportal.Domain.Models.DB.Accounts;
using Sourceportal.Domain.Models.Shared;

namespace Sourceportal.DB.Accounts
{
    public interface IAccountRepository
    {
        IList<ContactListContactDb> GetContactsList(ContactsFilter filter);
        AccountBasicDetailsDb GetAccountBasicDetails(int accountId);
        AccountBasicDetailsDb SaveBasicDetails(AccountBasicDetails accountBasicDetails);
        IList<LocationDb> GetLocations();
        IList<LocationDb> GetAccountLocations(int accountId);
        LocationDb SaveLocation(Location location);
        IList<CountryDb> GetCountryList();
        IList<StateDb> GetStateList(int countryId);
        ContactDetailsDb GetContactDetails(int contactId);
        string GetJobFunctionExternalId(int jobFunctionId);
        int GetJobFunctionIdByExternal(string jobFunctionExternalId);
        List<OwnerDb> GetOwners(int contactId);
        ContactDetailsDb SetContactDetails(SetContactDetailsRequest setContactDetailsRequest);
        IList<OwnerDb> SetAccountOwnership(SetContactOwnershipRequest ownershipRequest);
        LocationDb GetLocationDetails(int locationId);
        IList<LocationDb> GetAccountNonBillingLocations(int accountId);
        LocationDb GetAccountBillingAddress(int accountId);
        IList<LocationDb> GetAccountShippingAddresses(int accountId);
        List<AccountBasicDetailsDb> GetSuppliersList();
        List<AccountBasicDetailsDb> GetSuppliersAndVendorsList(int? selectedAccountId);
        //AccountBasicDetailsDb AccountInfoSet(AccountBasicDetails accountBasicDetails, int organizationId);
        List<AccountStatusDb> GetAccountStatusDbs();
        List<AccountTypeDb> GetAccountTypesDbs();
        IList<ContactsByAccountIdDb> GetContactsList(int accountId);
        IList<AccountsDb> GetAllAccounts(AccountFilter filter);
        IList<AccountsDb> GetAccountsByNameNum(AccountsFilter filter);
        IList<AccountsDb> GetAllAccountsByUserId(int userId);
        IList<LocationDb> GetShipToLocations(int isDropShip);
        IList<AccountLocationTypeDb> GetAccountLocationTypeDbs();
        ContactDetailsDb CreateContactWithUserOwnership(SetContactDetailsRequest setContactDetailsRequest);
        IList<ContactJobFunctionDb> ContactJobFunctionListGet();
        IList<ContactProjectsDb> ContactProjectListGet(int accountId, int contactId);
        int ContactProjectListSet(SetContactProjectsRequest setContactProjectsRequest);
        IList<ContactFocusesDb> ContactFocusListGet(int accountId, int contactId);
        int ContactFocusListSet(SetContactFocusesRequest setContactFocusesRequest);
        List<AccountHierarchyDb> AccountHierarchiesGet(int? hierarchyId);
        List<AccountHierarchyDb> AccountHierarchiesGetByExternal(string sapHierarchyId);
        List<AccountHierarchyDb> AccountHierarchyChildrenFromParentGet(int hierarchyId);
        int AccountHierarchyParentGetIdByChild(int childId);
        List<AccountFocusObjectTypeDb> AccountFocusObjectTypesGet();
        List<AccountFocusTypeDb> AccountFocusTypesGet();
        List<AccountFocusDb> AccountFocusesGet(int accountId);
        int SetAccountFocus(SetAccountFocusRequest setAccountFocusRequest);
        int DeleteAccountFocus(DeleteAccountFocusRequest deleteAccountFocusRequest);
        int CreateParentHierarchy(AccountBasicDetails accountBasicDetails);
        List<AccountTypesDataDb> AccountTypesDataGet(int accountId);
        IList<AccountsDb> SupplierLineCardMatch(List<int> commodities, List<MfrObject> mfrNames);
        IList<AccountGroupDb> AccountGroupListGet();
        bool UserAccountGroupDelete(int accountGroupId);
        IList<AccountGroupDetailDb> AccountGroupDetailGet(int accountGroupId);
        int AccountGroupSet(AccountGroupSetRequest accountGroupSetRequest);
        IList<AccountGroupDetailDb> SuppliersAccountGroupMatch(int accountGroupId);
        IList<AccountGroupDb> SuppliersAccountGroupListGet();
        List<AccountStatusDb> GetAccountStatuses(int accountId);
        void LocationSapDataSet(PairedIdsRequest contact);
        void ContactSapDataSet(PairedIdsRequest contact);
        void AccountSapDataSet(SetAccountExternalIdsRequest account, AccountTypeDetails customer, AccountTypeDetails supplier, int? status);
        int HierarchySapDataSet(AccountHierarchySapDetails hierarchy);
        void HierarchyChildSapDataSet(AccountHierarchySapDetails hierarchy);
        List<AccountHierarchyDb> AccountHierarchiesGet();
        AccountBasicDetailsDb DeleteAccount(AccountBasicDetails accountBasicDetails);
        ContactDetailsDb DeleteContact(SetContactDetailsRequest setContactDetailsRequest);
        int GetAccountIdByExternal(string externalId);
        int GetContactIdByExternal(string externalId);
        int GetLocationIdByExternal(string externalId);
    }
}