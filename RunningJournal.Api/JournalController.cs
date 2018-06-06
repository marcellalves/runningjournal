using RunningJournal.Api.Models;
using System.Collections.Generic;
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
            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                new JournalModel
                {
                    Entries = entries.ToArray()
                });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            entries.Add(journalEntry);
            return this.Request.CreateResponse();
        }
    }
}
