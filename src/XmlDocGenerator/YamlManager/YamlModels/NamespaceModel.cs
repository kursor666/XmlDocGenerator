using System.Collections.Generic;

namespace XmlDocGenerator.YamlManager.YamlModels
{
    public class NamespaceModel : BaseTypeNamespaceModel
    {
        public ICollection<string> Classes { get; set; }

        public ICollection<string> Structs { get; set; }

        public ICollection<string> Enums { get; set; }

        public ICollection<string> Interfaces { get; set; }

        public ICollection<string> Delegates { get; set; }

        public ICollection<string> Attributes { get; set; }
    }
}