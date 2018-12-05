using System.Collections.Generic;
using System.Linq;
using Sourceportal.DB.CommonData;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using Sourceportal.Domain.Models.API.Responses.SalesOrders;
using Sourceportal.Domain.Models.API.Requests.CommonData;
using Sourceportal.Domain.Models.API.Responses.PurchaseOrders;
using CurrencyResponse = Sourceportal.Domain.Models.API.Responses.CommonData.CurrencyResponse;
using Sourceportal.Domain.Models.API.Responses;

namespace SourcePortal.Services.CommonData
{
    public class CommonDataService : ICommonDataService
    {
        private ICommonDataRepository _commonDataRepository;

        public CommonDataService(ICommonDataRepository commonDataRepository)
        {
            _commonDataRepository = commonDataRepository;
        }


        public ContactListResponse GetContactsByAccount(int accountId)
        {
            var dbContacts = _commonDataRepository.GetContactsList(accountId);
            var list = new List<ContactResponse>();

            foreach (var dbContact in dbContacts)
            {
                list.Add(new ContactResponse
                {
                    ContactId = dbContact.ContactId,
                    AccountTypes = CleanLists(dbContact.AccountTypes).Split(',').ToList().Select(x => new AccountType { Name = x }).ToList(),
                    Owners = !string.IsNullOrEmpty(dbContact.Owners) ? CleanLists(dbContact.Owners).Split(',').ToList().Select(x => new Owner { Name = x }).ToList() : null,
                    AccountId = dbContact.AccountId,
                    AccountName = dbContact.AccountName,
                    AccountStatus = dbContact.AccountStatus,
                    Email = dbContact.Email,
                    FirstName = dbContact.FirstName,
                    LastName = dbContact.LastName,
                    IsActive = dbContact.IsActive,
                    Phone = dbContact.OfficePhone,
                    Title = dbContact.Title
                });
            }

            return new ContactListResponse { Contacts = list };
        }

        public CurrencyResponse GetAllCurrencies()
        {
            var dbCurrencies = _commonDataRepository.GetAllCurrencies();
            var currencies = new List<Currency>();

            foreach (var dbCurrency in dbCurrencies)
            {
                currencies.Add(new Currency { Id = dbCurrency.CurrencyId, Name = dbCurrency.CurrencyName, ExternalID = dbCurrency.ExternalID });
            }

            return new CurrencyResponse { Currencies = currencies };
        }

        public RegionResponse GetAllRegions()
        {
            var dbRegions = _commonDataRepository.GetAllRegions();
            var regions = new List<Regions>();

            foreach (var dbRegion in dbRegions)
            {
                regions.Add(new Regions { ShipfromRegionID = dbRegion.ShipfromRegionID, RegionName = dbRegion.RegionName, OrganizationID = dbRegion.OrganizationID, CountryID = dbRegion.CountryID });
            }

            return new RegionResponse { Regions = regions };
        }

        public IncotermsResponse GetAllIncoterms()
        {
            var dbIncoterms = _commonDataRepository.GetAllIncoterms();
            var incoterms = new List<Incoterm>();

            foreach (var dbIncoterm in dbIncoterms)
            {
                incoterms.Add(new Incoterm { IncotermID = dbIncoterm.IncotermID, IncotermName = dbIncoterm.IncotermName, ExternalID = dbIncoterm.ExternalID });
            }

            return new IncotermsResponse { Incoterms = incoterms };
        }

        public DeliveryRuleListResponse GetDeliveryRules()
        {
            var deliveryRuleDbs = _commonDataRepository.GetDeliveryRules();
            var deliveryRuleList = new List<DeliveryRuleResponse>();
            foreach (var deliveryRuleDb in deliveryRuleDbs)
            {
                deliveryRuleList.Add(new DeliveryRuleResponse
                {
                    DeliveryRuleId = deliveryRuleDb.DeliveryRuleId,
                    DeliveryRuleName = deliveryRuleDb.DeliveryRuleName
                });
            }
            return new DeliveryRuleListResponse
            {
                DeliveryRuleList = deliveryRuleList
            };
        }

