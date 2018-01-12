using System.Collections.Generic;
using System.Reflection;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo
{
    public class AssemblyNamespace:BaseAssemblyNamespaceType
    {
        public ICollection<AssemblyType> Classes { get; set; }

        public ICollection<AssemblyType> Interfaces { get; set; }

        public ICollection<AssemblyType> Structs { get; set; }

        public ICollection<AssemblyType> Enums { get; set; }

        public ICollection<AssemblyType> Delegates { get; set; }

        public ICollection<AssemblyType> Attributes { get; set; }

        public bool IsGroupDoc { get; set; }

        public AssemblyNamespace(Assembly assembly) : base(assembly)
        {
            Form = "Namespace";
            Classes = new List<AssemblyType>();
            Interfaces = new List<AssemblyType>();
            Structs = new List<AssemblyType>();
            Enums = new List<AssemblyType>();
            Delegates = new List<AssemblyType>();
            Attributes = new List<AssemblyType>();
            Name = Namespace;
            FullName = $"{Name}";
        }

        public override string ToString()
        {
            return Namespace;
        }
    }
}