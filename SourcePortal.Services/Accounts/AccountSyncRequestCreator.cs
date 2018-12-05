using Sourceportal.DB.Accounts;
using Sourceportal.DB.CommonData;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.DB.Accounts;
using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.Accounts;
using Sourceportal.Domain.Models.Middleware.Enums;
using Sourceportal.Utilities;
using SourcePortal.Services.Shared.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.Accounts
{
    public class AccountSyncRequestCreator : IAccountSyncRequestCreator
    {

        private readonly IAccountRepository _accountRepository;
        private readonly ICommonDataRepository _commonRepository;
        private readonly ISyncOwnershipCreator _syncOwnershipCreator;

        public AccountSyncRequestCreator(IAccountRepository accountRepository, ISyncOwnershipCreator syncOwnershipCreator, ICommonDataRepository commonRepository)
        {
            _accountRepository = accountRepository;
            _commonRepository = commonRepository;
            _syncOwnershipCreator = syncOwnershipCreator;
        }

        public MiddlewareSyncRequest<AccountSync> Create(int accountId)
        {
            var syncRequest = new MiddlewareSyncRequest<AccountSync>(
                accountId, MiddlewareObjectTypes.Account.ToString(), 
                UserHelper.GetUserId(), 
                (int)ObjectType.Accounts);

            var accountSync = AccountSync(accountId);
            syncRequest.Data = accountSync;
            return syncRequest;
        }

        private AccountSync AccountSync(int accountId)
        {
            var accountDetails = _accountRepository.GetAccountBasicDetails(accountId);
            var accountSync = new AccountSync(accountId, accountDetails.ExternalId);

            accountSync.BpDetails = SetAccountDetails(accountDetails);

            accountSync.Contacts = SetContacts(_accountRepository.GetContactsList(accountId));

            accountSync.Locations = SetLocations(_accountRepository.GetAccountLocations(accountId));

            return accountSync;
        }

        private AccountDetails SetAccountDetails(AccountBasicDetailsDb account)
        {
            var details = new AccountDetails();

            details.ExternalID = account.ExternalId;
            details.Name = account.AccountName;
            details.Website = account.Website;
            details.Email = account.Email;
            details.VendorNum = account.VendorNum;
            details.ApprovedVendor = account.ApprovedVendor;
            details.CompanyType = _commonRepository.GetCompanyTypes(account.CompanyTypeId).First().ExternalId;
            details.Ownership = _syncOwnershipCreator.Create(account.AccountId, ObjectType.Accounts);
            details.DBNum = account.DBNum;

            //choose active status.  if none active just choose one
            var statuses = _accountRepository.GetAccountStatuses(account.AccountId);
            foreach (var status in statuses)
            {
                if (status.AccountIsActive)
                    details.StatusExternalID = status.ExternalId;
            }
            if (string.IsNullOrEmpty(details.StatusExternalID))
                details.StatusExternalID = statuses.First().ExternalId;


            details.AccountTypes = new List<string>();
            if ((account.AccountTypeBitwise & 1) == 1)
            {
                details.AccountTypes.Add("supplier");
            }
            if((account.AccountTypeBitwise & 4) == 4)
            {
                details.AccountTypes.Add("customer");

                if(account.AccountHierarchyId > 0)
                    details.Hierarchy = SetAccountHierarchy(account.AccountHierarchyId);
            }            

            return details;
        }

        private AccountHierarchyRequest SetAccountHierarchy(int? hierarchyId)
        {
            var hierarchy = new AccountHierarchyRequest();

            if (hierarchyId == null)
                return hierarchy;

            var hierarchyDb = _accountRepository.AccountHierarchiesGet(hierarchyId).First();
            var hierarchyParentDb = _accountRepository.AccountHierarchiesGet(hierarchyDb.ParentID).First();

            hierarchy.Id =  (int)hierarchyId;
            hierarchy.SapHierarchyId = hierarchyDb.SAPHierarchyID;
            hierarchy.ParentName = hierarchyParentDb.HierarchyName;
            hierarchy.ParentGroupId = hierarchyParentDb.SAPGroupID;
            hierarchy.RegionId = hierarchyDb.RegionID;
            hierarchy.ChildGroupId = hierarchyDb.SAPGroupID;

            return hierarchy;
        }

        private List<ContactDetails> SetContacts(IList<ContactsByAccountIdDb> contactsDb)
        {
            var contacts = new List<ContactDetails>();

            foreach(var contactDb in contactsDb)
            {
                var contact = new ContactDetails();
                var contactDetailsDb = _accountRepository.GetContactDetails(contactDb.ContactId);

                contact.ContactId = contactDetailsDb.ContactId;
                contact.ExternalId = contactDetailsDb.ExternalId;
                contact.FirstName = contactDetailsDb.FirstName;
                contact.LastName = contactDetailsDb.LastName;
                contact.OfficePhone = contactDetailsDb.OfficePhone;
                contact.Email = contactDetailsDb.Email;
                contact.LocationExternalId = contactDetailsDb.LocationId == 0 ? null : _accountRepository.GetLocationDetails(contactDetailsDb.LocationId).ExternalID;
                contact.JobFunctionExternalId = contactDb.JobFunctionID != null ? _accountRepository.GetJobFunctionExternalId((int)contactDb.JobFunctionID) : null;

                contacts.Add(contact);
            }

            return contacts;
        }

        private List<LocationDetails> SetLocations(IList<LocationDb> locationsDb)
        {
            var locations = new List<LocationDetails>();

            foreach(var locationDb in locationsDb)
            {
                var location = new LocationDetails();

                location.LocationId = locationDb.LocationID;
                location.Name = locationDb.Name;
                location.ExternalId = locationDb.ExternalID;
                location.LocationTypeExternalId = locationDb.LocationTypeExternalID.Split(',').Select(x => x.Trim()).ToList();
                location.CountryExternalId = locationDb.CountryCode2;
                location.Address1 = locationDb.Address1;
                location.Address2 = locationDb.Address2;
                location.Address4 = locationDb.Address4;
                location.HouseNo = locationDb.HouseNumber;
                location.Street = locationDb.Street;
                location.City = locationDb.City;
                location.StateExternalId = locationDb.StateCode;
                location.PostalCode = locationDb.PostalCode;
                location.District = locationDb.District;

                locations.Add(location);
            }

            return locations;
        }
    }
}
