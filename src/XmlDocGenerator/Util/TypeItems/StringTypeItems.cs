using System.Collections.Generic;
using XmlDocGenerator.YamlManager.YamlModels;

namespace XmlDocGenerator.Util.TypeItems
{
    public class StringTypeItems
    {
        public string FilePath { get; set; }

        public string AssemblyName { get; set; }

        public ICollection<AssemblyTypeModel> Types { get; set; }

        public ICollection<MethodModel> Methods { get; set; }

        public ICollection<FieldModel> Fields { get; set; }

        public ICollection<ConstructorModel> Constructors { get; set; }

        public ICollection<EventModel> Events { get; set; }

        public ICollection<PropertyModel> Properties { get; set; }

        public ICollection<NamespaceModel> Namespaces { get; set; }

        public ICollection<AssemblyTypeModel> Attributes { get; set; }

        public ICollection<AssemblyTypeModel> Delegates { get; set; }

        public StringTypeItems()
        {
            Types = new List<AssemblyTypeModel>();
            Methods = new List<MethodModel>();
            Fields = new List<FieldModel>();
            Constructors = new List<ConstructorModel>();
            Events = new List<EventModel>();
            Properties = new List<PropertyModel>();
            Namespaces = new List<NamespaceModel>();
            Attributes = new List<AssemblyTypeModel>();
            Delegates = new List<AssemblyTypeModel>();
        }
    }
}