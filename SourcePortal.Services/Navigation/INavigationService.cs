using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.API.Responses.Security;

namespace SourcePortal.Services.Navigation
{
    public interface INavigationService
    {
        UserObjectSecurityGetResponse UserObjectSecurityGet(int objectId, int objectTypeId);
        NavigationsGetResponse NavigationListGet();
        GeneralSecurityGetResponse GeneralSecuritiesGet();
        bool UserObjectLevelSecurityGet(int objectId, int objectTypeId);
    }
}
