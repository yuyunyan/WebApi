using Sourceportal.Domain.Models.API.Responses.Transactions;
using Sourceportal.Domain.Models.Middleware.Transactions;

namespace SourcePortal.Services.Transactions
{
    public interface ITransactionService
    {
        TransactionResponseMw GetTransactions();
    }
}