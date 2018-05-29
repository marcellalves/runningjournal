using RunningJournal.Api;
using System;
using System.Net.Http;
using System.Web.Http.SelfHost;
using Xunit;

namespace RunningJournal.AcceptanceTests
{
    public class HomeJsonTests
    {
        [Fact]
        public void GetResponseReturnCorrectStatusCode()
        {
            var baseAddress = new Uri("http://localhost:9876");
            var config = new HttpSelfHostConfiguration(baseAddress);
            new Bootstrap().Configure(config);
            var server = new HttpSelfHostServer(config);
            using (var client = new HttpClient(server))
            {
                client.BaseAddress = baseAddress;

                var response = client.GetAsync("").Result;

                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }
    }
}
