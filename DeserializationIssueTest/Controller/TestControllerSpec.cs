using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RA;

namespace DeserializationIssueTest.Controller
{
    [TestFixture]
    public class TestControllerSpec
    {
        private TestServer _server;

        private HttpClient Client { get; set; }

        [SetUp]
        public void Setup()
        {
            var webHostBuilder = GetWebHostBuilder();

            _server ??= new TestServer(webHostBuilder);
            Client ??= _server.CreateClient();
        }
        
        [Test]
        public async Task MustReturnIssueIfInputRunbookHasInvalidFormat()
        {
            new RestAssured()
                .Given()
                .HttpClient(Client)
                .Header("Content-Type", "application/json")
                .Body(await GetRequestBodyAsync("InvalidDto"))
                .When()
                .Post("http://localhost:5000/endpoint")
                .Then()
                .TestStatus("Bad Request", x => x.Equals(StatusCodes.Status400BadRequest))
                .TestBody("Body check", x =>
                {
                    var errors = (JObject) x["errors"];
                    var error = (JArray) errors?["innerDtos[0].name"];
                    var actualErrorCheck = error != null && error.Count == 1 && error.First().ToString().Equals("Unexpected character encountered while parsing value: [. Path 'innerDtos[0].name', line 5, position 21.");
                    return actualErrorCheck && errors.Count == 1; 
                    // TODO: there are also 3 other errors in here instead of just 1 including some infinity loop which should not be the case and test should pass
                })
                .Debug()
                .AssertAll();
        }
        
        private static IWebHostBuilder GetWebHostBuilder()
        {
            var conf = new ConfigurationBuilder().Build();
            return GetWebHostBuilder(conf);
        }

        private static IWebHostBuilder GetWebHostBuilder(IConfiguration configuration)
        {
            return new WebHostBuilder()
                .UseStartup<Startup>();
        }
        
        private async Task<string> GetRequestBodyAsync(string bodyName)
        {
            var resourceName = $"DeserializationIssueTest.Controller.RequestBody.{bodyName}.json";
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using (var reader = new StreamReader(resourceStream ?? throw new FileNotFoundException(), Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
