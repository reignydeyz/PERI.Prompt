using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Template
    {
        public Template()
        {
            Section = new HashSet<Section>();
            TemplateSetting = new HashSet<TemplateSetting>();
        }

        public int TemplateId { get; set; }
        public string Name { get; set; }
        public DateTime? DateInactive { get; set; }

        public ICollection<Section> Section { get; set; }
        public ICollection<TemplateSetting> TemplateSetting { get; set; }
    }
}