        public OrganizationsResponse GetAllOrganizations(int ObjectTypeId = 0)
        {
            var dbOrgs = _commonDataRepository.GetAllOrganizations(ObjectTypeId);
            var orgs = new List<Organization>();

            foreach (var dbOrg in dbOrgs)
            {
                orgs.Add(new Organization { OrganizationID = dbOrg.OrganizationID, ParentOrgID = dbOrg.ParentOrgID, Name = dbOrg.Name, ExternalID = dbOrg.ExternalID });
            }

            return new OrganizationsResponse { Organizations = orgs };
        }

        public TimezoneResponse GetTimeZones()
        {
            var dbTimezones = _commonDataRepository.GetTimeZones();
            var tmzs = new List<TimeZone>();

            foreach (var dbTimezone in dbTimezones)
            {
                tmzs.Add(new TimeZone { name = dbTimezone.name, current_utc_offset = dbTimezone.current_utc_offset, is_currently_dst = dbTimezone.is_currently_dst });
            }

            return new TimezoneResponse { TimeZone = tmzs };
        }

        public AccountsResponse GetAccountsList(int selectedAccount)
        {
            var dbCustomerAccount = _commonDataRepository.GetAllAccounts(selectedAccount);
            var response = new List<Account>();

            foreach (var value in dbCustomerAccount)
            {
                response.Add(new Account { Name = value.AccountName, Id = value.AccountId });
            }
            return new AccountsResponse { Accounts = response };

        }

        public int GetIncotermOnAccount(int accountId)
        {
            return _commonDataRepository.GetIncotermOnAccount(accountId);
        }

        public AccountsResponse GetAccountsByType(int type)
        {
            var dbAccounts = _commonDataRepository.GetAccountsByType(type);
            var response = new List<Account>();

            foreach (var value in dbAccounts)
            {
                response.Add(new Account { Name = value.AccountName, Id = value.AccountId, TypeId = value.AccountTypeID });
            }
            return new AccountsResponse { Accounts = response };

        }

        private static string CleanLists(string stringToClean)
        {
            return !string.IsNullOrEmpty(stringToClean) ? stringToClean.Trim().TrimEnd(',') : null;
        }

        public StatusListResponse GetStatusesResponse(ObjectType objectType)
        {
            var response = GetStatuses(objectType);

            return new StatusListResponse { StatusList = response };
        }

        public List<Status> GetStatuses(ObjectType objectType)
        {
            var dbSoStatus = _commonDataRepository.GetStatusObjectTypeId(objectType);
            var response = new List<Status>();

            foreach (var value in dbSoStatus)
            {
                response.Add(new Status { Name = value.StatusName, Id = value.StatusId, IsDefault = value.IsDefault });
            }
            return response;
        }

        public AccountsByObjectTypeResponse GetAccountsByObjectType(int? objectTypeId, int? selectedAccountId)
        {
            var dbAccounts = _commonDataRepository.GetAccountsByObjectType(objectTypeId, selectedAccountId);
            var list = new List<ObjectTypeAccount>();
            foreach (var value in dbAccounts)
            {
                list.Add(new ObjectTypeAccount
                {
                    AccountId = value.AccountId,
                    AccountName = value.AccountName != null ? value.AccountName.Trim() : null,
                    AccountTypeId = value.AccountTypeId,
                    StatusName = value.StatusName,
                    SupplierRating = value.SupplierRating
                });
            }
            return new AccountsByObjectTypeResponse { Accounts = list };
        }

        public GridSettingsResponse GetGridSettings(string GridName)
        {
            var dbSettings = _commonDataRepository.GetGridSettings(GridName);
            GridSettingsResponse response = new GridSettingsResponse
            {
                GridName = dbSettings.GridName,
                ColumnDef = dbSettings.ColumnDef,
                FilterDef = dbSettings.FilterDef,
                SortDef = dbSettings.SortDef,
            };

            return response;
        }

        public int SetGridSettings(GridSettingsRequest request)
        {
            return _commonDataRepository.SetGridSettings(request);
        }

