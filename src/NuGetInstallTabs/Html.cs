using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace NuGetInstallTabs
{
    public class Html
    {
        private readonly HttpClient _client;

        public Html(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(Html))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{package}")] HttpRequest req,
            ILogger log,
            string package)
        {
            log.LogInformation($"Render HTML for {package}");

            if (package == null) return new BadRequestObjectResult("Invalid package");

            var uri = $"https://api.nuget.org/v3-flatcontainer/{package}/index.json";
            var response = await _client.GetAsync(uri);

            if (!response.IsSuccessStatusCode) return new NotFoundObjectResult("Package not found");

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var version = json["versions"].Last.Value<string>();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith("template.html"));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var template = reader.ReadToEnd();
                var result = template
                    .Replace("{{package}}", package)
                    .Replace("{{version}}", version);

                return new ContentResult { Content = result, ContentType = "text/html; charset=utf-8", StatusCode = 200 };
            }
        }
    }
}
