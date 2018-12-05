using Sourceportal.Domain.Models.Middleware;
using Sourceportal.Domain.Models.Middleware.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourcePortal.Services.Accounts
{
    public interface IAccountSyncRequestCreator
    {
        MiddlewareSyncRequest<AccountSync> Create(int accountId);
    }
}
