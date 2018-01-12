using System.Collections.Generic;

namespace XmlDocGenerator.YamlManager.YamlModels
{
    // BAD: в C# принято даже сокращения писать в верблюжьем стиле, т.е. не AssemblyTypeDTO, а AssemblyTypeDto
    //      кроме того, в данном случае DTO нелучший суффикс, больше подошло бы AssemblyTypeModel, так DTO используется для
    //      обмена данными между слоями или подсистемами
    public class AssemblyTypeModel : BaseTypeModel
    {
        public string MemberType { get; set; }

        public ICollection<string> SubTypes { get; set; }

        public string BaseType { get; set; }

        public ICollection<string> BaseInterfaces { get; set; }

        public ICollection<string> Methods { get; set; }

        public ICollection<string> Properties { get; set; }

        public ICollection<string> Fields { get; set; }

        public ICollection<string> Events { get; set; }

        public ICollection<string> Constructors { get; set; }

        public ICollection<string> NestedTypes { get; set; }

        public bool IsGeneric { get; set; }

        public bool IsNested { get; set; }

        public Dictionary<string, List<string>> Generics { get; set; }
    }
}