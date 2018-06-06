using System;

namespace RunningJournal.Api.Models
{
    public class JournalEntryModel
    {
        public DateTimeOffset Time { get; set; }

        public int Distance { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
