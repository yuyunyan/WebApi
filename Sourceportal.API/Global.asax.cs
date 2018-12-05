using System.Linq;
using System.Web.Http;
using Sourceportal.API.DependencyResolution;
using WebApi.StructureMap;

namespace Sourceportal.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.UseStructureMap(x =>
            {
                x.AddRegistry<DefaultRegistry>();
            });

            GlobalConfiguration.Configure(WebApiConfig.Register);
            Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(GlobalConfiguration.Configuration);
        }

        protected void Application_BeginRequest()
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.Flush();
                Response.End();
            }
        }
    }
}
