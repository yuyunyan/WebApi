using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sourceportal.DB.Navigation;
using Sourceportal.Domain.Models.API.Responses.Security;

namespace SourcePortal.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly INavigationRepository _navigationRepository;

        public NavigationService(INavigationRepository NavigationRepository)
        {
            _navigationRepository = NavigationRepository;
        }

        public NavigationsGetResponse NavigationListGet()
        {
            var dbNavList = _navigationRepository.NavigationListGet();
            var navigationList = new List<NavigationResponse>();
            foreach (var dbNavigation in dbNavList)
            {
                var navigation = new NavigationResponse();
                navigation.NavID = dbNavigation.NavID;
                navigation.NavName = dbNavigation.NavName;
                navigation.ParentNavID = dbNavigation.ParentNavID;
                navigation.Icon = dbNavigation.Icon;
                navigation.Interface = dbNavigation.Interface;
                navigation.SortOrder = dbNavigation.SortOrder;
                navigationList.Add(navigation);
            }
            return new NavigationsGetResponse
            {
                IsSuccess = true,
                Navigations = navigationList
            };
        }

        public GeneralSecurityGetResponse GeneralSecuritiesGet()
        {
            var dbGeneralSecurity = _navigationRepository.GeneralSecuritiesGet();
            var gsList = new List<GeneralSecurityObject>();
            foreach (var generalSecurity in dbGeneralSecurity)
            {
                var gs = new GeneralSecurityObject();
                gs.Type = generalSecurity.Type;
                gs.ID = generalSecurity.ID;
                gs.Name = generalSecurity.Name;
                gsList.Add(gs);
            }
            return new GeneralSecurityGetResponse
            {
                GeneralSecurityList = gsList,
                IsSuccess = true
            };
        }

        public UserObjectSecurityGetResponse UserObjectSecurityGet(int objectId, int objectTypeId)
        {
            var dbUserObjectSecurities = _navigationRepository.UserObjectSecurityGet(objectId, objectTypeId);
            var userSecList = new List<UserObjectSecurity>();
            foreach (var dbUserObjectSecurity in dbUserObjectSecurities)
            {
                var userSec = new UserObjectSecurity
                {
                    FieldID = dbUserObjectSecurity.FieldID,
                    Name = dbUserObjectSecurity.Name,
                    CanEdit = dbUserObjectSecurity.CanEdit == 1
                };
                userSecList.Add(userSec);
            }
            return new UserObjectSecurityGetResponse
            {
                UserObjectSecurities = userSecList,
                IsSuccess = true
            };
        }

        public bool UserObjectLevelSecurityGet(int objectId, int objectTypeId)
        {
            var checkUserObjectSecurity = _navigationRepository.UserObjectLevelSecurityGet(objectId, objectTypeId);
            return checkUserObjectSecurity;
        }
    }
}
