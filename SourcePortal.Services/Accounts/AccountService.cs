using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Sourceportal.DB.Accounts;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using Sourceportal.Domain.Models.DB.Accounts;
using Sourceportal.Domain.Models.SAP_API.Requests;
using Sourceportal.Domain.Models.SAP_API.Responses;
using Sourceportal.Domain.Models.Shared;
using Sourceportal.Utilities;
using SourcePortal.Services.ApiService;
using Sourceportal.Domain.Models.API.Responses.Sync;
using SourcePortal.Services.Shared.Middleware;
using Sourceportal.DB.CommonData;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.Middleware.Accounts;
using Sourceportal.Domain.Models.Middleware.Enums;
using LocationType = Sourceportal.Domain.Models.API.Responses.Accounts.LocationType;

namespace SourcePortal.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountSyncRequestCreator _accountSyncRequestCreator;
        private readonly IMiddlewareService _middlewareService;
        private readonly IRestClient _restClient;
        private readonly ICommonDataRepository _commonDataRepository;

        public AccountService(IAccountRepository accountRepository, IRestClient restClient, IAccountSyncRequestCreator accountSyncRequestCreator,
            IMiddlewareService middlewareService, ICommonDataRepository commonDataRepository)
        {
            _accountRepository = accountRepository;
            _accountSyncRequestCreator = accountSyncRequestCreator;
            _middlewareService = middlewareService;
            _restClient = restClient;
            _commonDataRepository = commonDataRepository;
        }

        public ContactListResponse GetContactsList(ContactsFilter filter)
        {
            var dbContacts =_accountRepository.GetContactsList(filter);
            var list = new List<ContactResponse>();
            int TotalCount = 0;
            
            foreach (var dbContact in dbContacts)
            {
                list.Add(new ContactResponse
                {
                    ContactId = dbContact.ContactId,
                    AccountTypes = !string.IsNullOrEmpty(dbContact.AccountTypes) ? CleanLists(dbContact.AccountTypes).Split(',').ToList()
                    .Select(x => new AccountType {Name = x}).ToList(): null,
                    Owners = !string.IsNullOrEmpty(dbContact.Owners) ? CleanLists(dbContact.Owners).Split(',').ToList()
                    .Select(x => new Owner {Name = x}).ToList() : null,
                    AccountId = dbContact.AccountId,
                    AccountName = dbContact.AccountName,
                    AccountStatus = dbContact.AccountStatus,
                    Email = dbContact.Email,
                   ExternalId = dbContact.ExternalID,
                    FirstName = dbContact.FirstName,
                    LastName = dbContact.LastName,
                    IsActive = dbContact.IsActive,
                    Phone = dbContact.OfficePhone,
                    Title = dbContact.Title
                });
            }
            
            if (dbContacts.Count() > 0)
                TotalCount = dbContacts[0].TotalRows;

            return new ContactListResponse {Contacts = list, TotalRowCount = TotalCount };
        }
        
        public AccountBasicDetails GetBasicDetails(int accountId)
        {
            var basicDetailsDb = _accountRepository.GetAccountBasicDetails(accountId);

            return CreateAccountBasicDetails(basicDetailsDb);
        }

        private AccountBasicDetails CreateAccountBasicDetails(AccountBasicDetailsDb basicDetailsDb)
        {
            var response = new AccountBasicDetails();
            response.Name = basicDetailsDb.AccountName;
            response.Number = basicDetailsDb.AccountNum;
            response.AccountTypeIds = GetAccountTypeIds(basicDetailsDb.AccountTypeBitwise);
            response.CompanyTypeId = basicDetailsDb.CompanyTypeId;
            response.StatusId = basicDetailsDb.AccountStatusId;
            response.StatusExternalId = basicDetailsDb.StatusExternalId;
            response.AccountId = basicDetailsDb.AccountId;
            response.ExternalId = basicDetailsDb.ExternalId;
            response.CurrencyId = basicDetailsDb.CurrencyId;
            response.PaymentTermID = basicDetailsDb.PaymentTermID;
            response.OrganizationId = basicDetailsDb.OrganizationId;
            response.AccountHierarchyId = basicDetailsDb.AccountHierarchyId;
            response.CreditLimit = basicDetailsDb.CreditLimit;
            response.OpenBalance = basicDetailsDb.OpenBalance;
            response.Email = basicDetailsDb.Email;
            response.Website = basicDetailsDb.Website;
            response.YearEstablished = basicDetailsDb.YearEstablished;
            response.NumOfEmployees = basicDetailsDb.NumOfEmployees;
            response.EndProductFocus = basicDetailsDb.EndProductFocus;
            response.CarryStock = basicDetailsDb.CarryStock;
            response.MinimumPO = basicDetailsDb.MinimumPO;
            response.ShippingInstructions = basicDetailsDb.ShippingInstructions;
            response.VendorNum = basicDetailsDb.VendorNum;
            response.SupplierRating = basicDetailsDb.SupplierRating;
            response.QCNotes = basicDetailsDb.QCNotes;
            response.PONotes = basicDetailsDb.PONotes;
            response.ApprovedVendor = basicDetailsDb.ApprovedVendor;
            response.IncotermID = basicDetailsDb.IncotermID;
            response.DBNum = basicDetailsDb.DBNum;
            return response;
        }

        public static List<int> GetAccountTypeIds(int accountTypeBitwise)
        {
            return Enum.GetValues(typeof(AccountTypes))
                .Cast<int>()
                .Where(enumValue => enumValue != 0 && (enumValue & accountTypeBitwise) == enumValue)
                .ToList();
        }

        public BasicDetailsOptions GetBasicDetailOptions()
        {
            throw new System.NotImplementedException();
        }

        public AccountBasicDetails SetNewAccountWithLocation(NewAccountDetails accountDetails)
        {
            var basicDetailsDb = _accountRepository.SaveBasicDetails(accountDetails);
            accountDetails.Location.AccountId = basicDetailsDb.AccountId;

            _accountRepository.SaveLocation(accountDetails.Location);
           // _accountRepository.SetContactDetails(accountDetails.Contact);
            accountDetails.Contact.AccountId = basicDetailsDb.AccountId;
            if (accountDetails.Contact.FirstName != null && accountDetails.Contact.LastName != null)
            {
                var newContactdB = _accountRepository.CreateContactWithUserOwnership(accountDetails.Contact);
            }
            
            if (!string.IsNullOrEmpty(basicDetailsDb.Error))
            {
                return new AccountBasicDetails { ErrorMessage = basicDetailsDb.Error };
            }

            return CreateAccountBasicDetails(basicDetailsDb);
        }

        public AccountBasicDetails SetAccountBasicDetails(AccountBasicDetails accountBasicDetails)
        {
            if (accountBasicDetails.HierarchyName != null)
            {
                var newChildHierarchyID = _accountRepository.CreateParentHierarchy(accountBasicDetails);
                accountBasicDetails.AccountHierarchyId = newChildHierarchyID;
            }
            var basicDetailsDb = accountBasicDetails.IsDeleted == 1 ? _accountRepository.DeleteAccount(accountBasicDetails) : _accountRepository.SaveBasicDetails(accountBasicDetails);

            if (!string.IsNullOrEmpty(basicDetailsDb.Error))
            {
                return new AccountBasicDetails {ErrorMessage = basicDetailsDb.Error};
            }

            return CreateAccountBasicDetails(basicDetailsDb);
        }

        public List<AccountStatus> GetAllAccountStatuses()
        {
            var accountStatuses = new List<AccountStatus>();
            var accountStatusDbs = _accountRepository.GetAccountStatusDbs();
            foreach (var accountStatusDb in accountStatusDbs)
            {
                accountStatuses.Add(new AccountStatus {Name = accountStatusDb.StatusName, Id = accountStatusDb.AccountStatusId, ExternalId = accountStatusDb.ExternalId});
            }

            return accountStatuses;
        }

        public List<AccountType> GetAllAccountTypes()
        {
            var accountTypes = new List<AccountType>();
            var accountTypeDbs = _accountRepository.GetAccountTypesDbs();
            foreach (var value in accountTypeDbs)
            {
                accountTypes.Add(new AccountType { Name = value.Name, Id = value.Id });
            }

            return accountTypes;
        }

        public List<CompanyType> GetAllCompanyTypes()
        {
            var companyTypesDb = _commonDataRepository.GetCompanyTypes();
            var companyTypes = new List<CompanyType>();
            
            foreach (var value in companyTypesDb)
            {
                companyTypes.Add(new CompanyType { Name = value.Name, Id = value.CompanyTypeID, ExternalId = value.ExternalId });
            }

            return companyTypes;
        }

        private static Location CreateLocation(LocationDb dbLocation)
        {
            return new Location
            {
                Name = dbLocation.Name,
                AccountId = dbLocation.AccountId,
                TypeId = dbLocation.LocationTypeId,
                Street = dbLocation.Street,
                AddressLine1 = dbLocation.Address1,
                AddressLine2 = dbLocation.Address2,
                AddressLine4 = dbLocation.Address4,
                City = dbLocation.City,
                CountryId = dbLocation.CountryId,
                CountryCode = dbLocation.CountryCode,
                CountryCode2 = dbLocation.CountryCode2,
                District = dbLocation.District,
                FormattedAddress = AddressFormatter.FormatAddress(dbLocation),
                HouseNo = dbLocation.HouseNumber,
                IsDeleted = dbLocation.IsDeleted,
                LocationId = dbLocation.LocationID,
                LocationTypeName = ((LocationTypesEnum)dbLocation.LocationTypeId).GetEnumDescription(),
                PostalCode = dbLocation.PostalCode,
                StateId = dbLocation.StateId,
                StateCode = dbLocation.StateCode,
                StateName = dbLocation.StateName,
                FormattedState = !string.IsNullOrEmpty(dbLocation.StateCode) ? dbLocation.StateCode : dbLocation.StateName,
                Note = dbLocation.Note,
                ErrorMessage = dbLocation.Error
            };
        }

        public IList<Location> GetLocations()
        {
            var dbLocations = _accountRepository.GetLocations();

            var locations = new List<Location>();

            foreach (var dbLocation in dbLocations)
            {
                locations.Add(CreateLocationFromDbLocation(dbLocation));
            }

            return locations;

        }

        public IList<Location> GetAccountShippingAddresses(int accountId)
        {
            var dbLocations = _accountRepository.GetAccountShippingAddresses(accountId);

            var locations = new List<Location>();

            foreach (var dbLocation in dbLocations)
            {
                locations.Add(CreateLocationFromDbLocation(dbLocation));
            }

            return locations;
        }

        public IList<Location> GetAccountLocations(int accountId)
        {
            var dbLocations = _accountRepository.GetAccountLocations(accountId);

            var locations = new List<Location>();
            
            foreach (var dbLocation in dbLocations)
            {
                locations.Add(CreateLocationFromDbLocation(dbLocation));
            }
            
            return locations;

        }

        private static Location CreateLocationFromDbLocation(LocationDb dbLocation)
        {
            List<string> locationTypeExternalIds = new List<string>();
            if (dbLocation.LocationTypeExternalID != null)
            {
                locationTypeExternalIds = dbLocation.LocationTypeExternalID.Split(',').Select(sValue => sValue.Trim()).ToList();
            }
            
            return new Location
            {
                Name = dbLocation.Name,
                ExternalId = dbLocation.ExternalID,
                LocationTypeExternalId = locationTypeExternalIds,
                AccountId = dbLocation.AccountId,
                TypeId = dbLocation.LocationTypeId,
                Street = dbLocation.Street,
                AddressLine1 = dbLocation.Address1,
                AddressLine2 = dbLocation.Address2,
                AddressLine4 = dbLocation.Address4,
                City = dbLocation.City,
                CountryId = dbLocation.CountryId,
                CountryCode = dbLocation.CountryCode,
                CountryCode2 = dbLocation.CountryCode2,
                District = dbLocation.District,
                FormattedAddress = AddressFormatter.FormatAddress(dbLocation),
                HouseNo = dbLocation.HouseNumber,
                IsDeleted = dbLocation.IsDeleted,
                LocationId = dbLocation.LocationID,
                LocationTypeName = dbLocation.LocationTypeName,//((LocationTypesEnum)dbLocation.LocationTypeId).GetEnumDescription(),
                PostalCode = dbLocation.PostalCode,
                StateId = dbLocation.StateId,
                StateCode = dbLocation.StateCode,
                StateName = dbLocation.StateName,
                FormattedState = !string.IsNullOrEmpty(dbLocation.StateCode) ? dbLocation.StateCode : dbLocation.StateName,
                Note = dbLocation.Note,
                ErrorMessage = dbLocation.Error
            };
        }

        public Location SaveAccountLocation(Location location)
        {
            var locationDb = _accountRepository.SaveLocation(location);

            if (!string.IsNullOrEmpty(locationDb.Error))
            {
                return new Location { ErrorMessage = locationDb.Error };
            }
            
            return CreateLocationFromDbLocation(locationDb);
        }

        public Location DeleteAccountLocation(Location location)
        {
            location.IsDeleted = true;
            var locationDb = _accountRepository.SaveLocation(location);

            if (!string.IsNullOrEmpty(locationDb.Error))
            {
                return new Location { ErrorMessage = locationDb.Error };
            }

            return CreateLocationFromDbLocation(locationDb);
        }

        public List<LocationType> GetLocationTypes()
        {
            var companyTypes = new List<LocationType>();

            var locationTypeDbs = _accountRepository.GetAccountLocationTypeDbs();
            foreach (var value in locationTypeDbs)
            {
                companyTypes.Add(new LocationType { Name = value.Name , Id = value.LocationTypeID, ExternalId = value.ExternalID});
            }

            return companyTypes;
        }

        public List<Country> GetCountryList()
        {
           var countryDbList = _accountRepository.GetCountryList();

            var countryList = new List<Country>();
            foreach (var countryDb in countryDbList)
            {
               countryList.Add(new Country
               {
                   Name = countryDb.CountryName,
                   Id = countryDb.CountryId,
                   Code = countryDb.CountryCode,
                   CodeForSap = countryDb.CountryCode2
               });
            }

            return countryList;
        }

        public List<State> GetStateList(int countryId)
        {
            var stateDbList = _accountRepository.GetStateList(countryId);
            var stateList = new List<State>();

            foreach (var stateDb in stateDbList)
            {
                stateList.Add(new State { Id = stateDb.StateId , Name = stateDb.StateName, Code = stateDb.StateCode});
            }

            return stateList;
        }

        public ContactDetailsResponse GetContactDetails(int contactId)
        {
            var contactDetailsDb = _accountRepository.GetContactDetails(contactId);
            var ownerDbs = _accountRepository.GetOwners(contactId);
            
            return MapContactDetailsResponse(contactDetailsDb, ownerDbs);
        }
        
        public ContactDetailsResponse SetContactDetails(SetContactDetailsRequest setContactDetailsRequest)
        {
            var contactDetailsDb = setContactDetailsRequest.IsDeleted ? _accountRepository.DeleteContact(setContactDetailsRequest): _accountRepository.SetContactDetails(setContactDetailsRequest);
            var ownerDbs = _accountRepository.GetOwners(contactDetailsDb.ContactId);

            return MapContactDetailsResponse(contactDetailsDb, ownerDbs);
        }

        public IList<Location> GetAccountLocationsForContact(int contactId)
        {
            var contactDetails = _accountRepository.GetContactDetails(contactId);
            return GetAccountLocations(contactDetails.AccountId);
        }

        public IList<ContactStatus> GetContactStatuses()
        {
            var companyTypes = new List<ContactStatus>();

            var values = Enum.GetValues(typeof(ContactStatusEnum));
            foreach (var value in values)
            {
                companyTypes.Add(new ContactStatus { Name = ((ContactStatusEnum)value).GetEnumDescription(), Id = (int)value });
            }

            return companyTypes;
        }

        public IList<ContactMethod> GetContactMethods()
        {
            var companyTypes = new List<ContactMethod>();

            var values = Enum.GetValues(typeof(ContactMethodEnum));
            foreach (var value in values)
            {
                companyTypes.Add(new ContactMethod { Name = ((ContactMethodEnum)value).GetEnumDescription(), Id = (int)value });
            }

            return companyTypes;
        }

        public ContactDetailsResponse SetContactOwnership(SetContactOwnershipRequest ownershipRequest)
        {
            var ownerDbs = _accountRepository.SetAccountOwnership(ownershipRequest);
            var contactDetailsDb = _accountRepository.GetContactDetails(ownershipRequest.ContactId);
            return MapContactDetailsResponse(contactDetailsDb, ownerDbs.ToList());
        }

        public Location GetLocationDetails(int locationId)
        {
            var locationDb = _accountRepository.GetLocationDetails(locationId);
            return CreateLocationFromDbLocation(locationDb);
        }



        public Location GetAccountBillingAddress(int accountId)
        {
            var locationDb = _accountRepository.GetAccountBillingAddress(accountId);
            if (locationDb == null)
            {
                return null;
            }
            return CreateLocationFromDbLocation(locationDb);
        }

        public IList<Location> GetAccountNonBillingLocations(int accountId)
        {
            var locations = new List<Location>();
            var locationsDb = _accountRepository.GetAccountNonBillingLocations(accountId);

            foreach (var dbLocation in locationsDb)
            {
                locations.Add(CreateLocationFromDbLocation(dbLocation));
            }

            return locations;
        }

        public IList<Location> GetShipToLocations(int isDropShip, bool filterUnique)
        {
            var locations = new List<Location>();
            var locationsDb = _accountRepository.GetShipToLocations(isDropShip);
            string locationsString = "";
            foreach (var dbLocation in locationsDb)
            {
                if (filterUnique)
                {
                    if (!locationsString.Contains("/" + dbLocation.LocationID.ToString() + "/"))
                    {
                        locations.Add(CreateLocation(dbLocation));
                        locationsString += "/" + dbLocation.LocationID.ToString() + "/";
                    }
                }
                else
                {
                    locations.Add(CreateLocation(dbLocation));
                }
            }
            return locations;
        }

        private static string CleanLists(string stringToClean)
        {
            return !string.IsNullOrEmpty(stringToClean) ? stringToClean.Trim().TrimEnd(',') : null;
        }

        private static ContactDetailsResponse MapContactDetailsResponse(ContactDetailsDb contactDetailsDb, List<OwnerDb> ownerDbs)
        {
            var owners = new List<Owner>();

            foreach (var ownerDb in ownerDbs)
            {
                owners.Add(new Owner
                {
                    Name = ownerDb.OwnerFirstName + " " + ownerDb.OwnerLastName,
                    UserId = ownerDb.OwnerId,
                    Percentage = ownerDb.Percent
                });
            }

            return new ContactDetailsResponse
            {
                ContactId = contactDetailsDb.ContactId,
                AccountName = contactDetailsDb.AccountName,
                Email = contactDetailsDb.Email,
                FirstName = contactDetailsDb.FirstName,
                LastName = contactDetailsDb.LastName,
                Fax = contactDetailsDb.Fax,
                LocationId = contactDetailsDb.LocationId,
                MobilePhone = contactDetailsDb.MobilePhone,
                OfficePhone = contactDetailsDb.OfficePhone,
                PreferredContactMethodId = contactDetailsDb.PreferredContactMethodId,
                IsActive = contactDetailsDb.IsActive,
                Title = contactDetailsDb.Title,
                Owners = owners,
                ExternalId = contactDetailsDb.ExternalId,
                Note = contactDetailsDb.Note,
                Department = contactDetailsDb.Department,
                JobFunctionID = contactDetailsDb.JobFunctionID,
                Birthdate = contactDetailsDb.Birthdate,
                Gender = contactDetailsDb.Gender,
                Salutation = contactDetailsDb.Salutation,
                MaritalStatus = contactDetailsDb.MaritalStatus,
                KidsNames = contactDetailsDb.KidsNames,
                ReportsTo = contactDetailsDb.ReportsTo,
                AccountTypeIds = GetAccountTypeIds(contactDetailsDb.AccountTypeBitwise)
            };
        }
        public List<AccountBasicDetails> GetSuppliersAndVendorsList(int? selectedAccountId)
        {
            var suppliersDb = _accountRepository.GetSuppliersAndVendorsList(selectedAccountId);
            var suppliers = new List<AccountBasicDetails>();

            foreach (var supplier in suppliersDb)
            {
                suppliers.Add(new AccountBasicDetails
                {
                    AccountId = supplier.AccountId,
                    Name = supplier.AccountName,
                    StatusName = supplier.StatusName
                });
            }

            return suppliers;
        }

        public ContactByAccountIdResponse GetContactsByAccountId(int accountId)
        {
            var contactsDb = _accountRepository.GetContactsList(accountId);
            var contacts = new List<Contacts>();
            foreach (var contact in contactsDb)
            {
               contacts.Add(new Contacts
                   {
                       AccountId = contact.AccountId,
                       ContactId = contact.ContactId,
                       AccountName = contact.AccountName,
                       FirstName = contact.FirstName,
                       LastName = contact.LastName,
                       OfficePhone = contact.OfficePhone,
                       Email = contact.Email,
                       IsActive = contact.IsActive
                   }
               );
            }
            return new ContactByAccountIdResponse {ContactsByAccountId = contacts};
        }

        public AccountsListResponse GetAllAccounts(AccountFilter filter)
        {
            var accounstDb = _accountRepository.GetAllAccounts(filter);
            var accounts = new List<AllAccounts>();
            foreach (var account in accounstDb)
            {
                accounts.Add(new AllAccounts
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                    AccountNum = account.AccountNum,
                    AccountType = account.AccountType,
                    CountryName = account.CountryName,
                    City = account.City,
                    Organization = account.Organization,
                    Owners = account.Owners,
                    AccountStatus = account.AccountStatus,
                    TotalContactCount = account.TotalContactCount,
                    AccountNameAndNum = account.AccountName + " (" + account.AccountNum + ")"
                });
            }
            return new AccountsListResponse{ AccountsList = accounts,TotalRowCount = accounstDb.Count >0 ? accounstDb.First().TotalRowCount:0};
        }

	public AccountsListResponse GetAccountsByNameNum(AccountsFilter filter)
        {
            var accounstDb = _accountRepository.GetAccountsByNameNum(filter);
            var accounts = new List<AllAccounts>();
            foreach (var account in accounstDb)
            {
                accounts.Add(new AllAccounts
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                    AccountNum = account.AccountNum,
                    AccountType = String.Join(", ", AccountTypes(account.AccountTypeIds).ToArray()),
                AccountNameAndNum = account.AccountName + " (" + account.AccountNum + ")"
                });
            }
            return new AccountsListResponse { AccountsList = accounts};
        }

        public AccountsListResponse SupplierLineCardMatch(SupplierLineCardMatchRequest supplierLineCardMatchRequest)
        {
            var accountsDb = _accountRepository.SupplierLineCardMatch(supplierLineCardMatchRequest.Commodities,
                supplierLineCardMatchRequest.Mfrs);
            var accounts = new List<AllAccounts>();
            foreach (var account in accountsDb)
            {
                accounts.Add(new AllAccounts
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                    AccountNum = account.AccountNum,
                    AccountNameAndNum = account.AccountName + " (" + account.AccountNum + ")",
                    ContactID = account.ContactID,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    Phone = account.OfficePhone,
                    Email = account.Email
                });
            }
            return new AccountsListResponse { AccountsList = accounts };
        }

        public AccountGroupListResponse AccountGroupListGet()
        {
            var accountGroupDbs = _accountRepository.AccountGroupListGet();
            var accountGroups = new List<AccountGroup>();
            foreach (var accountGroupDb in accountGroupDbs)
            {
                accountGroups.Add(new AccountGroup
                {
                    AccountGroupID = accountGroupDb.AccountGroupID,
                    GroupName = accountGroupDb.GroupName
                });
            }

            return new AccountGroupListResponse{AccountGroups = accountGroups};
        }

        public AccountGroupListResponse SuppliersAccountGroupListGet()
        {
            var accountGroupDbs = _accountRepository.SuppliersAccountGroupListGet();
            var accountGroups = new List<AccountGroup>();
            foreach (var accountGroupDb in accountGroupDbs)
            {
                accountGroups.Add(new AccountGroup
                {
                    AccountGroupID = accountGroupDb.AccountGroupID,
                    GroupName = accountGroupDb.GroupName
                });
            }

            return new AccountGroupListResponse { AccountGroups = accountGroups };
        }

        public BaseResponse UserAccountGroupDelete(int accountGroupId)
        {
            var deleteSuccess = _accountRepository.UserAccountGroupDelete(accountGroupId);
            return new BaseResponse
            {
                IsSuccess = deleteSuccess
            };
        }

        public List<string> AccountTypes(string accountTypeIds)
        {
            var accountType = new List<string>();
            if (accountTypeIds.Contains('4'))
            {
                accountType.Add("Customer");
            }

            if (accountTypeIds.Contains('1'))
            {
                accountType.Add("Supplier");
            }

            return accountType;
        }
        public AccountGroupDetailResponse AccountGroupDetailGet(int accountGroupId)
        {
            var accountGroupDetailResponse = new AccountGroupDetailResponse();
            var accountGroupLineDbs = _accountRepository.AccountGroupDetailGet(accountGroupId);
            if (accountGroupLineDbs.Count > 0)
            {
                var accountGroupLines = new List<AccountGroupLine>();
                foreach (var accountGroupDetailDb in accountGroupLineDbs)
                {
                    var accountType = new List<string>();
                    accountType = AccountTypes(accountGroupDetailDb.AccountTypeIds);
                    accountGroupLines.Add(new AccountGroupLine
                    {
                        AccountId = accountGroupDetailDb.AccountID,
                        AccountName = accountGroupDetailDb.AccountName,
                        AccountStatus = accountGroupDetailDb.AccountStatus,
                        AccountStatusId = accountGroupDetailDb.AccountStatusId,
                        AccountTypes = accountType,
                        ContactId = accountGroupDetailDb.ContactID,
                        ContactName = accountGroupDetailDb.ContactName,
                        GroupLineId = accountGroupDetailDb.GroupLineID
                    });

                    accountGroupDetailResponse.AccountGroupId = accountGroupLineDbs[0].AccountGroupID;
                    accountGroupDetailResponse.GroupName = accountGroupLineDbs[0].GroupName;
                    accountGroupDetailResponse.UserId = accountGroupLineDbs[0].UserID;
                    accountGroupDetailResponse.AccoutGroupLines = accountGroupLines;
                }
            }

            return accountGroupDetailResponse;
        }

        public AccountGroupDetailResponse SuppliersAccountGroupMatch(int accountGroupId)
        {
            var accountGroupDetailResponse = new AccountGroupDetailResponse();
            var accountGroupLineDbs = _accountRepository.SuppliersAccountGroupMatch(accountGroupId);
            if (accountGroupLineDbs.Count > 0)
            {
                var accountGroupLines = CreateAccountGroupObjects(accountGroupLineDbs);
                accountGroupDetailResponse.AccountGroupId = accountGroupLineDbs[0].AccountGroupID;
                accountGroupDetailResponse.GroupName = accountGroupLineDbs[0].GroupName;
                accountGroupDetailResponse.UserId = accountGroupLineDbs[0].UserID;
                accountGroupDetailResponse.AccoutGroupLines = accountGroupLines;
            }

            return accountGroupDetailResponse;
        }

        private List<AccountGroupLine> CreateAccountGroupObjects(IList<AccountGroupDetailDb> accountGroupLineDbs) {
            var accountGroupLines = new List<AccountGroupLine>();
            foreach (var accountGroupDetailDb in accountGroupLineDbs)
            {
                var accountType = new List<string>();
                if (accountGroupDetailDb.AccountTypeIds.Contains('4'))
                {
                    accountType.Add("Customer");
                    ;
                }

                if (accountGroupDetailDb.AccountTypeIds.Contains('1'))
                {
                    accountType.Add("Supplier");
                }
                accountGroupLines.Add(new AccountGroupLine
                {
                    AccountId = accountGroupDetailDb.AccountID,
                    AccountName = accountGroupDetailDb.AccountName,
                    AccountStatus = accountGroupDetailDb.AccountStatus,
                    AccountStatusId = accountGroupDetailDb.AccountStatusId,
                    AccountTypes = accountType,
                    ContactId = accountGroupDetailDb.ContactID,
                    ContactName = accountGroupDetailDb.ContactName,
                    FirstName = accountGroupDetailDb.FirstName,
                    LastName = accountGroupDetailDb.LastName,
                    Phone = accountGroupDetailDb.OfficePhone,
                    Email = accountGroupDetailDb.Email,
                    GroupLineId = accountGroupDetailDb.GroupLineID
                });
            }
            return accountGroupLines;
        }

        public AccountsListResponse GetAllAccountsByUserId(int userId)
        {
            var accounstDb = _accountRepository.GetAllAccountsByUserId(userId);
            var accounts = new List<AllAccounts>();
            foreach (var account in accounstDb)
            {
                accounts.Add(new AllAccounts
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName,
                    AccountNum = account.AccountNum,
                    AccountType = account.AccountType,
                    CountryName = account.CountryName,
                    City = account.City,
                    Organization = account.Organization,
                    Owners = account.Owners,
                    AccountStatus = account.AccountStatus,
                    TotalContactCount = account.TotalContactCount

                });
            }
            return new AccountsListResponse { AccountsList = accounts, TotalRowCount = accounstDb.Count > 0 ? accounstDb.First().TotalRowCount : 0 };
        }
        
        public List<AccountBasicDetails> GetSuppliers()
        {
            var suppliersDb = _accountRepository.GetSuppliersList();
            var suppliers = new List<AccountBasicDetails>();

            foreach (var supplier in suppliersDb)
            {
                suppliers.Add(new AccountBasicDetails {
                    AccountId = supplier.AccountId,
                    Name = supplier.AccountName
                });
            }

            return suppliers;
        }

        public IList<ContactJobFunction> ContactJobFunctionListGet()
        {
            var contactJobFunctionDbs = _accountRepository.ContactJobFunctionListGet();

            var contactJobFunctions = new List<ContactJobFunction>();

            foreach (var contactJobFunctionDb in contactJobFunctionDbs)
            {
                contactJobFunctions.Add(new ContactJobFunction
                {
                    JobFunctionID = contactJobFunctionDb.JobFunctionID,
                    JobFunctionName = contactJobFunctionDb.JobFunctionName
                });
            }

            return contactJobFunctions;
        }

        public ContactProjectsGetResponse ContactProjectsGet(int accountId, int contactId)
        {
            var contactProjectDbs = _accountRepository.ContactProjectListGet(accountId, contactId);

            var options = new List<ContactProjectOption>();
            var maps = new List<ContactProjectMap>();

            foreach (var contactProjectsDb in contactProjectDbs)
            {
                if (contactProjectsDb.IsOption)
                {
                    options.Add(new ContactProjectOption
                    {
                        ProjectID = contactProjectsDb.ProjectID,
                        Name = contactProjectsDb.Name
                    });
                }
                else
                {
                    maps.Add(new ContactProjectMap
                    {
                        ProjectID = contactProjectsDb.ProjectID,
                        Name = contactProjectsDb.Name
                    });
                }
            }
            return new ContactProjectsGetResponse
            {
                ContactProjectMaps = maps,
                ContactProjectOptions = options
            };
        }

        public BaseResponse ContactProjectsSet(SetContactProjectsRequest setContactProjectsRequest)
        {
            var rowCount = _accountRepository.ContactProjectListSet(setContactProjectsRequest);
            return new BaseResponse
            {
                IsSuccess = rowCount == 1
            };
        }

        public ContactFocusesGetResponse ContactFocusesGet(int accountId, int contactId)
        {
            var contactFocusDbs = _accountRepository.ContactFocusListGet(accountId, contactId);

            var options = new List<ContactFocusOption>();
            var maps = new List<ContactFocusMap>();

            foreach (var contactFocusesDb in contactFocusDbs)
            {
                if (contactFocusesDb.IsOption)
                {
                    options.Add(new ContactFocusOption
                    {
                        FocusID = contactFocusesDb.FocusID,
                        FocusName = contactFocusesDb.FocusName,
                        ObjectName = contactFocusesDb.ObjectName
                    });
                }
                else
                {
                    maps.Add(new ContactFocusMap
                    {
                        FocusID = contactFocusesDb.FocusID,
                        FocusName = contactFocusesDb.FocusName,
                        ObjectName = contactFocusesDb.ObjectName
                    });
                }
            }
            return new ContactFocusesGetResponse
            {
                ContactFocusMaps = maps,
                ContactFocusOptions = options
            };
        }

        public BaseResponse ContactFocusesSet(SetContactFocusesRequest setContactFocusesRequest)
        {
            var rowCount = _accountRepository.ContactFocusListSet(setContactFocusesRequest);
            return new BaseResponse
            {
                IsSuccess = rowCount == 1
            };
        }

        public AccountHierarchiesGetResponse AccountHierarchiesGet()
        {
            var accountHierarchyDbs = _accountRepository.AccountHierarchiesGet();
            var accountHierarchies = new List<AccountHierarchyResponse>();
            foreach (var accountHierarchyDb in accountHierarchyDbs)
            {
                accountHierarchies.Add(new AccountHierarchyResponse
                {
                    AccountHierarchyID = accountHierarchyDb.AccountHierarchyID,
                    HierarchyName = accountHierarchyDb.HierarchyName,
                    ParentID = accountHierarchyDb.ParentID,
                    RegionID = accountHierarchyDb.RegionID,
                    SAPGroupID = accountHierarchyDb.SAPGroupID,
                    SAPHierarchyID = accountHierarchyDb.SAPHierarchyID
                });
            }
            return new AccountHierarchiesGetResponse
            {
                AccountHierarchies = accountHierarchies
            };
        }

        public List<FocusObjectTypeResponse> AccountFocusObjectTypesGet()
        {
            var accountFocusObjectTypesDbs = _accountRepository.AccountFocusObjectTypesGet();
            var accountFocusObjectTypes = new List<FocusObjectTypeResponse>();
            foreach (var accountFocusTypeDb in accountFocusObjectTypesDbs)
            {
                accountFocusObjectTypes.Add(new FocusObjectTypeResponse
                {
                  
                    FocusObjectTypeId = accountFocusTypeDb.FocusObjectTypeId,
                    ObjectTypeId = accountFocusTypeDb.ObjectTypeId,
                    ObjectTypeName = accountFocusTypeDb.ObjectName
                });
            }
            return accountFocusObjectTypes;

        }
        public List<FocusTypeResponse> AccountFocusTypesGet()
        {
            var accountFocusTypesDbs = _accountRepository.AccountFocusTypesGet();
            var accountFocusTypes = new List<FocusTypeResponse>();
            foreach (var accountFocusTypeDb in accountFocusTypesDbs)
            {
                accountFocusTypes.Add(new FocusTypeResponse
                {

                    FocusTypeId = accountFocusTypeDb.FocusTypeId,
                    FocusName = accountFocusTypeDb.FocusName,
                    TypeRank = accountFocusTypeDb.TypeRank
                });
            }
            return accountFocusTypes;

        }
        public List<AccountFocusResponse> AccountFocusesGet(int accountId)
        {
            var accountFocusDbs = _accountRepository.AccountFocusesGet(accountId);
            var accountFocuses = new List<AccountFocusResponse>();
            foreach (var accountFocusDb in accountFocusDbs)
            {
                accountFocuses.Add(new AccountFocusResponse
                {
                    FocusId = accountFocusDb.FocusId,
                    AccountId = accountFocusDb.AccountId,
                    FocusTypeId = accountFocusDb.FocusTypeId,
                    ObjectTypeId = accountFocusDb.ObjectTypeId,
                    ObjectId = accountFocusDb.ObjectId,
                    CommodityId = accountFocusDb.CommodityId,
                    MfrId = accountFocusDb.MfrId,
                    ObjectName = accountFocusDb.ObjectName,
                    FocusName =  accountFocusDb.FocusName,
                    ObjectValue = accountFocusDb.ObjectValue,
                    CommodityName = accountFocusDb.CommodityName,
                    MfrName = accountFocusDb.MfrName,
                });
            }
            return accountFocuses;
        }

        public BaseResponse SetAccountFocus(SetAccountFocusRequest setAccountFocusRequest)
        {
            var rowCount = _accountRepository.SetAccountFocus(setAccountFocusRequest);
            return new BaseResponse
            {
                IsSuccess = rowCount == 1
            };
        }
        public BaseResponse DeleteAccountFocus(DeleteAccountFocusRequest deleteAccountFocusRequest)
        {
            var rowCount = _accountRepository.DeleteAccountFocus(deleteAccountFocusRequest);
            return new BaseResponse
            {
                IsSuccess = rowCount == 1
            };
        }

        public AccountTypesDataResponse AccountTypesDataGet(int accountId)
        {
            var typesDb = _accountRepository.AccountTypesDataGet(accountId);
            var typesData = new List<AccountTypesData>();

            foreach(var type in typesDb)
            {
                typesData.Add(new AccountTypesData
                {
                    AccountTypeID = type.AccountTypeID,
                    AccountStatusID = type.AccountStatusID,
                    StatusName = type.StatusName,
                    PaymentTermID = type.PaymentTermID,
                    PaymentTermName = type.PaymentTermName,
                    EPDSID = type.EPDSID
                });
            }

            return new AccountTypesDataResponse { Types = typesData };
        }

        public AccountGroupDetailResponse AccountGroupSet(AccountGroupSetRequest accountGroupSetRequest)
        {
            var accountGroupID = _accountRepository.AccountGroupSet(accountGroupSetRequest);
            var response = new AccountGroupDetailResponse
            {
                AccountGroupId = accountGroupID
            };
            return response;
        }

        public void AccountSapDataSet(SetAccountExternalIdsRequest account)
        {
            try
            {
                var accountDetails = new AccountBasicDetails();

                if (account.AccountDetails.NonStockSupplier && account.AccountDetails.CustomerDetails == null)
                {
                    return;
                }

                //hierarchy
                int parentId = 0;
                if (account.AccountDetails.Hierarchy != null)
                {
                    var hierarchies = _accountRepository.AccountHierarchiesGetByExternal(account.AccountDetails.Hierarchy.HierarchyId);
                   
                    if (hierarchies != null && hierarchies.Count > 0)
                    {
                        parentId = hierarchies.First().AccountHierarchyID;
                    }
                    else
                    {
                        parentId = _accountRepository.AccountHierarchyParentGetIdByChild(account.AccountDetails.Hierarchy.Id);

                        //safety measure in case the hierarchy id is the parent and not the child
                        if(parentId == 0)
                            parentId = account.AccountDetails.Hierarchy.Id;
                    }

                    var accountHierarchyChildSapResponses = account.AccountDetails.Hierarchy.Children;
                    var parentGroupId = account.AccountDetails.Hierarchy.ParentGroupId;

                    var hierarchyDb = new AccountHierarchySapDetails();
                    hierarchyDb.RegionId = 0;
                    hierarchyDb.HierarchyId = parentId;
                    hierarchyDb.ExternalHierarchyId = account.AccountDetails.Hierarchy.HierarchyId;
                    hierarchyDb.GroupId = parentGroupId;
                    hierarchyDb.HierarchyName = account.AccountDetails.Hierarchy.ParentName;
                    parentId = _accountRepository.HierarchySapDataSet(hierarchyDb);

                    var childHierarchies = accountHierarchyChildSapResponses.Where(x => x.ChildId != parentGroupId);
                    var accountHierarchyChildrenDb = _accountRepository.AccountHierarchyChildrenFromParentGet(parentId);

                    foreach (var child in childHierarchies)
                    {

                        var accountHierarchyChildDb = new AccountHierarchyDb();
                        if (hierarchies != null && hierarchies.Count > 0)
                            accountHierarchyChildDb = hierarchies.FirstOrDefault(x => x.SAPGroupID == child.ChildId);
                        else
                            accountHierarchyChildDb = accountHierarchyChildrenDb.FirstOrDefault(x => x.HierarchyName == child.childGroupName);

                        hierarchyDb = new AccountHierarchySapDetails();
                        hierarchyDb.HierarchyId = accountHierarchyChildDb != null ? accountHierarchyChildDb.AccountHierarchyID : 0;
                        hierarchyDb.GroupId = child.ChildId;
                        hierarchyDb.ExternalHierarchyId = account.AccountDetails.Hierarchy.HierarchyId;
                        hierarchyDb.ParentId = parentId;
                        hierarchyDb.HierarchyName = child.childGroupName;
                        hierarchyDb.RegionId = GetRegionId(child.childGroupName);
                        int hierarchyDbId = _accountRepository.HierarchySapDataSet(hierarchyDb);

                        if(child.AssignedToAccount)
                        {
                            accountDetails.AccountHierarchyId = hierarchyDbId;
                        }
                    }

                }
                
                //account details
                if (account.ObjectId < 1)
                {
                    accountDetails.AccountId = _accountRepository.GetAccountIdByExternal(account.ExternalId);
                    account.ObjectId = accountDetails.AccountId;
                }
                else
                    accountDetails.AccountId = account.ObjectId;

                accountDetails.ApprovedVendor = account.AccountDetails.ApprovedVendor;
                accountDetails.ExternalId = account.ExternalId;
                accountDetails.Number = account.ExternalId;
                accountDetails.StatusExternalId = account.AccountDetails.Status;
                accountDetails.Name = account.AccountDetails.Name;
                accountDetails.Website = account.AccountDetails.Website;
                accountDetails.Email = account.AccountDetails.Email;
                accountDetails.VendorNum = account.AccountDetails.VendorNum;
                accountDetails.DBNum = account.AccountDetails.DBNum;

                if(!string.IsNullOrEmpty(account.AccountDetails.CompanyType))
                    accountDetails.CompanyTypeId = GetAllCompanyTypes().First(x => x.ExternalId == account.AccountDetails.CompanyType).Id;

                //set hierarchy if we have one from when we created it earlier
                if (account.AccountDetails.Hierarchy != null)
                {
                    //if (account.AccountDetails.Hierarchy.Id > 0)
                    //    accountDetails.AccountHierarchyId = account.AccountDetails.Hierarchy.Id;

                }

                List<int> accountType = new List<int>();
                if (account.AccountDetails.CustomerDetails != null)
                    accountType.Add(4);
                if (account.AccountDetails.SupplierDetails != null && !account.AccountDetails.NonStockSupplier)
                    accountType.Add(1);
                accountDetails.AccountTypeIds = accountType;

                var setAccount = SetAccountBasicDetails(accountDetails);

                if (account.ObjectId < 1)
                    account.ObjectId = setAccount.AccountId;

                //set from sap specific details for customer and supplier specific data
                var customerDetails = SetAccountTypeDetails(account.AccountDetails.CustomerDetails);
                var supplierDetails = account.AccountDetails.NonStockSupplier ? null : SetAccountTypeDetails(account.AccountDetails.SupplierDetails);
                var status = _commonDataRepository.GetStatusIdByExternal(account.AccountDetails.Status);
                _accountRepository.AccountSapDataSet(account, customerDetails, supplierDetails, status);


                //locations
                var locationExternalIds = new List<string>();
                foreach (var sapLocation in account.Locations)
                {
                    var location = new Location();

                    location.AccountId = setAccount.AccountId;
                    locationExternalIds.Add(sapLocation.ExternalId);

                    if (sapLocation.LocationId < 1)
                        location.LocationId = _accountRepository.GetLocationIdByExternal(sapLocation.ExternalId);
                    else
                        location.LocationId = sapLocation.LocationId;

                    if(location.LocationId < 1)
                    {
                        if (!string.IsNullOrEmpty(sapLocation.Street))
                        {
                            location.Name = sapLocation.Street;
                        }
                        else if (!string.IsNullOrEmpty(sapLocation.City))
                        {
                            location.Name = sapLocation.City;
                        }
                        else 
                        {
                            location.Name = sapLocation.CountryExternalId;
                        }
                    }

                    if (sapLocation.LocationTypeExternalId != null)
                    {
                        if (sapLocation.LocationTypeExternalId.Contains("BILL_TO"))
                        {
                            location.TypeId += 1;
                        }

                        if (sapLocation.LocationTypeExternalId.Contains("SHIP_TO"))
                        {
                            location.TypeId += 2;
                        }
                    }
                    else
                    {
                        location.TypeId += 2;
                    }

                    location.CountryId = _commonDataRepository.GetCountryByExternal(sapLocation.CountryExternalId);
                    location.StateId = _commonDataRepository.GetStateByExternal(sapLocation.StateExternalId);

                    location.ExternalId = sapLocation.ExternalId;
                    location.AddressLine1 = sapLocation.Address1;
                    location.AddressLine2 = sapLocation.Address2;
                    location.HouseNo = sapLocation.HouseNo;
                    location.Street = sapLocation.Street;
                    location.AddressLine4 = sapLocation.Address4;
                    location.City = sapLocation.City;
                    location.PostalCode = sapLocation.PostalCode;
                    location.District = sapLocation.District;

                    var setLoc = SaveAccountLocation(location);
                }

                //get list of account locations in our db
                var locations = _accountRepository.GetAccountLocations(setAccount.AccountId);
                var deletedLocations = locations.Where(x => x.ExternalID != null && !locationExternalIds.Contains(x.ExternalID)).ToList();
                foreach(var location in deletedLocations)
                {
                    var deleteLoc = CreateLocationFromDbLocation(location);
                    var deleted = DeleteAccountLocation(deleteLoc);
                }


                //contacts
                var contactExternalIds = new List<string>();
                foreach (var sapContact in account.Contacts)
                {
                    var contact = new SetContactDetailsRequest();
                    
                    contact.AccountId = setAccount.AccountId;
                    contactExternalIds.Add(sapContact.ExternalId);

                    if (sapContact.ContactId < 1)
                        contact.ContactId = _accountRepository.GetContactIdByExternal(sapContact.ExternalId);
                    else
                        contact.ContactId = sapContact.ContactId;

                    contact.ExternalId = sapContact.ExternalId;
                    contact.FirstName = sapContact.FirstName;
                    contact.LastName = sapContact.LastName;
                    contact.OfficePhone = sapContact.OfficePhone;
                    contact.Email = sapContact.Email;
                    contact.IsActive = 1;
                    contact.JobFunctionID = sapContact.JobFunctionExternalId != null ? (int?)_accountRepository.GetJobFunctionIdByExternal(sapContact.JobFunctionExternalId) : null;

                    var setContact = SetContactDetails(contact);
                }

                var contacts = _accountRepository.GetContactsList(setAccount.AccountId);
                var deletedContacts = contacts.Where(x => x.ExternalID != null && !contactExternalIds.Contains(x.ExternalID)).ToList();
                foreach(var contact in deletedContacts)
                {
                    var deleteContactRequest = new SetContactDetailsRequest();
                    deleteContactRequest.ContactId = contact.ContactId;
                    deleteContactRequest.AccountId = contact.AccountId;
                    deleteContactRequest.ExternalId = contact.ExternalID;
                    deleteContactRequest.IsDeleted = true;
                    _accountRepository.DeleteContact(deleteContactRequest);
                }

            }
            catch (Exception e)
            {
                var error = e.Message;
            }
        }


        private int GetRegionId(string name)
        {
            if (name == RegionsEnum.Americas.GetEnumDescription())
                return (int)RegionsEnum.Americas;
            else if (name == RegionsEnum.EMEA.GetEnumDescription())
                return (int)RegionsEnum.EMEA;
            else if (name == RegionsEnum.APAC.GetEnumDescription())
                return (int)RegionsEnum.APAC;
            else
                return 0;
        }

        private AccountTypeDetails SetAccountTypeDetails(AccountTypeDetailsSap details)
        {
            var mappedDetails = new AccountTypeDetails();
            if (details != null)
            {
                mappedDetails.CurrencyID = details.CurrencyID != null ? _commonDataRepository.GetCurrencyIdByExternal(details.CurrencyID) : null;
                mappedDetails.PaymentTermID = details.PaymentTermID != null ? _commonDataRepository.GetPaymentTermIdByExternal(details.PaymentTermID) : 0;
                mappedDetails.CreditLimit = details.CreditLimit;
                mappedDetails.EpdsId = details.EpdsId;
            }
            return mappedDetails;
        }

        public SyncResponse Sync(int accountId)
        {
            var syncRequest = _accountSyncRequestCreator.Create(accountId);
            return _middlewareService.Sync(syncRequest);
        }

    }
}