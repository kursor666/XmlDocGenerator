using CommandLine;

namespace XmlDocGenerator.ProjectManager
{
    public class Options
    {
        [Option]
        public bool Verbose { get; set; }

        [Option("meta", Required = true)]
        public string MetaPath { get; set; }

    }
}