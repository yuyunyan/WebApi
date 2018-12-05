using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.Domain.Models.DB.Navigation;

namespace Sourceportal.DB.Navigation
{
    public interface INavigationRepository
    {
        List<DbNavigation> NavigationListGet();
        List<DbGeneralSecurity> GeneralSecuritiesGet();
        List<DbUserField> UserObjectSecurityGet(int objectId, int objectTypeId);
        bool UserObjectLevelSecurityGet(int objectId, int objectTypeId);
    }
}
