using System.Collections.Generic;

namespace XmlDocGenerator.YamlManager.YamlModels
{
    public class BaseTypeModel : BaseTypeNamespaceModel
    {
        public ICollection<string> Attributes { get; set; }
    }
}