using System;
using System.Collections.Generic;

namespace PERI.Prompt.EF
{
    public partial class Setting
    {
        public int SettingId { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int Priority { get; set; }
        public bool Required { get; set; }
    }
}
