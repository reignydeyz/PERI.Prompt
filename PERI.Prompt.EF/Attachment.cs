using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Attachment
    {
        public Attachment()
        {
            BlogAttachment = new HashSet<BlogAttachment>();
        }

        public int AttachmentId { get; set; }
        public string Url { get; set; }

        public ICollection<BlogAttachment> BlogAttachment { get; set; }
    }
}
