using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Gallery
    {
        public Gallery()
        {
            GalleryPhoto = new HashSet<GalleryPhoto>();
        }

        public int GalleryId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateInactive { get; set; }

        public ICollection<GalleryPhoto> GalleryPhoto { get; set; }
    }
}
