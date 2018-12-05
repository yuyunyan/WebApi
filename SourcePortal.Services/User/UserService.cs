using System.Collections.Generic;
using Sourceportal.DB.User;
using Sourceportal.Domain.Models.API.Requests;
using Sourceportal.Domain.Models.API.Responses;
namespace SourcePortal.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDetailsResponse UpdateUser(UserUpdateRequest userUpdateRequest)
        {
            Sourceportal.Domain.Models.DB.User dbUser = _userRepository.Update(userUpdateRequest);
            return createResponse(dbUser);
        }

        public UserDetailsResponse GetUser(int userId)
        {
            Sourceportal.Domain.Models.DB.User dbUser = _userRepository.GetUserData(userId);
            return createResponse(dbUser);
        }

        public bool SetAccountStatus(int userId, bool isEnabled)
        {
            return _userRepository.SetAccountStatus(userId, isEnabled);
        }
    
        public bool ValidatePassword(string emailaddress, string password)
        {
            return _userRepository.ValidatePassword(emailaddress, password);
        }
        private static UserRoleResponse CreateRole(Sourceportal.Domain.Models.DB.UserRole dbrole)
        {
            UserRoleResponse apirole = new UserRoleResponse();
            apirole.RoleID = dbrole.RoleID;
            apirole.RoleName = dbrole.RoleName;
            apirole.Type = System.Enum.GetName(typeof(Sourceportal.DB.Enum.ObjectType), dbrole.ObjectTypeID);
            return apirole;
        }
        
        private static UserDetailsResponse createResponse(Sourceportal.Domain.Models.DB.User dbuser)
        {
            UserDetailsResponse apiuser = new UserDetailsResponse();

            if (!string.IsNullOrEmpty(dbuser.Error))
            {
                apiuser.ErrorMessage = dbuser.Error;
                return apiuser;
            }

            apiuser.UserID = dbuser.UserID;
            apiuser.EmailAddress = dbuser.EmailAddress;
            apiuser.FirstName = dbuser.FirstName;
            apiuser.LastName = dbuser.LastName;
            apiuser.PhoneNumber = dbuser.PhoneNumber;
            apiuser.OrganizationID = dbuser.OrganizationID;
            apiuser.TimezoneName = dbuser.TimezoneName;
            apiuser.IsEnabled = dbuser.IsEnabled;

            apiuser.dateCreated = dbuser.dateCreated;
            apiuser.dateLastLogin = dbuser.dateLastLogin;
            apiuser.ErrorMessage = dbuser.Error;
            apiuser.Success = dbuser.IsSuccessful;
            return apiuser;
        }

        public static List<UserDetailsResponse> APIUserList(List<Sourceportal.Domain.Models.DB.User> dbuserlist)
        {
            List<Sourceportal.Domain.Models.API.Responses.UserDetailsResponse> apiuserlist = new List<Sourceportal.Domain.Models.API.Responses.UserDetailsResponse>();

            foreach(Sourceportal.Domain.Models.DB.User dbuser in dbuserlist)
            {
                Sourceportal.Domain.Models.API.Responses.UserDetailsResponse apiuser = new Sourceportal.Domain.Models.API.Responses.UserDetailsResponse();

                apiuser.UserID = dbuser.UserID;
                apiuser.EmailAddress = dbuser.EmailAddress;
                apiuser.FirstName = dbuser.FirstName;
                apiuser.LastName = dbuser.LastName;
                apiuser.OrganizationID = dbuser.OrganizationID;
                apiuser.IsEnabled = dbuser.IsEnabled;
                apiuser.organizationName = dbuser.OrganizationName;

                apiuser.dateCreated = dbuser.dateCreated;
                apiuser.dateLastLogin = dbuser.dateLastLogin;

                apiuserlist.Add(apiuser);

            }
            return apiuserlist;
        }
        public static Response<List<UserRoleResponse>> ApiUserRoleList(List<Sourceportal.Domain.Models.DB.UserRole> dbrolelist)
        {
            List<UserRoleResponse> apirolelist = new List<UserRoleResponse>();
            var response = new Response<List<UserRoleResponse>>();

            foreach (Sourceportal.Domain.Models.DB.UserRole dbrole in dbrolelist)
            {
                var apiRole = new UserRoleResponse();
                apiRole.RoleID = dbrole.RoleID;
                apiRole.RoleName = dbrole.RoleName;
                apiRole.Type = dbrole.ObjectName;
                apiRole.ObjectTypeID = dbrole.ObjectTypeID;
                apirolelist.Add(apiRole);

            }
            
            response.Data = apirolelist;
            response.IsSuccess = true;
            return response;


        }

    }
}
