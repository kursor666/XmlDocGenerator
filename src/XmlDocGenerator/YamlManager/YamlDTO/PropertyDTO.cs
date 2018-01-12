namespace XmlDocGenerator.YamlManager.YamlDTO
{
    public class PropertyDTO : FieldPropertyBaseDTO
    {
        public bool IsSetter { get; set; }

        public bool IsGetter { get; set; }

        public bool IsVirtual { get; set; }
    }
}