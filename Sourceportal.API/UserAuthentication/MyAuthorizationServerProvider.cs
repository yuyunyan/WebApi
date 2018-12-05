
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Sourceportal.DB.User;

namespace Sourceportal.API.UserAuthentication
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            ///Todo ///
            context.Validated(); // 
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = UserRepository.Login(context.UserName, context.Password);

            if (user == null || !string.IsNullOrEmpty(user.Error))
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
            }
            else
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName));
                identity.AddClaim(new Claim("userId", user.UserID.ToString()));
                identity.AddClaim(new Claim("emailAddress", user.EmailAddress));
                //identity.AddClaim(context.Options.AccessTokenExpireTimeSpan);
                context.Validated(identity);
            }
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            string thisIsTheToken = context.AccessToken;
            //add user Id and status as additional response parameter
            if (context.Identity != null && context.Identity.Name != null)
            {
                context.AdditionalResponseParameters.Add("displayusername", context.Identity.Name);
                var userIdClaim = context.Identity.Claims.FirstOrDefault(x => x.Type == "userId");
                if (userIdClaim != null)
                {
                    context.AdditionalResponseParameters.Add("userId", int.Parse(userIdClaim.Value));
                    //context.AdditionalResponseParameters.Add("expiryTime", context.Identity.Claims());
                }
            }
            return base.TokenEndpointResponse(context);
        }
    }
}