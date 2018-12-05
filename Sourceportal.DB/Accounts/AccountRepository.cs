using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.Accounts;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using Sourceportal.Domain.Models.DB.Accounts;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Domain.Models.Shared;
using Sourceportal.Utilities;

namespace Sourceportal.DB.Accounts
{
    public class AccountRepository : IAccountRepository
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;
        
        public IList<ContactListContactDb> GetContactsList(ContactsFilter filter)
        {
        
            IList<ContactListContactDb> contactDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                if (filter != null)
                {
                    param.Add("@SearchText", filter.FreeTextSearch);
                    param.Add("@AccountId", filter.AccountId);
                    param.Add("@RowOffset", filter.RowOffset);
                    param.Add("@RowLimit", filter.RowLimit);
                    param.Add("@DescSort", filter.DescSort);
                    param.Add("@SortBy", filter.SortBy);
                    param.Add("@FilterBy", filter.FilterBy);
                    param.Add("@FilterText", filter.FilterText);
                    param.Add("@AccountTypeId", filter.AccountTypeId);
                    param.Add("@AccountIsActive", filter.AccountIsActive);
                    param.Add("@UserID", UserHelper.GetUserId());
                }
                contactDbs = con.Query<ContactListContactDb>("uspContactsGetByAccountType", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return contactDbs;
        }

        public AccountBasicDetailsDb GetAccountBasicDetails(int accountId)
        {
            AccountBasicDetailsDb accountbasicDetailsDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@accountId", accountId);
                param.Add("@UserID", UserHelper.GetUserId());

                accountbasicDetailsDbs =
                    con.Query<AccountBasicDetailsDb>("uspAccountsGet", param,
                        commandType: CommandType.StoredProcedure).First();
                con.Close();
                
            }
            return accountbasicDetailsDbs;
        }

