using System.Collections.Generic;

namespace XmlDocGenerator.MarkdownManager.MarkdownModels
{
    public class MdGenericsModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<MarkdownTemplateModel> TypesList { get; set; }
        public bool HasConstraintTypes =>
            TypesList != null
            && TypesList.Count != 0;
    }
}