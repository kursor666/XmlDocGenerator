using System.Collections.Generic;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes;

namespace XmlDocGenerator.Util
{
    // BAD: суффикс Hash понимается как значение хеша ObjectTypesHash читается буквально как
    //      хеш для объектных типов. это сильно путает при чтении кода, нужно называть так,
    //      чтобы отражало суть, например: XdgAssemblyStructureItems
    public class ObjectTypesHash
    {
        public string FilePath { get; set; }

        public string AssemblyName { get; set; }

        // BAD: наружу всегда должны торчать интерфейсы коллекций, в данном случае IDictionary, а ещё лучше IReadOnlyDictionary
        public Dictionary<string, AssemblyType> Types { get; set; }

        public Dictionary<string, Method> Methods { get; }

        public Dictionary<string, Field> Fields { get; }

        public Dictionary<string, Constructor> Constructors { get; }

        public Dictionary<string, Event> Events { get; }

        public Dictionary<string, Property> Properties { get; }

        public Dictionary<string, AssemblyNamespace> Namespaces { get; set; }

        public ObjectTypesHash()
        {
            Types = new Dictionary<string, AssemblyType>();
            Methods = new Dictionary<string, Method>();
            Fields = new Dictionary<string, Field>();
            Constructors = new Dictionary<string, Constructor>();
            Events = new Dictionary<string, Event>();
            Properties = new Dictionary<string, Property>();
            Namespaces = new Dictionary<string, AssemblyNamespace>();
        }

    }
}