        public AccountBasicDetailsDb DeleteAccount(AccountBasicDetails accountBasicDetails)
        {
            AccountBasicDetailsDb savedRecord;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@accountId", accountBasicDetails.AccountId);
                param.Add("@AccountNum", accountBasicDetails.Number);
                param.Add("@AccountName", accountBasicDetails.Name);
                param.Add("@AccountTypeBitwise", accountBasicDetails.AccountTypeIds.Sum());
                param.Add("@CreatedBy", UserHelper.GetUserId());
                param.Add("@ExternalId", accountBasicDetails.ExternalId);
                param.Add("@Email", accountBasicDetails.Email);
                param.Add("@Website", accountBasicDetails.Website);
                param.Add("@YearEstablished", accountBasicDetails.YearEstablished);
                param.Add("@EndProductFocus", accountBasicDetails.EndProductFocus);
                param.Add("@CarryStock", accountBasicDetails.CarryStock);
                param.Add("@MinimumPO", accountBasicDetails.MinimumPO);
                param.Add("@ShippingInstructions", accountBasicDetails.ShippingInstructions);
                param.Add("@VendorNum", accountBasicDetails.VendorNum);
                param.Add("@SupplierRating", accountBasicDetails.SupplierRating);
                param.Add("@QCNotes", accountBasicDetails.QCNotes);
                param.Add("@PONotes", accountBasicDetails.PONotes);
                param.Add("@IsDeleted", accountBasicDetails.IsDeleted);
                var res = con.Query<AccountBasicDetailsDb>("uspAccountSet", param, commandType: CommandType.StoredProcedure);
                savedRecord = res.First();
                con.Close();
            }
            return savedRecord;
        }

        public AccountBasicDetailsDb SaveBasicDetails(AccountBasicDetails accountBasicDetails)
        {
            AccountBasicDetailsDb savedRecord;
            using (var con = new SqlConnection(ConnectionString))
            {
                
                int status = 0;
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@accountId", accountBasicDetails.AccountId);
                param.Add("@AccountNum", accountBasicDetails.Number);
                param.Add("@AccountName", accountBasicDetails.Name);
                param.Add("@AccountTypeBitwise", accountBasicDetails.AccountTypeIds.Sum());
                param.Add("@ret", status, direction: ParameterDirection.ReturnValue);
                param.Add("@CompanyTypeID", accountBasicDetails.CompanyTypeId);
                param.Add("@CreatedBy", UserHelper.GetUserId());
                //param.Add("@CurrencyID", accountBasicDetails.CurrencyId);
                param.Add("@ExternalId", accountBasicDetails.ExternalId);
                param.Add("@OrganizationID", accountBasicDetails.OrganizationId);
                param.Add("@AccountHierarchyID", accountBasicDetails.AccountHierarchyId);
                param.Add("@Email", accountBasicDetails.Email);
                param.Add("@Website", accountBasicDetails.Website);
                param.Add("@YearEstablished", accountBasicDetails.YearEstablished);
                param.Add("@NumOfEmployees", accountBasicDetails.NumOfEmployees);
                param.Add("@EndProductFocus", accountBasicDetails.EndProductFocus);
                param.Add("@CarryStock", accountBasicDetails.CarryStock);
                param.Add("@MinimumPO", accountBasicDetails.MinimumPO);
                param.Add("@ShippingInstructions", accountBasicDetails.ShippingInstructions);
                param.Add("@VendorNum", accountBasicDetails.VendorNum);
                param.Add("@SupplierRating", accountBasicDetails.SupplierRating);
                param.Add("@QCNotes", accountBasicDetails.QCNotes);
                param.Add("@PONotes", accountBasicDetails.PONotes);
                param.Add("@ApprovedVendor", accountBasicDetails.ApprovedVendor);
                param.Add("@IncotermID", accountBasicDetails.IncotermID);
                param.Add("@DBNum", accountBasicDetails.DBNum);

                var res =  con.Query<AccountBasicDetailsDb>("uspAccountSet", param, commandType: CommandType.StoredProcedure);

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", AccountDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                savedRecord = res.First();
                con.Close();
            }
            return savedRecord;

        }
        public IList<LocationDb> GetLocations()
        {
            IList<LocationDb> locationDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                locationDbs = con.Query<LocationDb>("usplocationsget", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return locationDbs;
        }

        public IList<LocationDb> GetAccountLocations(int accountId)
        {
            IList<LocationDb> locationDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@accountId", accountId);

                locationDbs = con.Query<LocationDb>("usplocationsget", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return locationDbs;
        }

        public LocationDb SaveLocation(Location location)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                LocationDb locationDb;
                int status = 0;
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@LocationID", location.LocationId);
                param.Add("@AccountID", location.AccountId);
                if (location.shipToChecked)
                {
                    param.Add("@LocationTypeID", (int)Enum.LocationType.BillTo + (int)Enum.LocationType.ShipTo);
                }
                else
                {
                    param.Add("@LocationTypeID", location.TypeId);
                }
                param.Add("@Name", location.Name);
                param.Add("@Address1", location.AddressLine1);
                param.Add("@Address2", location.AddressLine2);
                param.Add("@HouseNumber", location.HouseNo);
                param.Add("@Street", location.Street);
                param.Add("@Address4", location.AddressLine4);
                param.Add("@City", location.City);
                param.Add("@StateID", location.StateId);
                param.Add("@PostalCode", location.PostalCode);
                param.Add("@District", location.District);
                param.Add("@CountryID", location.CountryId);
                param.Add("@Note", location.Note);
                param.Add("@ExternalID", location.ExternalId);
                param.Add("@CreatedBy", UserHelper.GetUserId());
                param.Add("@IsDeleted", location.IsDeleted);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                locationDb = con.Query<LocationDb>("uspLocationSet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();

                var errorId = param.Get<int>("@ret");

                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", AccountDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                return locationDb;
            }
        }

        public IList<CountryDb> GetCountryList()
        {
            IList<CountryDb> countryDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                
                countryDbs = con.Query<CountryDb>("uspCountriesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return countryDbs;
        }

        public IList<StateDb> GetStateList(int countryId)
        {
            IList<StateDb> stateDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@CountryId", countryId);

                stateDbs = con.Query<StateDb>("uspStatesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return stateDbs;
        }

      

        public List<OwnerDb> GetOwners(int contactId)
        {
            List<OwnerDb> ownerDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ObjectId", contactId);
                param.Add("@ObjectTypeId", ObjectType.Contact);

                ownerDbs = con.Query<OwnerDb>("uspOwnershipGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return ownerDbs;
        }

        public ContactDetailsDb DeleteContact(SetContactDetailsRequest setContactDetailsRequest)
        {
            ContactDetailsDb contactDetailsDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ContactId", setContactDetailsRequest.ContactId);
                param.Add("@AccountID", setContactDetailsRequest.AccountId);
                param.Add("@ExternalId", setContactDetailsRequest.ExternalId);
                param.Add("@IsDeleted", setContactDetailsRequest.IsDeleted);
                param.Add("@CreatedBy", UserHelper.GetUserId());

                contactDetailsDb = con.Query<ContactDetailsDb>("uspContactSet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();
            }

            return contactDetailsDb;
        }

        public ContactDetailsDb SetContactDetails(SetContactDetailsRequest setContactDetailsRequest)
        {
            ContactDetailsDb contactDetailsDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ContactId", setContactDetailsRequest.ContactId);
                param.Add("@AccountID", setContactDetailsRequest.AccountId);
                param.Add("@FirstName", setContactDetailsRequest.FirstName);
                param.Add("@LastName", setContactDetailsRequest.LastName);
                param.Add("@OfficePhone", setContactDetailsRequest.OfficePhone);
                param.Add("@MobilePhone", setContactDetailsRequest.MobilePhone);
                param.Add("@Email", setContactDetailsRequest.Email);
                param.Add("@Fax", setContactDetailsRequest.Fax);
                param.Add("@IsActive", setContactDetailsRequest.IsActive);
                param.Add("@Title", setContactDetailsRequest.Title);
                param.Add("@Details", setContactDetailsRequest.Note);
                param.Add("@PreferredContactMethodId", setContactDetailsRequest.PreferredContactMethodId);
                param.Add("@LocationId", setContactDetailsRequest.LocationId);
                param.Add("@ExternalId", setContactDetailsRequest.ExternalId);
                param.Add("@Department", setContactDetailsRequest.Department);
                param.Add("@JobFunctionID", setContactDetailsRequest.JobFunctionID);
                param.Add("@Birthdate", setContactDetailsRequest.Birthdate);
                param.Add("@Gender", setContactDetailsRequest.Gender);
                param.Add("@Salutation", setContactDetailsRequest.Salutation);
                param.Add("@MaritalStatus", setContactDetailsRequest.MaritalStatus);
                param.Add("@KidsNames", setContactDetailsRequest.KidsNames);
                param.Add("@ReportsTo", setContactDetailsRequest.ReportsTo);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                param.Add("@CreatedBy", UserHelper.GetUserId());

                var res = con.Query<ContactDetailsDb>("uspContactSet", param, commandType: CommandType.StoredProcedure);
                con.Close();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", AccountDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                contactDetailsDb = res.First();

                //Update ownership for non new created contact-- Original comment
                //Nathan's comments - Ownership isn't coming through the API, so we cannot update ownership - 4/27
                if (setContactDetailsRequest.ContactId != 0 && setContactDetailsRequest.OwnerList != null && setContactDetailsRequest.OwnerList.Count > 0)
                {
                    SetAccountOwnership(new SetContactOwnershipRequest()
                    {
                        ContactId = setContactDetailsRequest.ContactId,
                        OwnerList = setContactDetailsRequest.OwnerList

                    });
                }

            }

            return contactDetailsDb;
        }

        public ContactDetailsDb CreateContactWithUserOwnership(SetContactDetailsRequest setContactDetailsRequest)
        {
            ContactDetailsDb contactDetailsDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ContactId", setContactDetailsRequest.ContactId);
                param.Add("@AccountID", setContactDetailsRequest.AccountId);
                param.Add("@FirstName", setContactDetailsRequest.FirstName);
                param.Add("@LastName", setContactDetailsRequest.LastName);
                param.Add("@OfficePhone", setContactDetailsRequest.OfficePhone);
                param.Add("@MobilePhone", setContactDetailsRequest.MobilePhone);
                param.Add("@Email", setContactDetailsRequest.Email);
                param.Add("@Fax", setContactDetailsRequest.Fax);
                param.Add("@IsActive", 1);
                param.Add("@Title", setContactDetailsRequest.Title);
                param.Add("@Details", setContactDetailsRequest.Note);
                param.Add("@PreferredContactMethodId", setContactDetailsRequest.PreferredContactMethodId);
                param.Add("@LocationId", setContactDetailsRequest.LocationId);
                param.Add("@ExternalId", setContactDetailsRequest.ExternalId);
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<ContactDetailsDb>("uspContactSet", param, commandType: CommandType.StoredProcedure);
                con.Close();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", AccountDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }

                contactDetailsDb = res.First();

                SetContactUserOwnership(new SetContactOwnershipRequest()
                {
                    ContactId = contactDetailsDb.ContactId,
                    OwnerList = setContactDetailsRequest.OwnerList

                });

            }

            return contactDetailsDb;
        }

        public IList<OwnerDb> SetContactUserOwnership(SetContactOwnershipRequest ownershipRequest)
        {
            IList<OwnerDb> ownerDbs;
            var owners = new List<OwnersRequest>();
            owners.Add(new OwnersRequest { userId = UserHelper.GetUserId(), percentage = 100 });


            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ObjectID", ownershipRequest.ContactId);
                param.Add("@ObjectTypeID", (int)ObjectType.Contact);
                param.Add("@OwnerList", JsonConvert.SerializeObject(owners));
                param.Add("@CreatedBy", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var response = con.Query("uspOwnershipSet", param, commandType: CommandType.StoredProcedure);
                con.Close();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = "Database error occured: ownership set failed.";
                    throw new GlobalApiException(errorMessage);
                }

                ownerDbs = response.Select(x =>
                {
                    return new OwnerDb
                    {
                        OwnerFirstName = x.OwnerFirstName,
                        OwnerId = x.OwnerId,
                        OwnerLastName = x.OwnerLastName,
                        Percent = Decimal.Parse(x.Percent.ToString()),
                        ExternalID = x.ExternalID
                    };
                }).ToList();

                return ownerDbs;
            }
        }

        public IList<OwnerDb> SetAccountOwnership(SetContactOwnershipRequest ownershipRequest)
        {
            IList<OwnerDb> ownerDbs;
            var ownerList  = ownershipRequest.OwnerList.Select(x => new { userId = x.UserId, percentage = x.Percentage }).ToList();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ObjectID", ownershipRequest.ContactId );
                param.Add("@ObjectTypeID", (int)ObjectType.Contact);

                param.Add("@OwnerList", JsonConvert.SerializeObject(ownerList));
                param.Add("@CreatedBy", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var response = con.Query("uspOwnershipSet", param, commandType: CommandType.StoredProcedure);
                con.Close();

                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = "Database error occured: ownership set failed.";
                    throw new GlobalApiException(errorMessage);
                }
                
                ownerDbs = response.Select(x =>
                {
                    return new OwnerDb
                    {
                        OwnerFirstName = x.OwnerFirstName,
                        OwnerId = x.OwnerId,
                        OwnerLastName = x.OwnerLastName,
                        Percent = Decimal.Parse(x.Percent.ToString()),
                        ExternalID = x.ExternalID
                    };
                }).ToList();

                return ownerDbs;
            }
        }

        public IList<LocationDb> GetAccountShippingAddresses(int accountId)
        {
            List<LocationDb> locationDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@LocationTypeID", (int)Sourceportal.DB.Enum.LocationType.ShipTo);
                locationDbs = con.Query<LocationDb>("uspLocationsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return locationDbs;
        }

        public LocationDb GetAccountBillingAddress(int accountId)
        {
            LocationDb locationDb = null;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@AccountID", accountId);
                param.Add("@LocationTypeID", (int)Sourceportal.DB.Enum.LocationType.BillTo);

                var result= con.Query<LocationDb>("uspLocationsGet", param, commandType: CommandType.StoredProcedure);
                con.Close();

                if (result != null)
                {
                    if (result.Any())
                    {
                       return result.First() as LocationDb;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var errorMessage = $"Database error occured: Billing address for account id: {accountId}: not found.";
                    throw new GlobalApiException(errorMessage);
                }
            }
        }

        public IList<LocationDb> GetAccountNonBillingLocations(int accountId)
        {
            IList<LocationDb> locationDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@AccountID", accountId);

                locationDb = con.Query<LocationDb>("uspLocationsNonBillingGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return locationDb;
        }

        public IList<LocationDb> GetShipToLocations(int isDropShip)
        {
            IList<LocationDb> locationDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@IsDropShip", isDropShip);

                locationDb = con.Query<LocationDb>("uspShipToLocationsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return locationDb;
        }

        public LocationDb GetLocationDetails(int locationId)
        {
            LocationDb locationDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@locationId", locationId);

                var res = con.Query<LocationDb>("uspLocationsGet", param, commandType: CommandType.StoredProcedure);
                if (!res.Any())
                {
                    var errorMessage = $"Database error occured: Location id: {locationId}: not found.";
                    throw new GlobalApiException(errorMessage);
                }

                locationDb = res.First();
                con.Close();
            }

            return locationDb;
        }

        public ContactDetailsDb GetContactDetails(int contactId)
        {
            ContactDetailsDb contactDetailsDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@contactId", contactId);
                param.Add("@UserID", UserHelper.GetUserId());
                
                var res = con.Query<ContactDetailsDb>("uspContactsGet", param, commandType: CommandType.StoredProcedure);

                if (!res.Any())
                {
                    var errorMessage = $"Database error occured: Contact id: {contactId}: not found.";
                    throw new GlobalApiException(errorMessage);
                }

                contactDetailsDb = res.First();
                con.Close();
            }

            return contactDetailsDb;
        }

        public List<AccountBasicDetailsDb> GetSuppliersAndVendorsList(int? selectedAccountId)
        {
            List<AccountBasicDetailsDb> supplierDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountTypeID", 3);

                supplierDbs = con.Query<AccountBasicDetailsDb>("uspAccountsByTypeGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return supplierDbs;
        }

        public List<AccountBasicDetailsDb> GetSuppliersList()
        {
            List<AccountBasicDetailsDb> supplierDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountTypeID", 1);

                supplierDbs = con.Query<AccountBasicDetailsDb>("uspAccountsByTypeGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return supplierDbs;
        }

        public List<AccountStatusDb> GetAccountStatusDbs()
        {
            List<AccountStatusDb> supplierDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                
                supplierDbs = con.Query<AccountStatusDb>("uspAccountStatusesAllGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return supplierDbs;
        }

        public List<AccountTypeDb> GetAccountTypesDbs()
        {
            List<AccountTypeDb> accountTypesDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                accountTypesDb = con.Query<AccountTypeDb>("uspAccountTypesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return accountTypesDb;
        }

        public IList<ContactsByAccountIdDb> GetContactsList(int accountId)
        {
            IList<ContactsByAccountIdDb> contactsDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@UserID", UserHelper.GetUserId());
                contactsDbs =
                    con.Query<ContactsByAccountIdDb>("uspContactsByAccountIdGet", param, commandType: CommandType.StoredProcedure)
                        .ToList();
                con.Close();
            }
            return contactsDbs;
        }

        public IList<AccountsDb> GetAllAccounts(AccountFilter filter)
        {
            IList<AccountsDb> accountsDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@RowOffset", filter.RowOffset);
                param.Add("@RowLimit", filter.RowLimit);
                param.Add("@SortBy", filter.SortBy);
                param.Add("@DescSort", filter.DescSort);
                param.Add("@SearchString", filter.SearchString);
                param.Add("@AccountTypeId", filter.AccountTypeId);
                param.Add("@FilterBy", filter.FilterBy);
                param.Add("@FilterText", filter.FilterText);
                param.Add("@AccountIsActive", filter.AccountIsActive);
     
                param.Add("@UserID", UserHelper.GetUserId());

                accountsDbs = con.Query<AccountsDb>("uspAccountsGetByType", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountsDbs;
        }

        public IList<AccountsDb> GetAllAccountsByUserId(int UserId)
        {
            IList<AccountsDb> accountsDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountID", null);
                param.Add("@RowOffset", 0);
                param.Add("@RowLimit", 50);
                param.Add("@SortBy", "AccountName");
                param.Add("@DescSort", true);
                param.Add("@SearchString", "");
                param.Add("@UserID", UserId);

                accountsDbs = con.Query<AccountsDb>("uspAccountsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountsDbs;
        }

        public IList<AccountsDb> GetAccountsByNameNum(AccountsFilter filter)
        {
            IList<AccountsDb> accountsDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SearchString", filter.SearchString);
                if(filter.AccountType != null)
                    param.Add("@AccountTypeID", filter.AccountType);
                if (filter.ObjectTypeId != null)
                    param.Add("@ObjectTypeID", filter.ObjectTypeId);
                accountsDbs = con.Query<AccountsDb>("uspAccountSearchByNameNumGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountsDbs;
        }

        public IList<AccountLocationTypeDb> GetAccountLocationTypeDbs()
        {
            List<AccountLocationTypeDb> locationTypes;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                locationTypes = con.Query<AccountLocationTypeDb>("uspAccountLocationTypesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return locationTypes;
        }

        public IList<ContactJobFunctionDb> ContactJobFunctionListGet()
        {
            List<ContactJobFunctionDb> contactJobFunctionDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                contactJobFunctionDbs = con.Query<ContactJobFunctionDb>("uspContactJobFunctionsGet",
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return contactJobFunctionDbs;
        }

        public IList<ContactProjectsDb> ContactProjectListGet(int accountId, int contactId)
        {
            List<ContactProjectsDb> contactProjectDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@ContactID", contactId);
                contactProjectDbs = con
                    .Query<ContactProjectsDb>("uspContactProjectsGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return contactProjectDbs;
        }

        public int ContactProjectListSet(SetContactProjectsRequest setContactProjectsRequest)
        {
            int RowCount;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ProjectID", setContactProjectsRequest.ProjectID);
                param.Add("@ContactID", setContactProjectsRequest.ContactID);
                param.Add("@IsDeleted", setContactProjectsRequest.IsDeleted);
                RowCount = con
                    .Query<int>("uspContactProjectsSet", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return RowCount;
        }

        public IList<ContactFocusesDb> ContactFocusListGet(int accountId, int contactId)
        {
            List<ContactFocusesDb> contactFocusesDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                param.Add("@ContactID", contactId);
                contactFocusesDbs = con
                    .Query<ContactFocusesDb>("uspContactFocusesGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return contactFocusesDbs;
        }

        public int ContactFocusListSet(SetContactFocusesRequest setContactFocusesRequest)
        {
            int RowCount;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@FocusID", setContactFocusesRequest.FocusID);
                param.Add("@ContactID", setContactFocusesRequest.ContactID);
                param.Add("@IsDeleted", setContactFocusesRequest.IsDeleted);
                RowCount = con
                    .Query<int>("uspContactFocusesSet", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return RowCount;
        }

        public List<AccountHierarchyDb> AccountHierarchiesGet()
        {
            return AccountHierarchiesGet(null);
        }

        public List<AccountHierarchyDb> AccountHierarchiesGet(int? hierarchyId)
        {
            List<AccountHierarchyDb> accountHierarchyDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountHierarchyID", hierarchyId);
                accountHierarchyDbs = con
                    .Query<AccountHierarchyDb>("uspAccountHierarchiesGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return accountHierarchyDbs;
        }

        public List<AccountHierarchyDb> AccountHierarchyChildrenFromParentGet(int hierarchyId)
        {
            List<AccountHierarchyDb> accountHierarchyDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ParentID", hierarchyId);
                accountHierarchyDbs = con
                    .Query<AccountHierarchyDb>("SELECT " +
                    "AccountHierarchyID, " +
                    "ParentID, " +
                    "HierarchyName, " +
                    "SAPHierarchyID, " +
                    "SAPGroupID " +
                    "FROM AccountHierarchies WHERE ParentID=@ParentID ", param, commandType: null)
                    .ToList();
                con.Close();
            }
            return accountHierarchyDbs;
        }

        public int AccountHierarchyParentGetIdByChild(int childId)
        {
            int parentId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountHierarchyID", childId);
                parentId = con
                    .Query<int>("Select ParentID From AccountHierarchies Where AccountHierarchyID=@AccountHierarchyID ", param, commandType: null).FirstOrDefault();
                con.Close();
            }
            return parentId;
        }

        public string GetJobFunctionExternalId(int jobFunctionId)
        {
            string jobFunctionExternal;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@JobFunctionID", jobFunctionId);
                jobFunctionExternal = con
                    .Query<string>("Select ExternalID From ContactJobFunctions Where JobFunctionID=@JobFunctionID ", param, commandType: null).FirstOrDefault();
                con.Close();
            }
            return jobFunctionExternal;
        }

        public int GetJobFunctionIdByExternal(string jobFunctionExternalId)
        {
            int jobFunctionId;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", jobFunctionExternalId);
                jobFunctionId = con
                    .Query<int>("Select JobFunctionID From ContactJobFunctions Where ExternalID=@ExternalID ", param, commandType: null).FirstOrDefault();
                con.Close();
            }
            return jobFunctionId;
        }

        public List<AccountHierarchyDb> AccountHierarchiesGetByExternal(string sapHierarchyId)
        {
            List<AccountHierarchyDb> accountHierarchyDb = new List<AccountHierarchyDb>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@SAPHierarchyID", sapHierarchyId);
                var result = con
                    .Query<AccountHierarchyDb>("uspAccountHierarchiesGetByExternal", param,
                        commandType: CommandType.StoredProcedure);
                if (result != null && result.Count() > 0)
                {
                    accountHierarchyDb = result.ToList();
                }
                con.Close();
            }
            return accountHierarchyDb;
        }

        public List<AccountFocusObjectTypeDb> AccountFocusObjectTypesGet()
        {
            List<AccountFocusObjectTypeDb> accountFocusObjectTypesDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                accountFocusObjectTypesDbs = con
                    .Query<AccountFocusObjectTypeDb>("uspAccountFocusObjectTypesGet", commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return accountFocusObjectTypesDbs;
        }
        public List<AccountFocusTypeDb> AccountFocusTypesGet()
        {
            List<AccountFocusTypeDb> accountFocusTypesDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                accountFocusTypesDbs = con
                    .Query<AccountFocusTypeDb>("uspAccountFocusTypesGet", commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return accountFocusTypesDbs;
        }
        public List<AccountFocusDb> AccountFocusesGet(int accountId)
        {
            List<AccountFocusDb> accountFocusDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                accountFocusDbs = con
                    .Query<AccountFocusDb>("uspAccountFocusesGet", param, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return accountFocusDbs;
        }

        public int SetAccountFocus(SetAccountFocusRequest setAccountFocusRequest)
        {
            int RowCount;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@FocusID", setAccountFocusRequest.FocusID);
                param.Add("@AccountID", setAccountFocusRequest.AccountID);
                param.Add("@FocusTypeID", setAccountFocusRequest.FocusTypeID);
                param.Add("@FocusObjectTypeID", setAccountFocusRequest.FocusObjectTypeID);
                param.Add("@ObjectID", setAccountFocusRequest.ObjectID);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                //parm.Add("@IsDeleted", setPurchaseOrderLineRequest.IsDeleted);
                RowCount = con.Query<int>("uspAccountFocusSet", param, commandType: CommandType.StoredProcedure)
                                   .First();
                con.Close();
            }

            return RowCount;
        }
        public int DeleteAccountFocus(DeleteAccountFocusRequest deleteAccountFocusRequest)
        {
            int RowCount;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                DynamicParameters param = new DynamicParameters();

                param.Add("@FocusID", deleteAccountFocusRequest.FocusID);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);
                RowCount = con.Query<int>("uspAccountFocusDelete", param, commandType: CommandType.StoredProcedure).First();
                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    var errorMessage = string.Format("Database error occured: {0}", AccountDbErrors.ErrorCodes[errorId]);
                    throw new GlobalApiException(errorMessage);
                }
                con.Close();
            }
            return RowCount;

        }
        public int CreateParentHierarchy(AccountBasicDetails accountBasicDetails)
        {
            int AccountHierarchyID;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RegionID", accountBasicDetails.RegionId);
                param.Add("@HierarchyName", accountBasicDetails.HierarchyName);
                param.Add("@UserID", UserHelper.GetUserId());

                AccountHierarchyID = con.Query<int>("uspAccountHierarchySet", param,
                    commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return AccountHierarchyID;
        }

        public void AccountSapDataSet(SetAccountExternalIdsRequest account, AccountTypeDetails customer, AccountTypeDetails supplier, int? status)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountID", account.ObjectId);
                param.Add("@ExternalId", account.ExternalId);
                param.Add("@UserID", UserHelper.GetUserId());

                if (status > 0)
                    param.Add("@Status", status);

                if (customer != null)
                {
                    param.Add("@CustomerCurrencyID", customer.CurrencyID);
                    param.Add("@CustomerCreditLimit", customer.CreditLimit);
                    param.Add("@CustomerEpdsId", customer.EpdsId);
                    if (customer.PaymentTermID > 0)
                        param.Add("@CustomerPaymentTermID", customer.PaymentTermID);
                }

                if (supplier != null)
                {
                    param.Add("@SupplierCurrencyID", supplier.CurrencyID);
                    param.Add("@SupplierEpdsId", supplier.EpdsId);
                    if (supplier.PaymentTermID > 0)
                        param.Add("@SupplierPaymentTermID", supplier.PaymentTermID);
                }

                con.Query("uspAccountSapDataSet", param,
                    commandType: CommandType.StoredProcedure);
                con.Close();
            }
        }

        public void ContactSapDataSet(PairedIdsRequest contact)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ContactID", contact.LocalId);
                param.Add("@ExternalId", contact.ExternalId);
                param.Add("@UserID", UserHelper.GetUserId());

                con.Query("uspContactSapDataSet", param,
                    commandType: CommandType.StoredProcedure);
                con.Close();
            }
        }

        public void LocationSapDataSet(PairedIdsRequest location)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@LocationID", location.LocalId);
                param.Add("@ExternalId", location.ExternalId);
                param.Add("@UserID", UserHelper.GetUserId());

                con.Query("uspLocationSapDataSet", param,
                    commandType: CommandType.StoredProcedure);
                con.Close();
            }
        }

        public int HierarchySapDataSet(AccountHierarchySapDetails hierarchy)
        {
            int hierarchyId;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                if(hierarchy.ParentId > 0)
                    param.Add("@ParentId", hierarchy.ParentId);
                if(hierarchy.HierarchyId > 0)
                    param.Add("@AccountHierarchyID", hierarchy.HierarchyId);
                param.Add("@RegionId", hierarchy.RegionId);
                param.Add("@HierarchyName", hierarchy.HierarchyName);
                param.Add("@ExternalId", hierarchy.ExternalHierarchyId);
                param.Add("@GroupId", hierarchy.GroupId);
                param.Add("@UserID", UserHelper.GetUserId());

                hierarchyId = con.Query<int>("uspAccountHierarchySapDataSet", param,
                    commandType: CommandType.StoredProcedure).First();
                con.Close();
            }

            return hierarchyId;
        }

        public void HierarchyChildSapDataSet(AccountHierarchySapDetails hierarchy)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ParentId", hierarchy.ParentId);
                param.Add("@ExternalId", hierarchy.ExternalHierarchyId);
                param.Add("@GroupId", hierarchy.GroupId);
                param.Add("@HierarchyName", hierarchy.HierarchyName);
                param.Add("@UserID", UserHelper.GetUserId());

                con.Query("uspAccountHierarchyChildSapDataSet", param,
                    commandType: CommandType.StoredProcedure);
                con.Close();
            }
        }

        public List<AccountTypesDataDb> AccountTypesDataGet(int accountId)
        {
            List<AccountTypesDataDb> types = new List<AccountTypesDataDb>();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountID", accountId);

                types = con.Query<AccountTypesDataDb>("uspAccountTypesTermsStatusGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return types;
        }

        public IList<AccountsDb> SupplierLineCardMatch(List<int> commodities, List<MfrObject> mfrNames)
        {
            IList<AccountsDb> accountsDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                var jsonCommodities = JsonConvert.SerializeObject(commodities);
                var jsonMfrs = JsonConvert.SerializeObject(mfrNames);
                param.Add("@CommoditiesJSON", jsonCommodities);
                param.Add("@ManufacturersJSON", jsonMfrs);
                accountsDbs = con.Query<AccountsDb>("uspSuppliersLineCardMatch", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountsDbs;
        }

        public IList<AccountGroupDb> AccountGroupListGet()
        {
            IList<AccountGroupDb> accountGroupDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", UserHelper.GetUserId());
                accountGroupDbs = con.Query<AccountGroupDb>("uspUserAccountGroupListGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountGroupDbs;
        }

        public IList<AccountGroupDb> SuppliersAccountGroupListGet()
        {
            IList<AccountGroupDb> accountGroupDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", UserHelper.GetUserId());
                accountGroupDbs = con.Query<AccountGroupDb>("uspSuppliersAccountGroupListGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountGroupDbs;
        }

        public bool UserAccountGroupDelete(int accountGroupId)
        {
            int rowCount; 
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountGroupID", accountGroupId);
                rowCount = con.Query<int>("uspUserAccountGroupDelete", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return rowCount == 1;
        }

        public IList<AccountGroupDetailDb> AccountGroupDetailGet(int accountGroupId)
        {
            IList<AccountGroupDetailDb> accountGroupDetailDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountGroupID", accountGroupId);
                accountGroupDetailDbs = con.Query<AccountGroupDetailDb>("uspAccountGroupDetailGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountGroupDetailDbs;
        }

        public int AccountGroupSet(AccountGroupSetRequest accountGroupSetRequest)
        {
            var accountGroupID = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountGroupID", accountGroupSetRequest.AccountGroupID);
                param.Add("@GroupName", accountGroupSetRequest.GroupName);
                param.Add("@IsDeleted", accountGroupSetRequest.IsDeleted);
                param.Add("@UserID", UserHelper.GetUserId());
                param.Add("@GroupLinesData", JsonConvert.SerializeObject(accountGroupSetRequest.AccountGroupLines));

                accountGroupID = con.Query<int>("uspAccountGroupSet", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return accountGroupID;
        }

        public IList<AccountGroupDetailDb> SuppliersAccountGroupMatch(int accountGroupId)
        {
            IList<AccountGroupDetailDb> accountGroupDetailDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountGroupID", accountGroupId);
                accountGroupDetailDbs = con.Query<AccountGroupDetailDb>("uspSuppliersAccountGroupMatch", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountGroupDetailDbs;
        }

        public List<AccountStatusDb> GetAccountStatuses(int accountId)
        {
            List<AccountStatusDb> accountStatusDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountID", accountId);
                accountStatusDbs = con.Query<AccountStatusDb>("uspAccountStatusGet", param,
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return accountStatusDbs;
        }

        public int GetAccountIdByExternal(string externalId)
        {
            int accountId;

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                var res = con
                  .Query<int>("uspAccountByExternalGet", param,
                      commandType: CommandType.StoredProcedure);
                accountId = res.FirstOrDefault();
                con.Close();
            }

            return accountId;

        }

        public int GetContactIdByExternal(string externalId)
        {
            int contactId;

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                var res = con
                  .Query<int>("uspContactByExternalGet", param,
                      commandType: CommandType.StoredProcedure);
                contactId = res.FirstOrDefault();
                con.Close();
            }

            return contactId;

        }

        public int GetLocationIdByExternal(string externalId)
        {
            int locationId;

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                var res = con
                  .Query<int>("uspLocationByExternalGet", param,
                      commandType: CommandType.StoredProcedure);
                locationId = res.FirstOrDefault();
                con.Close();
            }

            return locationId;

        }

        //public AccountBasicDetailsDb AccountInfoSet(AccountBasicDetails accountBasicDetails, int organizationId)
        //{
        //    using (var con = new SqlConnection(ConnectionString))
        //    {
        //        AccountBasicDetailsDb savedRecord;
        //        con.Open();
        //        DynamicParameters param = new DynamicParameters();

        //        param.Add("@accountId", accountBasicDetails.AccountId);
        //        param.Add("@AccountName", accountBasicDetails.Name);
        //        param.Add("@AccountStatusID", accountBasicDetails.StatusId);
        //        param.Add("@AccountTypeBitwise", accountBasicDetails.AccountTypeIds.Sum());
        //        param.Add("@CompanyTypeID", accountBasicDetails.CompanyTypeId);
        //        param.Add("@OrganizationID", organizationId);
        //        param.Add("@CreatedBy", UserHelper.GetUserId());
        //        param.Add("@ret", direction: ParameterDirection.ReturnValue);

        //        var res = con.Query<AccountBasicDetailsDb>("uspAccountSet", param, commandType: CommandType.StoredProcedure);
        //        con.Close();

        //        var errorId = param.Get<int>("@ret");
        //        if (errorId != 0)
        //        {
        //            var errorMessage = string.Format("Database error occured: {0}", AccountDbErrors.ErrorCodes[errorId]);
        //            throw new GlobalApiException(errorMessage);
        //        }
        //        savedRecord = res.First();
        //        return savedRecord;
        //    }
        //}
    }
}
