using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Sourceportal.Domain.Models.DB.Roles;
using System.Configuration;
using Dapper;
using Sourceportal.DB.Enum;
using System.Data;
using System.Security.Claims;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Requests.Security;
using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using Sourceportal.Utilities;

namespace Sourceportal.DB.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SourcePortalConnection"].ConnectionString;
        public List<DbNavigationLink> GetNavigationLinksForRole(int roleId)
        {
            List<DbNavigationLink> navLinks = new List<DbNavigationLink>();

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@RoleID",roleId);

                navLinks = con.Query<DbNavigationLink>("uspNavigationByRoleGet", param,commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return navLinks;
        }
        
        public List<DbNavigationLink> GetNavigationLinksForType()
        {
            List<DbNavigationLink> navLinks = new List<DbNavigationLink>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                navLinks = con.Query<DbNavigationLink>("uspNavigationGet",commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return navLinks;
        }

        public List<DbField> GetFieldsForType(ObjectType objectType)
        {
            List<DbField> Field = new List<DbField>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ObjectTypeID",objectType);
                Field= con.Query<DbField>("uspFieldsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();            
            }
            return Field;
        }

        public List<DbField> GetFieldsForRole(int roleId)
        {
            List<DbField> Field = new List<DbField>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@RoleID", roleId);
                Field= con.Query<DbField>("uspFieldsByRoleGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return Field;
        }

        public List<DbPermission> GetPermissionsForType(ObjectType objectType)
        {
            List<DbPermission> Permission = new List<DbPermission>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@ObjectTypeID",objectType);
                Permission= con.Query<DbPermission>("uspPermissionsGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            };

            return Permission;
        }

        public List<DbPermission> GetPermissionsForRole(int roleId)
        {
            List<DbPermission> Permission = new List<DbPermission>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();

                param.Add("@RoleID", roleId);
                Permission= con.Query<DbPermission>("uspPermissionsByRoleGet", param,commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return Permission;
        }

        public RoleDetailsResponse GetRoleStaticData(int roleId)
        {
            RoleDetailsResponse staticData;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                staticData = con.Query<RoleDetailsResponse>("select RoleName, ObjectTypeID from dbo.Roles where RoleID = "+ roleId, commandType: CommandType.Text).First();
                con.Close();
            }
            return staticData;
        }

        public ObjectType GetRoleName(int roleId)
        {
            ObjectType type;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                type = con.Query<ObjectType>("select ObjectTypeID from dbo.Roles where RoleID = " + roleId, commandType: CommandType.Text).First();
                con.Close();
            }
            return type;
        }

        public DbRoleCreated InsertUpdateDeleteRole(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest, int userId)
        {
            var dbRoleCreated = new DbRoleCreated();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RoleID", roleCreateDeleteRequest.RoleId);
                param.Add("@RoleName", roleCreateDeleteRequest.RoleName);
                param.Add("@ObjectTypeID", roleCreateDeleteRequest.ObjectTypeId);
                param.Add("@CreatorID", userId);
                param.Add("@IsDeleted", roleCreateDeleteRequest.IsDeleted);

                dbRoleCreated = con.Query<DbRoleCreated>("uspRoleSet", param, commandType: CommandType.StoredProcedure)
                       .First();
                con.Close();
                //try
                //{
                //    con.Open();
                //    DynamicParameters param = new DynamicParameters();
                //    List<int> permissionIds = roleCreateDeleteRequest.PermissionIds;
                //    List<int> navigationLinkIds = roleCreateDeleteRequest.NavigationLinkIds;

                //    var fields = roleCreateDeleteRequest.Fields;

                //    if (permissionIds != null && permissionIds.Count > 0)
                //    {
                //        param.Add("@PermissionIDs", JsonConvert.SerializeObject(permissionIds));
                //    }

                //    if (navigationLinkIds != null && navigationLinkIds.Count > 0)
                //    {
                //        param.Add("@NavIDs", JsonConvert.SerializeObject(navigationLinkIds));
                //    }
                //    if (fields != null && fields.Count > 0)
                //    {
                //        var visibleOnlyFields = GetVisibleOnlyFields(fields);
                //        var editableFields = GetEditableFields(fields);
                //        param.Add("@CanViewFieldIDs", JsonConvert.SerializeObject(visibleOnlyFields));
                //        param.Add("@CanEditFieldIDs", JsonConvert.SerializeObject(editableFields));
                //    }
                //    param.Add("@RoleID", roleCreateDeleteRequest.RoleId, direction: ParameterDirection.InputOutput);
                //    param.Add("@RoleName", roleCreateDeleteRequest.RoleName);
                //    param.Add("@ObjectTypeID", roleCreateDeleteRequest.ObjectTypeId);
                //    param.Add("@CreatorID", userId);
                //    param.Add("@IsDeleted",roleCreateDeleteRequest.IsDeleted);

                //    dbRoleCreated = con
                //        .Query<DbRoleCreated>("uspRoleSet", param, commandType: CommandType.StoredProcedure)
                //        .First();

                //    if (dbRoleCreated.NewRoleId == 0)
                //    {
                //        dbRoleCreated.ErrorMessage = "Error Occured when try to create";
                //    }
                //}
                //catch (Exception ex)
                //{
                //    dbRoleCreated.ErrorMessage = ex.Message;
                //}
            }
           return dbRoleCreated;
        }

        public int InsertUpdateDeletePermissions(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest, int userId, int roleId)
        {
            int rowCount = 0;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RoleID", roleId);
                param.Add("@CreatorID", userId);
                param.Add("@ListData", JsonConvert.SerializeObject(roleCreateDeleteRequest.PermissionData));

                 rowCount = con.Query<int>("uspRolePermissionsSet", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return rowCount;
        }

        public int InsertUpdateDeleteFields(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest, int userId, int roleId)
        {
            int rowCount = 0;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RoleID", roleId);
                param.Add("@CreatorID", userId);
                param.Add("@ListData", JsonConvert.SerializeObject(roleCreateDeleteRequest.FieldData));

                rowCount = con.Query<int>("uspRoleFieldsSet", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return rowCount;
        }

        public int InsertUpdateDeleteNavLinks(RoleCreateUpdateDeleteRequest roleCreateDeleteRequest, int userId, int roleId)
        {
            int rowCount = 0;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@RoleID", roleId);
                param.Add("@CreatorID", userId);
                param.Add("@ListData", JsonConvert.SerializeObject(roleCreateDeleteRequest.NavLinkData));

                rowCount = con.Query<int>("uspRoleNavLinksSet", param, commandType: CommandType.StoredProcedure)
                    .First();
                con.Close();
            }
            return rowCount;
        }

        public List<DbRole> GetRolesForUser(int userId)
        {
            List<DbRole> Field = new List<DbRole>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID ", userId);
                Field = con.Query<DbRole>("uspUserRolesGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return Field;
        }

        public List<DbRole> GetAllRoles()
        {
            List<DbRole> Field = new List<DbRole>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                Field = con.Query<DbRole>("uspRolesGet", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return Field;
        }

        public List<DbSecurityType> GetTypeOptions()
        {
            var typeOptions = new List<DbSecurityType>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                typeOptions = con.Query<DbSecurityType>("uspSecurityTypeListGet",
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return typeOptions;
        }

        public List<DbObjectTypeSecurity> GetObjectTypeSecurityList()
        {
            var dbObjectTypeSecurities = new List<DbObjectTypeSecurity>();

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                dbObjectTypeSecurities = con
                    .Query<DbObjectTypeSecurity>("uspObjectTypeSecurityListGet",
                        commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return dbObjectTypeSecurities;
        }

        public List<DbFilterObject> GetFilterObjects()
        {
            var dbFilterObjects = new List<DbFilterObject>();

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                dbFilterObjects = con.Query<DbFilterObject>("uspFilterObjectListGet",
                    commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return dbFilterObjects;
        }

        public List<DbNavRole> GetUserNavigationRoles(int userId)
        {
            List<DbNavRole> dbNavRoles;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", userId);
                dbNavRoles = con.Query<DbNavRole>("uspNavigationRolesForUserGet", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return dbNavRoles;
        }

        public List<DbRoleTypeOption> GetRoleTypeOptions()
        {
            List<DbRoleTypeOption> dbRoleTypeOptions;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();

                dbRoleTypeOptions = con.Query<DbRoleTypeOption>("uspObjectTypeOptionsForRoleGet").ToList();

                con.Close();
            }
            return dbRoleTypeOptions;
        }

        public DbNavRole SetUserNavigationRole(UserNavigationRoleSetRequest navigationRoleSetRequest)
        {
            DbNavRole dbNavRole;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserID", navigationRoleSetRequest.UserID);
                param.Add("@RoleID", navigationRoleSetRequest.RoleID);
                param.Add("@IsDeleted", navigationRoleSetRequest.IsDeleted);
                param.Add("@UserRoleID", navigationRoleSetRequest.UserRoleID);
                param.Add("@Creator", UserHelper.GetUserId());

                dbNavRole = con.Query<DbNavRole>("uspNavigationRoleForUserSet", param, commandType: CommandType.StoredProcedure).First();
                con.Close();
            }
            return dbNavRole;
        }

        public DbRole SaveUserRole(UserRolesSaveRequest userRolesSaveRequest)
        {
            int userRoleId;
            DbRole dbRole;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var param = new DynamicParameters();
                param.Add("@UserRoleID", userRolesSaveRequest.UserRoleID);
                param.Add("@UserID", userRolesSaveRequest.UserID);
                param.Add("@RoleID", userRolesSaveRequest.RoleID);
                param.Add("@TypeSecurityID", userRolesSaveRequest.TypeSecurityID);
                param.Add("@FilterObjectID", userRolesSaveRequest.FilterObjectID);
                param.Add("@IsDeleted", userRolesSaveRequest.IsDeleted);
                param.Add("@CreatorID", UserHelper.GetUserId());
                param.Add("@ret", direction: ParameterDirection.ReturnValue);

                var res = con.Query<int>("uspUserRoleSet", param, commandType: CommandType.StoredProcedure);
                var errorId = param.Get<int>("@ret");
                if (errorId != 0)
                {
                    throw new GlobalApiException("Database error occured. Update Failed.");
                }
                userRoleId = res.First();

                if (userRolesSaveRequest.IsDeleted == 0)
                {
                    param = new DynamicParameters();
                    param.Add("@UserID", userRolesSaveRequest.UserID);
                    param.Add("@UserRoleID", userRoleId);

                    dbRole = con.Query<DbRole>("uspUserRolesGet", param, commandType: CommandType.StoredProcedure).First();
                }
                else
                {
                    dbRole = new DbRole
                    {
                        UserRoleID = userRoleId
                    };
                }
                
                con.Close();
            }
            return dbRole;
        }
    }
}
