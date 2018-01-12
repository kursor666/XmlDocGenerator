using System.Collections.Generic;

namespace XmlDocGenerator.YamlManager.YamlModels
{
    public class MethodConstructorBaseModel:LocalAssemblyTypeModel
    {
        public List<string> ArgsTypes { get; set; }

        public bool IsGeneric { get; set;  }

        public Dictionary<string, List<string>> Generics { get; set; }
    }
}