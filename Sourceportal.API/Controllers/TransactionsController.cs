using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sourceportal.Domain.Models.API.Responses.Transactions;
using Sourceportal.Domain.Models.Middleware.Transactions;
using SourcePortal.Services.Transactions;

namespace Sourceportal.API.Controllers
{
    public class TransactionsController : ApiController
    {
        private readonly ITransactionService _transactionservice;

        public TransactionsController(ITransactionService transactionservice)
        {
            _transactionservice = transactionservice;
        }

        [Authorize]
        [HttpGet]
        [Route("api/transactions")]
        public TransactionResponseMw GetTransactions()
        {
            return _transactionservice.GetTransactions();
        }
    }
}
