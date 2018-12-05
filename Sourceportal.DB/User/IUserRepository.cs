using System.Collections.Generic;
using Sourceportal.Domain.Models.API.Requests;

namespace Sourceportal.DB.User
{
    public interface IUserRepository
    {
        Domain.Models.DB.User Update(UserUpdateRequest user);
        Domain.Models.DB.User GetUserData(int userId);
        bool SetAccountStatus(int profileId, bool isEnabled);
        int GetUserIdFromExternal(string externalId);
        bool ValidatePassword(string emailaddress, string password);
        Dictionary<int, string> GetUserNamesForIds(List<int> userIds);
    }
}