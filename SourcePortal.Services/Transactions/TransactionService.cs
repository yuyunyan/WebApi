using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Sourceportal.DB.User;
using Sourceportal.Domain.Models.API.Responses.Transactions;
using Sourceportal.Domain.Models.Middleware.Transactions;
using SourcePortal.Services.ApiService;

namespace SourcePortal.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly IRestClient _restClient;
        private readonly IUserRepository _userRepository;

        public TransactionService(IRestClient restClient, IUserRepository userRepository)
        {
            _restClient = restClient;
            _userRepository = userRepository;
        }

        public TransactionResponseMw GetTransactions()
        {
            var queryParams =  new NameValueCollection{};

            var responseMw = _restClient.Get<TransactionResponseMw>("transaction/log", queryParams);
            return responseMw;
        }
    }
}
