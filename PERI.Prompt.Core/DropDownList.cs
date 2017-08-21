using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Prompt.Core
{
    public class DropDownList
    {
        public static SelectList BooleanList(string trueText, string falseText)
        {
            List<BooleanItem> ddItem = new List<BooleanItem>();
            ddItem.Add(new BooleanItem(trueText, true));
            ddItem.Add(new BooleanItem(falseText, false));
            return new SelectList(ddItem, "Value", "Text");
        }

        /// <summary>
        /// Boolean item (True/False)
        /// </summary>
        public class BooleanItem
        {
            public BooleanItem(string text, bool value)
            {
                this.Text = text;
                this.Value = value;
            }
            public string Text { get; set; }
            public bool Value { get; set; }
        }

        /// <summary> (string)
        /// Default item
        /// </summary>
        public class Item
        {
            public Item(string text, string value)
            {
                this.Text = text;
                this.Value = value;
            }
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }
}
