using CommandLine;

namespace XmlDocGenerator.ProjectManager
{
    [Verb("scan", HelpText = "Dll scan")]
    public class ScanMode : Options
    {
        [Option("dll", Required = true)]
        public string DllPath { get; set; }

    }
}