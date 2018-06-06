using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using Xunit;

namespace RunningJournal.AcceptanceTests
{
    public class HomeJsonTests
    {
        [Fact]
        public void GetResponseReturnCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        public void PostReturnsResponseWithCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time = DateTimeOffset.Now,
                    distance = 8500,
                    duration = TimeSpan.FromMinutes(44)
                };

                var response = client.PostAsJsonAsync("", json).Result;

                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        public void GetAfterPostReturnsResponseWithPostedEntry()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time = DateTimeOffset.Now,
                    distance = 8100,
                    duration = TimeSpan.FromMinutes(41)
                };
                var expected = JObject.FromObject(json);
                client.PostAsJsonAsync("", json).Wait();

                var response = client.GetAsync("").Result;

                var actual = response.Content.ReadAsAsync<JObject>().Result;

                Assert.Contains(expected, (JArray)actual["entries"]);
            }
        }
    }
}
