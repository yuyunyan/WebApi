using Sourceportal.DB.Enum;
using Sourceportal.DB.Roles;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.Security;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Responses.Roles;
using Sourceportal.Domain.Models.API.Responses.Security;
using Sourceportal.Utilities;
using System.Collections.Generic;

namespace SourcePortal.Services.Roles
{
    public class RoleService:IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Response<RoleDetailsResponse> GetRoleDetails(int roleId)
        {
            var staticData =_roleRepository.GetRoleStaticData(roleId);
            var objectType = (ObjectType) staticData.ObjectTypeId;
            var factory = new RoleDetailsCreatorFactory().GetCreator(objectType);
            var response = factory.Create(objectType, roleId, _roleRepository);

            response.IsSuccess = true;
            response.Data.RoleName = staticData.RoleName;
            response.Data.ObjectTypeId = (int)objectType;
            response.Data.RoleID = roleId;
            return response;
        }

        public RoleCreateResponse InsertUpdateDeleteRole(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest)
        {
            var response = new RoleCreateResponse();
            // Get the claims values
            var userId = UserHelper.GetUserId();
            var dbResponse = _roleRepository.InsertUpdateDeleteRole(roleCreateDeleteRequest, userId);
            var permissionsCount =
                _roleRepository.InsertUpdateDeletePermissions(roleCreateDeleteRequest, userId, dbResponse.RoleID);
            var fieldsCount =
                _roleRepository.InsertUpdateDeleteFields(roleCreateDeleteRequest, userId, dbResponse.RoleID);

            var navLinksCount =
                _roleRepository.InsertUpdateDeleteNavLinks(roleCreateDeleteRequest, userId, dbResponse.RoleID);

            response.RoleID = dbResponse.RoleID;
            return response;
        }

        public Response<RoleDetailsResponse> GetDataToCreateNewRole(ObjectType objectType)
        {
            var roleDetailCreator = new RoleDetailsCreatorFactory().GetCreator(objectType);
            var response = roleDetailCreator.Create(objectType, null, _roleRepository);
            response.Data.ObjectTypeId = (int)objectType;
            response.IsSuccess = true;
            return response;
        }

        public UserRolesResponse GetRolesForUser(int userId)
        {
            var dbUserRoles = _roleRepository.GetRolesForUser(userId);
            var userRoles = new List<UserRole>();

            foreach (var dbRole in dbUserRoles)
            {
                userRoles.Add(new UserRole
                {
                    UserRoleID = dbRole.UserRoleID,
                    ObjectTypeId = dbRole.ObjectTypeId,
                    ObjectTypeName = dbRole.ObjectName,
                    RoleId = dbRole.RoleId,
                    RoleName = dbRole.RoleName,
                    FilterObject = dbRole.FilterObjectId > 0 ? dbRole.FilterObjectId.ToString(): "All",
                    FilterObjectID = dbRole.FilterObjectId,
                    FilterObjectTypeID = dbRole.FilterObjectTypeId,
                    FilterTypeID = dbRole.FilterTypeId,
                    TypeDescription =  dbRole.TypeDescription,
                    TypeSecurityID = dbRole.TypeSecurityId
                });
            }

            return new UserRolesResponse
            {
                UserRoles = userRoles
            };
        }

        public ObjectTypeSecuritiesGetResponse GetObjectTypeSecurityList()
        {
            var objectTypeSecurities = new List<ObjectTypeSecurity>();
            var dbObjectTypeSecurities = _roleRepository.GetObjectTypeSecurityList();
            foreach (var dbObjectTypeSecurity in dbObjectTypeSecurities)
            {
                objectTypeSecurities.Add(new ObjectTypeSecurity
                {
                    TypeSecurityID = dbObjectTypeSecurity.TypeSecurityID,
                    TypeDescription = dbObjectTypeSecurity.TypeDescription,
                    ObjectTypeID = dbObjectTypeSecurity.ObjectTypeID,
                    FilterTypeID = dbObjectTypeSecurity.FilterTypeID,
                    FilterObjectTypeID = dbObjectTypeSecurity.FilterObjectTypeID
                });
            }
            return new ObjectTypeSecuritiesGetResponse
            {
                ObjectTypeSecurities = objectTypeSecurities
            };
        }

