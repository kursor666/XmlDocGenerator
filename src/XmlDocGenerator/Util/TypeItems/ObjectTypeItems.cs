using System.Collections.Generic;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes;

namespace XmlDocGenerator.Util.TypeItems
{
    // BAD: суффикс Hash понимается как значение хеша ObjectTypesHash читается буквально как
    //      хеш для объектных типов. это сильно путает при чтении кода, нужно называть так,
    //      чтобы отражало суть, например: XdgAssemblyStructureItems
    public class ObjectTypeItems
    {
        public string FilePath { get; set; }

        public string AssemblyName { get; set; }

        // BAD: наружу всегда должны торчать интерфейсы коллекций, в данном случае IDictionary, а ещё лучше IReadOnlyDictionary
        public IDictionary<string, AssemblyType> Types { get; set; }

        public IDictionary<string, Method> Methods { get; }

        public IDictionary<string, Field> Fields { get; }

        public IDictionary<string, Constructor> Constructors { get; }

        public IDictionary<string, Event> Events { get; }

        public IDictionary<string, Property> Properties { get; }

        public IDictionary<string, AssemblyNamespace> Namespaces { get; set; }

        public IDictionary<string, AssemblyType> Attributes { get; set; }

        public IDictionary<string, AssemblyType> Delegates { get; set; }

        public ObjectTypeItems()
        {
            Types = new Dictionary<string, AssemblyType>();
            Methods = new Dictionary<string, Method>();
            Fields = new Dictionary<string, Field>();
            Constructors = new Dictionary<string, Constructor>();
            Events = new Dictionary<string, Event>();
            Properties = new Dictionary<string, Property>();
            Namespaces = new Dictionary<string, AssemblyNamespace>();
            Delegates = new Dictionary<string, AssemblyType>();
            Attributes = new Dictionary<string, AssemblyType>();
        }

    }
}