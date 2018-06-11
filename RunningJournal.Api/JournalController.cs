using RunningJournal.Api.Models;
using Simple.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RunningJournal.Api
{
    public class JournalController : ApiController
    {
        private readonly static List<JournalEntryModel> entries = new List<JournalEntryModel>();

        public HttpResponseMessage Get()
        {
            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);

            var entries = db.JournalEntry
                .FindAll(db.JournalEntry.User.UserName == "foo")
                .ToArray<JournalEntryModel>();

            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                new JournalModel
                {
                    Entries = entries
                });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);

            var userId = db.User.Insert(UserName: "foo").UserId;

            db.JournalEntry.Insert(
                UserId: userId,
                Time: journalEntry.Time,
                Distance: journalEntry.Distance,
                Duration: journalEntry.Duration
                );

            return this.Request.CreateResponse();
        }
    }
}