        public UserRolesResponse GetAllRoles()
        {
            var dbUserRoles = _roleRepository.GetAllRoles();
            var userRoles = new List<UserRole>();
            foreach (var dbRole in dbUserRoles)
            {
                userRoles.Add(new UserRole
                {
                    ObjectTypeId = dbRole.ObjectTypeId,
                    RoleId = dbRole.RoleId,
                    RoleName = dbRole.RoleName,
                });
            }
            return new UserRolesResponse
            {
                UserRoles = userRoles
            };
        }

        public SecurityTypeListGetResponse GetTypeOptions()
        {
            var typeList = new List<SecurityType>();
            var dbTypes = _roleRepository.GetTypeOptions();

            foreach (var dbType in dbTypes)
            {
                typeList.Add(new SecurityType
                {
                    ObjectTypeID = dbType.ObjectTypeID,
                    ObjectName = dbType.ObjectName
                });
            }
            return new SecurityTypeListGetResponse
            {
                Types = typeList
            };
        }

        public FilterObjectListGetResponse GetFilterObjects()
        {
            var filterObjects = new List<FilterObjectResponse>();
            var dbObjects = _roleRepository.GetFilterObjects();
            foreach (var dbFilterObject in dbObjects)
            {
                filterObjects.Add(new FilterObjectResponse
                {
                    ObjectID = dbFilterObject.ObjectID,
                    ObjectTypeID = dbFilterObject.ObjectTypeID,
                    ObjectName = dbFilterObject.ObjectName
                });
            }
            return new FilterObjectListGetResponse
            {
                FilterObjects = filterObjects
            };

        }

        public UserNavigationRolesGetResponse GetUserNavigationRoles(int userId)
        {
            var dbNavRoles = _roleRepository.GetUserNavigationRoles(userId);
            var navigationRoles = new List<UserNavigationRole>();
            foreach (var dbNavRole in dbNavRoles)
            {
                var navigationRole = new UserNavigationRole
                {
                    RoleID = dbNavRole.RoleID,
                    RoleName = dbNavRole.RoleName,
                    IsDeleted = dbNavRole.IsDeleted,
                    UserRoleID = dbNavRole.UserRoleID
                };
                navigationRoles.Add(navigationRole);
            }
            return new UserNavigationRolesGetResponse
            {
                IsSuccess = true,
                NavigationRoles = navigationRoles
            };
        }

        public UserNavigationRoleSetResponse SetUserNavigationRole(
            UserNavigationRoleSetRequest navigationRoleSetRequest)
        {
            var dbNavRole = _roleRepository.SetUserNavigationRole(navigationRoleSetRequest);
            var navigationRole = new UserNavigationRole
            {
                RoleID = dbNavRole.RoleID,
                UserRoleID = dbNavRole.UserRoleID,
                RoleName = dbNavRole.RoleName,
                IsDeleted = dbNavRole.IsDeleted
            };
            return new UserNavigationRoleSetResponse
            {
                IsSuccess = true,
                NavigationRole = navigationRole
            };
        }


        public UserRoleSetResponse SaveUserRoles(UserRolesSaveRequest userRolesSaveRequest)
        {
           
            var dbRole = _roleRepository.SaveUserRole(userRolesSaveRequest);
            return new UserRoleSetResponse
            {
                UserRoleID = dbRole.UserRoleID,
                ObjectTypeId = dbRole.ObjectTypeId,
                ObjectTypeName = dbRole.ObjectName,
                RoleId = dbRole.RoleId,
                RoleName = dbRole.RoleName,
                FilterObject = dbRole.FilterObjectId > 0 ? dbRole.FilterObjectId.ToString() : "All",
                FilterObjectID = dbRole.FilterObjectId,
                FilterObjectTypeID = dbRole.FilterObjectTypeId,
                FilterTypeID = dbRole.FilterTypeId,
                TypeDescription = dbRole.TypeDescription,
                TypeSecurityID = dbRole.TypeSecurityId,
                IsSuccess = true
            };
        }

        public RoleTypeOptionsGetResponse GetRoleTypeOptions()
        {
            var dbRoleTypeOptions = _roleRepository.GetRoleTypeOptions();
            var roleTypeOptions = new List<RoleTypeOption>();

            foreach (var dbRoleTypeOption in dbRoleTypeOptions)
            {
                var roletypeOption = new RoleTypeOption
                {
                    ObjectTypeID = dbRoleTypeOption.ObjectTypeID,
                    ObjectName = dbRoleTypeOption.ObjectName
                };
                roleTypeOptions.Add(roletypeOption);
            }
            return new RoleTypeOptionsGetResponse
            {
                IsSuccess = true,
                RoleTypeOptions = roleTypeOptions
            };
        }
    }

}
