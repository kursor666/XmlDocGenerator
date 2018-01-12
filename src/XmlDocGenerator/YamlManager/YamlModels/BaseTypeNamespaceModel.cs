namespace XmlDocGenerator.YamlManager.YamlModels
{
    public class BaseTypeNamespaceModel
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public string Namespace { get; set; }

        public string XmlName { get; set; }

        public string Module { get; set; }

        public bool IsTargetType { get; set; }

        public string Description { get; set; }

        public string TypeForm { get; set; }

        public string Version { get; set; }
    }
}