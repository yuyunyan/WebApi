using Sourceportal.DB.Enum;

namespace SourcePortal.Services.Roles
{
    public class RoleDetailsCreatorFactory
    {
        public IRoleDetailsCreator GetCreator(ObjectType objectType)
        {
            if (objectType == ObjectType.Navigation)
            {
                return new NavigationLinkCreator();
            }

            return new NonNavigationRoleDetailsCreator();
        }
    }
}
