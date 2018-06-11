using Newtonsoft.Json.Linq;
using Simple.Data;
using System;
using System.Configuration;
using System.Dynamic;
using System.Net.Http;
using Xunit;

namespace RunningJournal.AcceptanceTests
{
    public class HomeJsonTests
    {
        [Fact]
        [UseDatabase]
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
        [UseDatabase]
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
        [UseDatabase]
        public void AfterPostingEntryGetRootReturnsEntryInContent()
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

        [Fact]
        [UseDatabase]
        public void GetRootReturnsCorrectEntryFromDatabase()
        {
            dynamic entry = new ExpandoObject();
            entry.time = DateTimeOffset.Now;
            entry.distance = 6000;
            entry.duration = TimeSpan.FromMinutes(31);

            var expected = JObject.FromObject((object)entry);

            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);
            var userId = db.User.Insert(UserName: "foo").UserId;
            entry.UserId = userId;
            db.JournalEntry.Insert(entry);

            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                var actual = response.Content.ReadAsAsync<JObject>().Result;

                Assert.Contains(expected, (JArray)actual["entries"]);
            }
        }
    }
}
