using XmlDocGenerator.Util;

namespace XmlDocGenerator.YamlManager.YamlModels
{
    public class FieldModel : FieldPropertyBaseModel
    {
        public bool IsEnumElement { get; set; }

        public FieldConstValue ConstValue { get; set; }
    }
}