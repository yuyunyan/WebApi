using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Services.Description;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Sourceportal.API.Models;
using Sourceportal.API.UserAuthentication;
using Sourceportal.DB.ErrorManagementService;
using Sourceportal.Domain.Models.Services.ErrorManagement;
using SourcePortal.Services.ErrorManagement;
using StructureMap;

namespace Sourceportal.API
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        //public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
           // PublicClientId = "self";
            var myProvider = new MyAuthorizationServerProvider();
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = myProvider,
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});

            app.Use(async (ctx, next) =>
            {
                await next();
                if ((ctx.Response.StatusCode == (int)HttpStatusCode.BadRequest) && ctx.Response.ReasonPhrase == "Bad Request")
                {
                    var container = new Container(x =>
                    {
                        x.For<IErrorManagementService>().Use<ErrorManagementService>();
                        x.For<IErrorManagementRepository>().Use<ErrorManagementRepository>();
                    });
                    var exc = new HttpRequestValidationException("The request is invalid.");
                    var request = new HttpRequestMessage {RequestUri = ctx.Request.Uri};
                    var errorManagementService = container.GetInstance<IErrorManagementService>();
                    errorManagementService.LoggingError(new ExceptionDTO{Exception = exc, Request = request});
                }
            });
        }
    }
}
