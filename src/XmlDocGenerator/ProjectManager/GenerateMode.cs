using CommandLine;

namespace XmlDocGenerator.ProjectManager
{
    [Verb("gen", HelpText = "Md Doc generate")]
    public class GenerateMode : Options
    {
        [Option("out", Required = true)]
        public string OutPath { get; set; }

        [Option("template", Required = true)]
        public string TemplatesPath { get; set; }

        [Option("clear")]
        public bool Clear { get; set; }

        [Option("regen")]
        public bool Reneg { get; set; }

        [Option("md", Default = false)]
        public bool MdKey { get; set; }
    }
}