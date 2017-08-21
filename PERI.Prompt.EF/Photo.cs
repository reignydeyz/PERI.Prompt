using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Photo
    {
        public Photo()
        {
            BlogPhoto = new HashSet<BlogPhoto>();
            GalleryPhoto = new HashSet<GalleryPhoto>();
            PagePhoto = new HashSet<PagePhoto>();
            SectionItemPhoto = new HashSet<SectionItemPhoto>();
        }

        public int PhotoId { get; set; }
        public string Url { get; set; }

        public ICollection<BlogPhoto> BlogPhoto { get; set; }
        public ICollection<GalleryPhoto> GalleryPhoto { get; set; }
        public ICollection<PagePhoto> PagePhoto { get; set; }
        public ICollection<SectionItemPhoto> SectionItemPhoto { get; set; }
    }
}
