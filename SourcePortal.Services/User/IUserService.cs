using Sourceportal.Domain.Models.API.Responses;
using Sourceportal.Domain.Models.API.Requests;
using System.Collections.Generic;

namespace SourcePortal.Services.User
{
    public interface IUserService
    {
        UserDetailsResponse UpdateUser(UserUpdateRequest userUpdateRequest);
        UserDetailsResponse GetUser(int userId);
        bool SetAccountStatus(int userId, bool isEnabled);
        bool ValidatePassword(string emailaddress, string password);
    }
}