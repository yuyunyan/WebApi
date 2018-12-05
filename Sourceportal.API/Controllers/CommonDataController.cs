using System;
using SourcePortal.Services.SalesOrder;
using System.Web.Http;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.API.Requests.Quotes;
using Sourceportal.Domain.Models.API.Responses.SalesOrders;
using SourcePortal.Services.CommonData;
using Sourceportal.Domain.Models.API.Responses.Accounts;
using SourcePortal.Services.Accounts;
using Sourceportal.Domain.Models.API.Responses.Items;
using SourcePortal.Services.Items;
using Sourceportal.Domain.Models.API.Responses.ObjectTypes;
using Sourceportal.Domain.Models.API.Responses.CommonData;
using Sourceportal.Domain.Models.API.Requests.CommonData;
using System.Collections.Generic;

namespace Sourceportal.API.Controllers
{

    public class CommonDataController : ApiController
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ICommonDataService _commonDataService;
        private readonly IAccountService _accountService;
        private readonly IItemService _itemService;

        public CommonDataController(ISalesOrderService salesOrderService, ICommonDataService commonDataService1,
            IAccountService accountService, IItemService itemservice)
        {
            _salesOrderService = salesOrderService;
            _commonDataService = commonDataService1;
            _accountService = accountService;
            _itemService = itemservice;
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAccounts")]
        public AccountsResponse GetAccountsList(int selectedAccount)
        {
            return _commonDataService.GetAccountsList(selectedAccount);
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getIncotermOnAccount")]
        public int GetIncotermOnAccount(int accountId)
        {
            return _commonDataService.GetIncotermOnAccount(accountId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAccountsByType")]
        public AccountsResponse GetAccountsByType(int type)
        {
            return _commonDataService.GetAccountsByType(type);
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/GetCountryList")]
        public CountryListResponse GetCountryList()
        {
            var countryList = _accountService.GetCountryList();
            return new CountryListResponse {Countries = countryList};
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/GetLocationsByAccountId")]
        public AccountLocationResponse GetLocationsByAccount(int accountId)
        {
            var locations = _accountService.GetAccountLocations(accountId);
            return new AccountLocationResponse {Locations = locations};
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/GetContactsByAccountId")]
        public ContactListResponse GetContactsByAccount(int accountId)
        {
            return _commonDataService.GetContactsByAccount(accountId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAllCurrencies")]
        public CurrencyResponse GetAllCurrencies()
        {
            return _commonDataService.GetAllCurrencies();
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAllRegions")]
        public RegionResponse GetAllRegions()
        {
            return _commonDataService.GetAllRegions();
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getCompanyTypes")]
        public IList<CompanyType> GetCompanyTypes()
        {
            return _accountService.GetAllCompanyTypes();
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAllIncoterms")]
        public IncotermsResponse GetAllIncoterms()
        {
            return _commonDataService.GetAllIncoterms();
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAllOrganizations")]
        public OrganizationsResponse GetAllOrganizations(int objectTypeId = 0)
        {
            return _commonDataService.GetAllOrganizations(objectTypeId);
        }

        [Authorize]
        [Route("api/common/Timezones")]
        [HttpGet]
        public Domain.Models.API.Responses.TimezoneResponse GetTimeZones()
        {
            return _commonDataService.GetTimeZones();
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getManufacturersList")]
        public ManufacturerListResponse ManufacturersList(string searchText)
        {
            return _itemService.GetManufacturerList(searchText);
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAccountsByObjectType")]
        public AccountsByObjectTypeResponse GetAccountsByObjectType(int? objectTypeId,int? selectedAccountId)
        {
            return _commonDataService.GetAccountsByObjectType(objectTypeId, selectedAccountId);
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getGridSettings")]
        public GridSettingsResponse GetGridSettings(string gridName)
        {
            return _commonDataService.GetGridSettings(gridName);
        }

        [Authorize]
        [HttpPost]
        [Route("api/common/setGridSettings")]
        public int SetGridSettings(GridSettingsRequest request)
        {
            return _commonDataService.SetGridSettings(request);
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAllShippingMethods")]
        public ShippingMethodResponse GetAllShippingMethods()
        {
            return _commonDataService.GetAllShippingMethods();
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getAllFreightPaymentMethods")]
        public FreightPaymentMethodsGetResponse GetAllFreightPaymentMethods()
        {
            return _commonDataService.GetAllFreightPaymentMethods();
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getConfigValue")]
        public ConfigValueResponse getConfigValue(string configName)
        {
            return _commonDataService.GetConfigValue(configName);
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getDeliveryRules")]
        public DeliveryRuleListResponse GetDeliveryRules()
        {
            return _commonDataService.GetDeliveryRules();
        }

        [Authorize]
        [HttpGet]
        [Route("api/common/getInventoryObjectTypeId")]
        public ObjectTypeIdResponse GetInventoryObjectTypeId()
        {
            var response = new ObjectTypeIdResponse();
            response.ObjectTypeId = (int)ObjectType.Inventory;
            return response;
        }

        [HttpGet]
        [Route("api/common/getCarrierMethods")]
        public CarrierMethodResponse GetCarrierMethods(int? carrierId)
        {
            return _commonDataService.GetCarrierMethods(carrierId);
        }


        [HttpGet]
        [Route("api/common/getWarehouses")]
        public WarehouseResponse GetWarehouses(int? organizationId)
        {
            return _commonDataService.GetWarehouses(organizationId);
        }

        [HttpGet]
        [Route("api/common/getWarehouseBins")]
        public WarehouseBinResponse GetWarehouseBins()
        {
            return _commonDataService.GetWarehouseBins();
        }
    }
}
