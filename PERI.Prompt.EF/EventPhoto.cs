using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class EventPhoto
    {
        public int EventId { get; set; }
        public int PhotoId { get; set; }

        public Event Event { get; set; }
        public Photo Photo { get; set; }
    }
}
