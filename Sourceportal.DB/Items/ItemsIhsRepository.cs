using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using Elasticsearch.Net;
using Nest;

using Sourceportal.Domain.Models.DB.Items;
using Sourceportal.DB.ErrorManagementService;
using Sourceportal.Domain.Models.API.Requests.ErrorLog;
using Sourceportal.Domain.Models.API.Responses.ErrorLog;
using Sourceportal.Domain.Models.Services.ErrorManagement;

namespace Sourceportal.DB.Items
{
    public class ItemsIhsRepository : IItemsIhsRepository
    {
        private static readonly string ESurl = ConfigurationManager.AppSettings["ElasticSearchUrl"];
        private static readonly string EsUser = ConfigurationManager.AppSettings["ElasticSearchUser"];
        private static readonly string EsPassword = ConfigurationManager.AppSettings["ElasticSearchPassowrd"];
        private static readonly string[] FieldsToInclude =
            {"abstractProduct.ihsId", "abstractProduct.mpn", "abstractProduct.manufacturer", "abstractProduct.categories.en_US"};
        private static readonly string CompletionField = "completionTermsFuzzy.en_US";
        private readonly IErrorManagementRepository _errorManagementRepository;
        private readonly ElasticClient _client;

        public ItemsIhsRepository(IErrorManagementRepository errorManagementRepository)
        {
            _errorManagementRepository = errorManagementRepository;
            var node = new Uri(ESurl);
            var settings = new ConnectionSettings(node);
            settings.BasicAuthentication(EsUser, EsPassword);
            settings.DefaultIndex("parts*");
            _client = new ElasticClient(settings);

        }
        public List<ItemIhs> GetItems(string searchString, int limit)
        {
            

            var result = _client.Search<ItemIhs>(s => s
                .Source(y => y.Includes(f => f.Fields(FieldsToInclude)))
                .Suggest(ss => ss
                    .Completion("suggest", c => c
                        .Field(CompletionField)
                        .Fuzzy(f => f
                            .Fuzziness(Fuzziness.EditDistance(0))
                            .MinLength(0)
                            .PrefixLength(0)
                            .Transpositions()
                            .UnicodeAware(false)
                        )
                        .Prefix(searchString)
                        .Size(limit)
                    )
                ));
            if (result.ApiCall.Success)
            {
                if (result.Suggest.Count > 0)
                {
                    var itemIhsList = result.Suggest.First().Value.First().Options.ToList().Select(x => x.Source);
                    return itemIhsList.ToList();
                }
                else
                    return new List<ItemIhs>();
            }
            else
            {
                //Fire email (future)
                ExceptionLogSave ihsLog = new ExceptionLogSave()
                {
                    ApplicationId = (int)Enum.ApplicationType.WebApi,
                    Url = "api/items/getSuggestions?searchString=" + searchString,
                    ExceptionType = "ElasticSearch",
                    ErrorMessage = result.ApiCall.OriginalException.ToString(),
                    TimeStamp = DateTime.Now,
                    UserId = Utilities.UserHelper.GetUserId()
                };
                _errorManagementRepository.SaveLogDb(ihsLog);
                return new List<ItemIhs>();
            }
        }

        public List<ItemIhs> SearchItem(string partnumber)
        {
            var query = new BoolQuery
            {
                Must = new QueryContainer[] { new MatchQuery { Field = Nest.Infer.Field("abstractProduct.mpn"), Query = partnumber }, }
            };

            var request = new SearchRequest
            {
                Query = query,
                Source = new SourceFilter { Includes = new string[]{ "abstractProduct.ihsId", "abstractProduct.mpn", "abstractProduct.technicalData" } }
            };

            //var json = client.RequestResponseSerializer.SerializeToString(request);
            var result = _client.Search<ItemIhs>(request);

            //if (result.Documents.Count > 0)
            //{
                return result.Documents.ToList();
            //}
        }
    }
}
