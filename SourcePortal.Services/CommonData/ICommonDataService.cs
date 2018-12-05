using System.Collections.Generic;
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
   public interface ICommonDataService
    {
        ContactListResponse GetContactsByAccount(int accountId);
        AccountsResponse GetAccountsByType(int type);
        CurrencyResponse GetAllCurrencies();
        RegionResponse GetAllRegions();
        IncotermsResponse GetAllIncoterms();
        OrganizationsResponse GetAllOrganizations(int objectTypeId = 0);
        TimezoneResponse GetTimeZones();
        AccountsResponse GetAccountsList(int selectedAccount);
        int GetIncotermOnAccount(int accountId);
        StatusListResponse GetStatusesResponse(ObjectType objectType);
        AccountsByObjectTypeResponse GetAccountsByObjectType(int? objectTypeId, int? selectedAccountId);
        GridSettingsResponse GetGridSettings(string GridName);
        int SetGridSettings(GridSettingsRequest request);
        List<Status> GetStatuses(ObjectType objectType);
        ShippingMethodResponse GetAllShippingMethods();
        FreightPaymentMethodsGetResponse GetAllFreightPaymentMethods();
        PaymentTermsListResponse GetPaymentTerms(int paymentTermId);
        ConfigValueResponse GetConfigValue(string configName);
        DeliveryRuleListResponse GetDeliveryRules();
        CarrierMethodResponse GetCarrierMethods(int? carrierId);
        WarehouseResponse GetWarehouses(int? organizationId);
        WarehouseBinResponse GetWarehouseBins();
    }
}
