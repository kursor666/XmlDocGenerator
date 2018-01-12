using System.Collections.Generic;
using XmlDocGenerator.YamlManager.YamlDTO;

namespace XmlDocGenerator.Util
{
    // BAD: суффикс Hash понимается как значение хеша StringTypesHash читается буквально как
    //      хеш для строковых типов. это сильно путает при чтении кода, нужно называть так,
    //      чтобы отражало суть, например: XdgAssemblyStructureModels
    public class StringTypesHash
    {
        public string FilePath { get; set; }

        public string AssemblyName { get; set; }

        public ICollection<AssemblyTypeDTO> Types { get; set; }

        public ICollection<MethodDTO> Methods { get; set; }

        public ICollection<FieldDTO> Fields { get; set; }

        public ICollection<ConstructorDTO> Constructors { get; set; }

        public ICollection<EventDTO> Events { get; set; }

        public ICollection<PropertyDTO> Properties { get; set; }

        public ICollection<NamespaceDTO> Namespaces { get; set; }

        public StringTypesHash()
        {
            
        }
    }
}