using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Sourceportal.DB.Enum;
using Sourceportal.DB.Roles;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.DB.Roles;

namespace SourcePortal.Services.Roles
{
    public class NavigationLinkCreator : IRoleDetailsCreator
    {
        public Response<RoleDetailsResponse> Create(ObjectType objectType, int? roleId, IRoleRepository roleRepository)
        {
            var roleDetails = new Response<RoleDetailsResponse>();
            roleDetails.Data = new RoleDetailsResponse();

            var navLinks = new List<NavigationLink>();
            
            var allLinks = roleRepository.GetNavigationLinksForType();
            var linksForuser = roleId != null ? roleRepository.GetNavigationLinksForRole(roleId.Value) : new List<DbNavigationLink>();
            var idsOfLinksForUser = linksForuser.Select(x => x.NavID).ToList();

            var addedIds = new List<int>();
            navLinks = CreateLinksRecursive(allLinks, idsOfLinksForUser, addedIds, linksForuser);

            roleDetails.Data.NavigationLinks = navLinks;
            
            return roleDetails;
        }

        private static List<NavigationLink> CreateLinksRecursive(List<DbNavigationLink> allLinks, List<int> idsOfLinksForUser, List<int> addedIds,
            List<DbNavigationLink> linksForuser)
        {
            if (allLinks.Count == 0)
                return null;

            var list = new List<NavigationLink>();
            
            foreach (var dbNavigationLink in allLinks)
            {
                if (!addedIds.Contains(dbNavigationLink.NavID))
                {
                    var currentLink = linksForuser.Find(x => x.NavID == dbNavigationLink.NavID);
                    var navLink = new NavigationLink
                    {
                        NavId= dbNavigationLink.NavID,
                        NavName = dbNavigationLink.NavName,
                        
                        SelectedForRole = idsOfLinksForUser.Contains(dbNavigationLink.NavID),
                        ChildNodes = CreateLinksRecursive( allLinks.Where(x => x.ParentNavID != null && x.ParentNavID == dbNavigationLink.NavID).ToList(), idsOfLinksForUser, addedIds, linksForuser)
                    };
                    if (currentLink?.RoleID != null)
                    {
                        navLink.RoleId = (int) currentLink.RoleID;
                    }
                    list.Add(navLink);
                }
                addedIds.Add(dbNavigationLink.NavID);
            }

            return list;
        }
    }
}
