using CommandLine;
using XmlDocGenerator.ProjectManager;

namespace XmlDocGenerator
{
    class Program
    {
        static int Main(string[] args)
        {
            var proj = new ProjectController();
            return Parser.Default.ParseArguments<ScanMode, GenerateMode>(args)
                .MapResult(
                    (GenerateMode opts) => proj.Generate(opts),
                    (ScanMode opts) => proj.Scan(opts),
                    errs => 1);
        }
    }
}