        public ShippingMethodResponse GetAllShippingMethods()
        {
            var dbShippingMethods = _commonDataRepository.GetAllShippingMethods();
            var shippingMethods = new List<ShippingMethod>();

            foreach (var dbShippingMethod in dbShippingMethods)
            {
                shippingMethods.Add(new ShippingMethod { ShippingMethodID = dbShippingMethod.ShippingMethodID, MethodName = dbShippingMethod.MethodName });
            }

            return new ShippingMethodResponse { ShippingMethods = shippingMethods };
        }


        public FreightPaymentMethodsGetResponse GetAllFreightPaymentMethods()
        {
            var dbFreightPaymentMethodsDb = _commonDataRepository.GetAllFreightPaymentMethods();
            var freight = new List<FreightPaymentMethodResponse>();
            foreach (var freightPaymentMethodDb in dbFreightPaymentMethodsDb)
            {
                freight.Add(new FreightPaymentMethodResponse
                {
                    FreightPaymentMethodID = freightPaymentMethodDb.FreightPaymentMethodID,
                    MethodName = freightPaymentMethodDb.MethodName,
                    ExternalID = freightPaymentMethodDb.ExternalID,
                    UseAccountNum = freightPaymentMethodDb.UseAccountNum
                });
            }
            return new FreightPaymentMethodsGetResponse
            {
                FreightPaymentMethods = freight
            };
        }

        public PaymentTermsListResponse GetPaymentTerms(int paymentTermId)
        {
            var response = new PaymentTermsListResponse();

            var paymentTermsDb = _commonDataRepository.GetPaymentTerms(paymentTermId);

            var paymentTerms = new List<PaymentTermResponse>();

            foreach (var paymentTerm in paymentTermsDb)
            {
                var po = new PaymentTermResponse
                {
                    PaymentTermID = paymentTerm.PaymentTermID,
                    TermName = paymentTerm.TermName,
                    TermDesc = paymentTerm.TermDesc,
                    NetDueDays = paymentTerm.NetDueDays,
                    DiscountDays = paymentTerm.DiscountDays,
                    DiscountPercent = paymentTerm.DiscountPercent,
                    ExternalID = paymentTerm.ExternalID
                };

                paymentTerms.Add(po);
            }

            response.PaymentTerms = paymentTerms;
            response.IsSuccess = true;

            return response;

        }

        public ConfigValueResponse GetConfigValue(string configName)
        {
            var configValueDb = _commonDataRepository.GetConfigValue(configName);
            var response = new ConfigValueResponse();
            response.ConfigValue = configValueDb.ConfigValue;
            return response;
        }

        public CarrierMethodResponse GetCarrierMethods(int? carrierId)
        {
            var carrierMethodDb = _commonDataRepository.GetCarrierMethods(carrierId);
            var methods = new List<CarrierMethod>();
            foreach (var value in carrierMethodDb)
            {
                methods.Add(new CarrierMethod
                {
                    MethodId = value.MethodId,
                    MethodName = value.MethodName,
                    CarrierId = value.CarrierId
                });
            }
            return new CarrierMethodResponse
            {
                CarrierMethods = methods
            };
        }

        public WarehouseResponse GetWarehouses(int? organizationId)
        {
            var warehouseDb = _commonDataRepository.GetWarehouses(organizationId);
            var warehouses = new List<Warehouse>();
            foreach(var value in warehouseDb)
            {
                warehouses.Add(new Warehouse
                {
                    WarehouseID = value.WarehouseID,
                    WarehouseName = value.WarehouseName,
                    LocationID = value.LocationID,
                    ExternalID = value.ExternalID
                });

            }
            return new WarehouseResponse
            {
                Warehouses = warehouses
            };
        }
        public WarehouseBinResponse GetWarehouseBins()
        {
            var warehouseBinDb = _commonDataRepository.GetWarehouseBins();
            var warehouseBins = new List<WarehouseBin>();
            foreach (var value in warehouseBinDb)
            {
                warehouseBins.Add(new WarehouseBin
                {
                    WarehouseBinID = value.WarehouseBinID,
                    WarehouseID = value.WarehouseID,
                    BinName = value.BinName,
                    ExternalID = value.ExternalID
                });

            }
            return new WarehouseBinResponse
            {
                WarehouseBins = warehouseBins
            };
        }


    }
}
