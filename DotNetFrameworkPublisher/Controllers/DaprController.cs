using Man.Dapr.Sidekick;
using Man.Dapr.Sidekick.Process;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DotNetFrameworkPublisher.Controllers
{
    public class DaprController : ApiController
    {
        private DaprSidekick Sidekick => WebApiConfig.Sidekick;

        [HttpGet]
        [Route("api/DaprHealth")]
        public async Task<DaprHealthResult> GetDaprHealth()
        {
            return await Sidekick.Sidecar.GetHealthAsync(CancellationToken.None);
        }


        [HttpGet]
        [Route("api/DaprStatus")]
        public IHttpActionResult GetDaprStatus()
        {
            return Ok(new
            {
                process = Sidekick.Sidecar.GetProcessInfo(),   // Information about the sidecar process such as if it is running
                options = Sidekick.Sidecar.GetProcessOptions() // The sidecar options if running, including ports and locations
            });
        }

        [HttpGet]
        [Route("api/DaprMetrics")]
        public async Task<HttpResponseMessage> GetDaprMetrics()
        {
            var ms = new MemoryStream();
            await Sidekick.Sidecar.WriteMetricsAsync(ms, CancellationToken.None);
            ms.Position = 0;
            var text = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(text);
            return resp;
        }

        [HttpGet]
        [Route("api/DaprMetadata")]
        public async Task<HttpResponseMessage> GetDaprMetadata()
        {
            var ms = new MemoryStream();
            await Sidekick.Sidecar.WriteMetadataAsync(ms, CancellationToken.None);
            ms.Position = 0;
            var text = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(text);
            return resp;
        }

    }
}
