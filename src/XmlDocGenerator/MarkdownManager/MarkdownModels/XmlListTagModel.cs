using System.Collections.Generic;

namespace XmlDocGenerator.MarkdownManager.MarkdownModels
{
    public class XmlListTagModel
    {
        public bool IsBullet { get; set; } = false;
        public bool IsTable { get; set; } = false;
        public bool IsNumber { get; set; } = false;

        public List<string> HeaderList { get; set; }
        public bool HasHeader =>
            HeaderList != null
            && HeaderList.Count != 0;

        public List<XmlListItem> ItemsList { get; set; }
        public bool HasItems =>
            ItemsList != null
            && ItemsList.Count != 0;
    }
}