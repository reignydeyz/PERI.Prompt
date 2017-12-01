using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Event
    {
        public Event()
        {
            EventPhoto = new HashSet<EventPhoto>();
        }

        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Time { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateInactive { get; set; }

        public ICollection<EventPhoto> EventPhoto { get; set; }
    }
}
