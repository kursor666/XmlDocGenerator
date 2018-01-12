using System;
using System.IO;
using System.Linq;
using HandlebarsDotNet;
using XmlDocGenerator.MarkdownManager.MarkdownModels;
using XmlDocGenerator.ProjectManager;
using static System.String;

namespace XmlDocGenerator.MarkdownManager
{
    public class MarkdownTemplate
    {
        private Func<object, string> _compiledTemplate;

        private Func<object, string> _compiledIndexTemplate;

        private readonly GenerateMode _generateMode;

        public MarkdownTemplate(GenerateMode generateMode)
        {
            _generateMode = generateMode;
            CreateTemplate();
        }

        public string CreateMdText(MarkdownTemplateModel model) => _compiledTemplate(model);

        public string CreateMdIndexPage(NamepacesIndexPageModel model) => _compiledIndexTemplate(model);

        public string CurrentRelativeKey { get; set; }

        public void CreateTemplate()
        {
            var linkPartial = File.ReadAllText($@"{_generateMode.TemplatesPath}\Partials\link.txt");
            Handlebars.RegisterTemplate("link", linkPartial);

            var enumElementsPartial = File.ReadAllText($@"{_generateMode.TemplatesPath}\Partials\enumElement.txt");
            Handlebars.RegisterTemplate("enumElement", enumElementsPartial);

            var constElementsPartial = File.ReadAllText($@"{_generateMode.TemplatesPath}\Partials\constElement.txt");
            Handlebars.RegisterTemplate("constElement", constElementsPartial);

            var stringIndexTemplate = File.ReadAllText($@"{_generateMode.TemplatesPath}\Index.txt");
            _compiledIndexTemplate= Handlebars.Compile(stringIndexTemplate);

            var namespaceTemplate = File.ReadAllText($@"{_generateMode.TemplatesPath}\Partials\namespace.txt");
            Handlebars.RegisterTemplate("namespace", namespaceTemplate);

            Handlebars.RegisterHelper("relativeKey", (writer, context, arguments) =>
            {
                if (!IsNullOrEmpty(CurrentRelativeKey))
                    writer.WriteSafeString(CurrentRelativeKey);
            });

            var stringTemplate = File.ReadAllText($@"{_generateMode.TemplatesPath}\Main.txt");
            _compiledTemplate = Handlebars.Compile(stringTemplate);
        }


    }
}