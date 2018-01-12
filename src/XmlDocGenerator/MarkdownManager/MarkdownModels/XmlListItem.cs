using System.Collections.Generic;

namespace XmlDocGenerator.MarkdownManager.MarkdownModels
{
    public class XmlListItem
    {
        public List<string> Items { get; set; }
        public bool HasItems =>
            Items != null
            && Items.Count != 0;
    }
}