using Man.Dapr.Sidekick;
using Newtonsoft.Json;
using Publisher.Controllers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace DotNetFrameworkPublisher.Controllers
{
    public class PublisherController : ApiController
    {
        private DaprSidekick Sidekick => WebApiConfig.Sidekick;

        [HttpGet]
        [Route("publisher/CreateOrder")]
        public async Task CreateOrder()
        {
            // Get a Dapr Service Invocation http client for this service's AppId.
            // This will perform a local round-trip call to the sidecar and back
            // to this controller to demonstrate service invocation in action.
            var appId = Sidekick.Sidecar.GetProcessOptions()?.AppId;
            if (appId != null)
            {
                // Create an HttpClient for this service appId
                var httpClient = Sidekick.HttpClientFactory.CreateInvokeHttpClient(appId);

                //first attempt with GET
                var result = await httpClient.GetStringAsync("/publisher/test");

                //second attempt with POST
                string json = JsonConvert.SerializeObject(new OrderData());
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var result2 = await httpClient.PostAsync("/publisher/post", httpContent);
            }
        }

        [HttpGet]
        [Route("publisher/test")]
        public IHttpActionResult Test()
        {
            return Ok("SUCCESS");
        }

        [HttpPost]
        [Route("publisher/post")]
        public IHttpActionResult Post(OrderData data)
        {
            return Ok(data);
        }
    }
}
