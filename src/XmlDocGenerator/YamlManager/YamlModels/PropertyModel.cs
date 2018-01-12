namespace XmlDocGenerator.YamlManager.YamlModels
{
    public class PropertyModel : FieldPropertyBaseModel
    {
        public bool IsSetter { get; set; }

        public bool IsGetter { get; set; }

        public bool IsVirtual { get; set; }
    }
}