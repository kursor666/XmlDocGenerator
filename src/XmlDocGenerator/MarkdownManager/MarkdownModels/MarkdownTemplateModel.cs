using System;
using System.Collections.Generic;
using System.Linq;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.Extensions;
using static System.String;

namespace XmlDocGenerator.MarkdownManager.MarkdownModels
{
    /// <summary>
    /// В данном пространстве имен лежит модель и вспомогательные модели для генерации
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc { }

    /// <summary>
    /// Общая модель, для шаблона
    /// </summary>
    public class MarkdownTemplateModel : ICloneable
    {
        private string _filePath;
        public string FilePath
        {
            get => FileNumber > 0
                ? _filePath.Insert(_filePath.IndexOf(".md", StringComparison.Ordinal), FileNumber.ToString())
                : _filePath;
            set => _filePath = value;
        }

        public int FileNumber { get; set; }

        public string FileLink =>
            !MdKey ? FilePath?.Remove(FilePath.IndexOf(".md", StringComparison.Ordinal)) : FilePath;

        public string RelativeKey => FilePath != null ? Enumerable.Repeat("../", FilePath.Count(c => c == '/'))
            .Aggregate("", (current, item) => current + item) : "";
        public bool HasRelativeKey => !IsNullOrEmpty(RelativeKey);

        public bool MdKey { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string XmlName { get; set; }

        public MarkdownTemplateModel Namespace { get; set; }
        public bool HasNamespace => Namespace != null;

        public string TypeForm { get; set; }

        public FieldConstValue ConstValue { get; set; }
        public bool HasConstValue => ConstValue != null;

        public string ParamName { get; set; }

        public string Module { get; set; }
        public bool HasModule => !IsNullOrEmpty(Module);

        public string Description { get; set; }
        public bool HasDescription => !IsNullOrEmpty(Description);

        public string ListDescription { get; set; }
        public bool HasListDescription => !IsNullOrEmpty(ListDescription);

        public string Value { get; set; }
        public bool HasValue => !IsNullOrEmpty(Value);

        public string Version { get; set; }
        public bool HasVersion =>
            !IsNullOrEmpty(Version);

        public bool IsNested { get; set; }

        public bool IsEnumElement { get; set; }

        public bool IsTargetType { get; set; }

        public string Example { get; set; }
        public bool HasExample => !IsNullOrEmpty(Example);

        public string Remarks { get; set; }
        public bool HasRemarks => !IsNullOrEmpty(Remarks);

        public MarkdownTemplateModel BaseClass { get; set; }

        public List<MarkdownTemplateModel> BaseInterfaces { get; set; }
        public bool HasBaseInterfaces =>
            BaseInterfaces != null
            && BaseInterfaces.Count != 0;

        public List<MarkdownTemplateModel> ConstsList { get; set; }
        public bool HasConsts => ConstsList != null && ConstsList.Count != 0;

        public List<MarkdownTemplateModel> EnumElements { get; set; }
        public bool HasEnumElements => EnumElements != null
            && EnumElements.Count != 0;

        public List<MarkdownTemplateModel> Seealso { get; set; }
        public bool HasSeealso =>
            Seealso != null
            && Seealso.Count != 0;

        public List<MarkdownTemplateModel> Exceptions { get; set; }
        public bool HasExceptions =>
            Exceptions != null
            && Exceptions.Count != 0;

        public List<MarkdownTemplateModel> Classes { get; set; }
        public bool HasClasses =>
            Classes != null
            && Classes.Count != 0;

        public List<MarkdownTemplateModel> Interfaces { get; set; }
        public bool HasInterfaces =>
            Interfaces != null
            && Interfaces.Count != 0;

        public List<MarkdownTemplateModel> Structs { get; set; }
        public bool HasStructs =>
            Structs != null
            && Structs.Count != 0;

        public List<MarkdownTemplateModel> Enums { get; set; }
        public bool HasEnums =>
            Enums != null
            && Enums.Count != 0;

        public List<MarkdownTemplateModel> Delegates { get; set; }
        public bool HasDelegates =>
            Delegates != null
            && Delegates.Count != 0;

        public List<MarkdownTemplateModel> Attributes { get; set; }
        public bool HasAttributes =>
            Attributes != null
            && Attributes.Count != 0;

        public List<MarkdownTemplateModel> InheritanceHierarchy { get; set; }
        public bool HasInheritance =>
            InheritanceHierarchy != null
            && InheritanceHierarchy.Count != 0;

        public List<MarkdownTemplateModel> Constructors { get; set; }
        public bool HasConstructors =>
            Constructors != null
            && Constructors.Count != 0;

        public List<MarkdownTemplateModel> Methods { get; set; }
        public bool HasMethods =>
            Methods != null
            && Methods.Count != 0;

        public List<MarkdownTemplateModel> Properties { get; set; }
        public bool HasProperties =>
            Properties != null
            && Properties.Count != 0;

        public List<MarkdownTemplateModel> Fields { get; set; }
        public bool HasFields =>
            Fields != null
            && Fields.Count != 0;

        public List<MarkdownTemplateModel> Events { get; set; }
        public bool HasEvents =>
            Events != null
            && Events.Count != 0;

        public List<MarkdownTemplateModel> NestedTypes { get; set; }
        public bool HasNestedTypes =>
            NestedTypes != null
            && NestedTypes.Count != 0;

        public List<MarkdownTemplateModel> ArgsTypes { get; set; }
        public bool HasArgsTypes =>
            ArgsTypes != null
            && ArgsTypes.Count != 0;

        public bool IsGeneric { get; set; }

        public MarkdownTemplateModel ReturnType { get; set; }
        public bool HasReturnType =>
            ReturnType != null;

        public List<MdGenericsModel> Generics { get; set; }
        public bool HasGenerics =>
            Generics != null
            && Generics.Count != 0;

        public MarkdownTemplateModel FieldType { get; set; }
        public bool HasFieldType =>
            FieldType != null;



        public object Clone() => MemberwiseClone();

        public MarkdownTemplateModel CloneCast =>
            Clone().Action<MarkdownTemplateModel>(model =>
            {
                model.FilePath = null;
            });
    }
}