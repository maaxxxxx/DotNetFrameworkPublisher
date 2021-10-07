using Man.Dapr.Sidekick;
using Man.Dapr.Sidekick.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotNetFrameworkPublisher
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static DaprSidekick Sidekick { get; private set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Stop()
        {
            // Shut down the Dapr Sidecar - this is a blocking call
            Sidekick.Sidecar.Stop(DaprCancellationToken.None);
        }
    }
}
