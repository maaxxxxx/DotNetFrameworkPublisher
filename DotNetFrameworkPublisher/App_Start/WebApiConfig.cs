using Man.Dapr.Sidekick;
using Man.Dapr.Sidekick.Threading;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace DotNetFrameworkPublisher
{
    public static class WebApiConfig
    {
        public static DaprSidekick Sidekick { get; private set; }
        public static void Register(HttpConfiguration config)
        {
            // Set options in code
            var options = new DaprOptions
            {
                LogLevel = "debug",
                
                Sidecar = new DaprSidecarOptions
                {
                    AppPort = 58717,
                    AppId = "dotnetframeworkpublisher",
                    LogLevel = "debug",
                    AppProtocol = "http"
                }
            };

            // Build the default Sidekick controller
            Sidekick = new DaprSidekickBuilder().Build();

            // Start the Dapr Sidecar, this will come up in the background
            Sidekick.Sidecar.Start(() => options, DaprCancellationToken.None);

            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Debug;
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("xml", "true", "application/xml"));
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
