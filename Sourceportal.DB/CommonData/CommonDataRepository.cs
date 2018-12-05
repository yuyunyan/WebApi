using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sourceportal.Domain.Models.DB.Accounts;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
using System.Xml.Schema;
using Sourceportal.DB.Enum;
using Sourceportal.Domain.Models.DB.Quotes;
using Sourceportal.Domain.Models.DB.CommonData;
using Sourceportal.Utilities;
using Sourceportal.Domain.Models.API.Requests.CommonData;
using Sourceportal.Domain.Models.API.Responses.PurchaseOrders;
using Sourceportal.Domain.Models.DB.PurchaseOrders;
using CurrencyDb = Sourceportal.Domain.Models.DB.Accounts.CurrencyDb;
using Sourceportal.Domain.Models.API.Responses.Accounts;

namespace Sourceportal.DB.CommonData
{
   public class CommonDataRepository:ICommonDataRepository
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;
        public IList<ContactListContactDb> GetContactsList(int accountId)
        {
            IList<ContactListContactDb> contactDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountId", accountId);
                param.Add("@UserID", UserHelper.GetUserId());

                contactDbs = con.Query<ContactListContactDb>("uspContactsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return contactDbs;
        }

        public IList<CurrencyDb> GetAllCurrencies()
        {
            IList<CurrencyDb> currenciesDb;

            DynamicParameters param = new DynamicParameters();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                currenciesDb = con.Query<CurrencyDb>("uspCurrenciesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return currenciesDb;
        }

        public CurrencyDb GetCurrency(string currencyId)
        {
            CurrencyDb currenciesDb;
            
            DynamicParameters param = new DynamicParameters();
            param.Add("@CurrencyID", currencyId);

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                currenciesDb = con.Query<CurrencyDb>("uspCurrenciesGet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return currenciesDb;
        }

        public IList<RegionDb> GetAllRegions()
        {
            IList<RegionDb> regionsDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                regionsDb = con.Query<RegionDb>("uspShipFromRegionsGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return regionsDb;
        }

        public IList<IncotermDb> GetAllIncoterms()
        {
            IList<IncotermDb> incotermsDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                incotermsDb = con.Query<IncotermDb>("uspIncotermsGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return incotermsDb;
        }

        public IncotermDb GetIncoterm(int incoTermId)
        {
            IncotermDb incotermDb;
            DynamicParameters param = new DynamicParameters();
            param.Add("@IncotermID", incoTermId);
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                incotermDb = con.Query<IncotermDb>("uspIncotermsGet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return incotermDb;
        }

        public IList<OrganizationsDb> GetAllOrganizations(int objectTypeId = 0)
        {
            IList<OrganizationsDb> organizationsDb;
            DynamicParameters param = new DynamicParameters();
            if(objectTypeId > 0)
                param.Add("@ObjectTypeID", objectTypeId);
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                organizationsDb = con.Query<OrganizationsDb>("uspOrganizationsGet",param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return organizationsDb;
        }

        public IList<TimezoneDb> GetTimeZones()
        {
            IList<TimezoneDb> timezoneDb;
            DynamicParameters param = new DynamicParameters();

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                timezoneDb = con.Query<TimezoneDb>("SELECT * FROM sys.time_zone_info", param, commandType: null).ToList();
                con.Close();
            }
            return timezoneDb;
        }

        public OrganizationsDb GetOrganization(int organizationId)
        {
            OrganizationsDb organizationDb;
            DynamicParameters param = new DynamicParameters();
            param.Add("@OrganizationID", organizationId);
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                organizationDb = con.Query<OrganizationsDb>("uspOrganizationsGet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return organizationDb;
        }

        public string GetParentOrganizationExternalId(int organizationId)
        {
            string parentOrgExternal;
            DynamicParameters param = new DynamicParameters();
            param.Add("@OrganizationID", organizationId);
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                parentOrgExternal = con.Query<string>("SELECT O2.ExternalID from Organizations O1 " +
                    "INNER JOIN Organizations O2 ON O2.OrganizationId = O1.ParentOrgID " +
                    "WHERE O1.OrganizationID = @OrganizationID", param, commandType: null).FirstOrDefault();
                con.Close();
            }
            return parentOrgExternal;
        }

        public IList<StatusDb> GetStatusObjectTypeId(ObjectType objectType)
        {
            IList<StatusDb> SoStautsDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@ObjectTypeID", objectType);
                SoStautsDbs = con
                    .Query<StatusDb>("uspStatusesGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return SoStautsDbs;
            }

        public IList<AccountByObjectTypeDb> GetAccountsByObjectType(int? objectTypeId, int? selectedAccountId)
        {
            IList<AccountByObjectTypeDb> accountsDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@ObjectTypeID", objectTypeId);
                parm.Add("@SelectedAccountID", selectedAccountId);
                accountsDbs = con
                    .Query<AccountByObjectTypeDb>("uspAccountsByObjectTypeGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();
                con.Close();
            }
            return accountsDbs;
        }

        public IList<CustomerAccountDb> GetAllAccounts(int selectedAccount)
        {
            IList<CustomerAccountDb> customerAccountDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@AccountTypeID", ObjectType.AccountTypeCustomerId);
                parm.Add("@SelectedAccountID",selectedAccount);

                customerAccountDbs = con
                    .Query<CustomerAccountDb>("uspAccountsByTypeGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();

                con.Close();
            }
            return customerAccountDbs;
        }

        public int GetIncotermOnAccount(int accountId)
        {
            int incotermId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@AccountID", accountId);

                var result = con.Query<int?>("SELECT IncotermID FROM Accounts WHERE AccountID=@AccountID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    incotermId = result.First() != null ? (int)result.First() : 0;
                }

                con.Close();
            }

            return incotermId;
        }

        public IList<AccountByTypeDb> GetAccountsByType(int type)
        {
            IList<AccountByTypeDb> accountsByType;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@AccountTypeID", type);

                accountsByType = con
                    .Query<AccountByTypeDb>("uspAccountsByTypeGet", parm, commandType: CommandType.StoredProcedure)
                    .ToList();

                con.Close();
            }
            return accountsByType;
        }


        public GridSettingsDb GetGridSettings(string GridName)
        {
            GridSettingsDb GridSettings;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@UserID", UserHelper.GetUserId());
                parm.Add("@GridName", GridName);

                GridSettings = con.Query<GridSettingsDb>("uspGridSettingsGet", parm, commandType: CommandType.StoredProcedure).First();

                con.Close();
            }
            return GridSettings;
        }

        public int SetGridSettings(GridSettingsRequest request)
        {
            int ret = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters parm = new DynamicParameters();

                parm.Add("@UserID", UserHelper.GetUserId());
                parm.Add("@GridName", request.GridName);
                parm.Add("@ColumnDef", request.ColumnDef);
                parm.Add("@SortDef", request.SortDef);
                parm.Add("@FilterDef", request.FilterDef);

                con.Query("uspGridSettingsSet", parm, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            return ret;
        }

        public IList<ShippingMethodDb> GetAllShippingMethods()
        {
            IList<ShippingMethodDb> shippingMethodDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                shippingMethodDbs = con.Query<ShippingMethodDb>("uspShippingMethodsGet").ToList();
                con.Close();
            }
            return shippingMethodDbs;
        }

        public IList<FreightPaymentMethodDb> GetAllFreightPaymentMethods()
        {
            IList<FreightPaymentMethodDb> freightPaymentMethodDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                freightPaymentMethodDbs = con.Query<FreightPaymentMethodDb>("uspFreightPaymentMethodsGet").ToList();
                con.Close();
            }
            return freightPaymentMethodDbs;
        }

        public List<PaymentTermDb> GetPaymentTerms(int paymentTermId)
        {
            List<PaymentTermDb> purchaseOrderDb;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@PaymentTermID", paymentTermId);

                purchaseOrderDb = con.Query<PaymentTermDb>("uspPaymentTermsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return purchaseOrderDb;
        }

        public IList<CountryDb> GetCountry(int countryId)
        {
            IList<CountryDb> countryDbs;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@CountryID", countryId);
                countryDbs = con.Query<CountryDb>("uspCountriesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }

            return countryDbs;
        }

        public int GetCountryByExternal(string externalId)
        {
            int countryId;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                countryId = con.Query<int>("uspCountriesGetByExternal", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }

            return countryId;
        }

        public int GetStateByExternal(string externalId)
        {
            int stateId;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                stateId = con.Query<int>("uspStatesGetByExternal", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }

            return stateId;
        }

        public List<PackagingOptionsDb> GetPackagingOptions(int packagingId)
        {
            List<PackagingOptionsDb> packagingOptionsDbs;

            DynamicParameters param = new DynamicParameters();

            param.Add("@PackagingID", packagingId);

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                packagingOptionsDbs = con
                    .Query<PackagingOptionsDb>("uspPackagingGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return packagingOptionsDbs;
        }

        public List<PackageConditionsDb> GetPackageConditions(int packageConditionId)
        {
            List<PackageConditionsDb> packageConditionsDbs;

            DynamicParameters param = new DynamicParameters();

            param.Add("@PackageConditionID", packageConditionId);

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                packageConditionsDbs = con
                    .Query<PackageConditionsDb>("uspPackageConditionsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return packageConditionsDbs;
        }

        public ConfigValueDb GetConfigValue(string configName)
        {
            ConfigValueDb configValueDb;

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@configName", configName);
                var res = con
                  .Query<ConfigValueDb>("uspConfigVariablesGet", param,
                      commandType: CommandType.StoredProcedure);
                configValueDb = res.FirstOrDefault();
                con.Close();
            }

            return configValueDb;
        }

        public int GetPackagingIdFromExternal(string externalId)
        {
            int packagingId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", externalId);

                var result = con.Query<int>("SELECT PackagingID FROM codes.lkpPackaging WHERE ExternalID=@ExternalID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    packagingId = result.First();
                }

                con.Close();
            }

            return packagingId;
        }

        public int GetPackageConditionIdFromExternal(string externalId)
        {
            int packageConditionId = 0;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@ExternalID", externalId);

                var result = con.Query<int>("SELECT PackageConditionID FROM codes.lkpPackageConditions WHERE ExternalID=@ExternalID", param, commandType: null);
                if (result != null && result.Count() > 0)
                {
                    packageConditionId = result.First();
                }

                con.Close();
            }

            return packageConditionId;
        }

        public string GetCurrencyIdByExternal(string externalId)
        {
            string currencyId;

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                var res = con
                  .Query<string>("uspCurrencyByExternalGet", param,
                      commandType: CommandType.StoredProcedure);
                currencyId = res.FirstOrDefault();
                con.Close();
            }

            return currencyId;

        }
        public int GetPaymentTermIdByExternal(string externalId)
        {
            int paymentTermId;

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                var res = con
                  .Query<int>("uspPaymentTermByExternalGet", param,
                      commandType: CommandType.StoredProcedure);
                paymentTermId = res.FirstOrDefault();
                con.Close();
            }

            return paymentTermId;
        }

        public int GetStatusIdByExternal(string externalId)
        {
            int statusId;

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                param.Add("@ExternalId", externalId);
                var res = con
                  .Query<int>("uspStatusByExternalGet", param,
                      commandType: CommandType.StoredProcedure);
                statusId = res.FirstOrDefault();
                con.Close();
            }

            return statusId;
        }

        public List<CompanyTypesDb> GetCompanyTypes(int? companyTypeId = null)
        {
            List<CompanyTypesDb> companyTypes = new List<CompanyTypesDb>();

            using (var con = new SqlConnection(ConnectionString))
            {
                var param = new DynamicParameters();
                if(companyTypeId != null)
                    param.Add("@CompanyTypeId", companyTypeId);
                var res = con
                  .Query<CompanyTypesDb>("uspCompanyTypesGet", param,
                      commandType: CommandType.StoredProcedure);
                companyTypes = res.ToList();
                con.Close();
            }

            return companyTypes;
        }

        public List<DeliveryRuleDb> GetDeliveryRules()
        {
            List<DeliveryRuleDb> deliveryRuleDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                deliveryRuleDbs = con.Query<DeliveryRuleDb>("uspDeliveryRulesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return deliveryRuleDbs;
        }
        
        public List<CarrierMethodDb> GetCarrierMethods(int? carrierId)
        {
            List<CarrierMethodDb> carrierMethodDbs;
            using(var con= new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@CarrierID", carrierId);
                carrierMethodDbs = con.Query<CarrierMethodDb>("uspCarrierMethodsGet", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return carrierMethodDbs;
        }

        public List<WarehouseDb> GetWarehouses(int? organizationId)
        {
            List<WarehouseDb> warehouseDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@OrganizationID", organizationId);
                warehouseDbs = con.Query<WarehouseDb>("uspGetWarehouses", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return warehouseDbs;
        }
        public List<WarehouseBinDb> GetWarehouseBins()
        {
            List<WarehouseBinDb> warehouseBinDbs;
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                warehouseBinDbs = con.Query<WarehouseBinDb>("uspGetWarehouseBins", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return warehouseBinDbs;
        }
        
    }
}
