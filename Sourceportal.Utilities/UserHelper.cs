using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace Sourceportal.Utilities
{
    public class UserHelper
    {
        private static int MiddlewareUserId = 999999;
        private static string MiddlewareUsername = "MiddlewareUser";
        
        public static int GetUserId()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userIdString = identity.Claims.Where(c => c.Type == "userId").Select(c => c.Value).SingleOrDefault();
            var userId = userIdString != null ? int.Parse(userIdString) : 0;
            return userId;
        }
        public static string GetUserEmail()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var emailString = identity.Claims.Where(c => c.Type == "emailAddress").Select(c => c.Value).SingleOrDefault();
            return emailString != null ? emailString: "";
        }

        public static string GetUserFullName()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var nameString = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            return nameString != null? nameString : "";
        }
        public static int GetUserIdWhenCreateObject(int objectId)
        {
            if (IsNewObject(objectId))
            {
                return GetUserId();
            }

            return 0;
        }

        public static void SetMiddlewareUser()
        {
            GenericIdentity identity = new GenericIdentity(MiddlewareUsername);
            identity.AddClaim(new Claim("userId", MiddlewareUserId.ToString()));
           
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
        }

        public static bool IsMiddlewareUser()
        {
           return Thread.CurrentPrincipal.Identity != null &&  Thread.CurrentPrincipal.Identity.Name == MiddlewareUsername;
        }

        private static bool IsNewObject(int objectId)
        {
            return objectId == 0;
        }
    }
}