using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class TemplateSetting
    {
        public int SettingId { get; set; }
        public int TemplateId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Template Template { get; set; }
    }
}
