using Sourceportal.Domain.Models.DB.Accounts;
using System.Collections.Generic;
using Sourceportal.Domain.Models.DB.Quotes;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.DB.CommonData;
using Sourceportal.Domain.Models.API.Requests.CommonData;
using Sourceportal.Domain.Models.DB.PurchaseOrders;
using CurrencyDb = Sourceportal.Domain.Models.DB.Accounts.CurrencyDb;
using Sourceportal.Domain.Models.API.Responses.Accounts;

namespace Sourceportal.DB.CommonData
{
   public interface ICommonDataRepository
    {
        int GetPackagingIdFromExternal(string externalId);
        int GetPackageConditionIdFromExternal(string externalId);
        IList<ContactListContactDb> GetContactsList(int accountId);
        IList<CurrencyDb> GetAllCurrencies();
        IList<RegionDb> GetAllRegions();
        IList<IncotermDb> GetAllIncoterms();
        IList<OrganizationsDb> GetAllOrganizations(int objectTypeId = 0);
        IList<TimezoneDb> GetTimeZones();
        IList<CustomerAccountDb> GetAllAccounts(int selectedAccount);
        int GetIncotermOnAccount(int accountId);
        IList<AccountByTypeDb> GetAccountsByType(int type);
        IList<StatusDb> GetStatusObjectTypeId(ObjectType objectType);
        IList<AccountByObjectTypeDb> GetAccountsByObjectType(int? objectTypeId, int? selectedAccountId);
        GridSettingsDb GetGridSettings(string GridName);
        int SetGridSettings(GridSettingsRequest request);
        IList<ShippingMethodDb> GetAllShippingMethods();
        IList<FreightPaymentMethodDb> GetAllFreightPaymentMethods();
        IncotermDb GetIncoterm(int incoTermId);
        OrganizationsDb GetOrganization(int organizationId);
        string GetParentOrganizationExternalId(int organizationId);
        List<PaymentTermDb> GetPaymentTerms(int paymentTermId);
        CurrencyDb GetCurrency(string currencyId);
        IList<CountryDb> GetCountry(int countryId);
        List<PackagingOptionsDb> GetPackagingOptions(int packagingId);
        List<PackageConditionsDb> GetPackageConditions(int packageConditionId);
        ConfigValueDb GetConfigValue(string configName);
        string GetCurrencyIdByExternal(string externalId);
        List<DeliveryRuleDb> GetDeliveryRules();
        int GetPaymentTermIdByExternal(string externalId);
        int GetStatusIdByExternal(string externalId);
        List<CompanyTypesDb> GetCompanyTypes(int? companyTypeId = null);
        int GetCountryByExternal(string externalId);
        int GetStateByExternal(string externalId);
        List<CarrierMethodDb> GetCarrierMethods(int? carrierId);
        List<WarehouseDb> GetWarehouses(int? organizationID);
        List<WarehouseBinDb> GetWarehouseBins();
    }
}
