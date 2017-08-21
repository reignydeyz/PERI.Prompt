using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class GalleryPhoto
    {
        public int GalleryId { get; set; }
        public int PhotoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateInactive { get; set; }

        public Gallery Gallery { get; set; }
        public Photo Photo { get; set; }
    }
}
