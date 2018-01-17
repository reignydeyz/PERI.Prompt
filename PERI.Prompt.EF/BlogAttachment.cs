using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class BlogAttachment
    {
        public int BlogId { get; set; }
        public int AttachmentId { get; set; }

        public Attachment Attachment { get; set; }
        public Blog Blog { get; set; }
    }
}
