using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using HandlebarsDotNet;
using XmlDocGenerator.MarkdownManager.MarkdownModels;
using XmlDocGenerator.ProjectManager;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.Extensions;
using static System.String;

namespace XmlDocGenerator.XmlManager
{
    public class XmlController : BaseController
    {
        private Dictionary<string, Func<object, string>> _compiledTemplates;

        private MarkdownTemplateModel _model;

        private List<MarkdownTemplateModel> _models;

        private readonly GenerateMode _generateMode;

        private string _currentRelativeKey;

        public XmlController(GenerateMode generateMode)
        {
            _generateMode = generateMode;
            _compiledTemplates = new Dictionary<string, Func<object, string>>();
        }

        public List<MarkdownTemplateModel> GetElements(List<MarkdownTemplateModel> models)
        {
            try
            {
                _models = models;
                foreach (var templateModel in _models
                    .Where(model => model.Description != null)
                    .OrderBy(model => model.RelativeKey)
                    .ToList())
                {
                    if (templateModel.RelativeKey != _currentRelativeKey)
                    {
                        Console.WriteLine(_currentRelativeKey);
                        _currentRelativeKey = templateModel.RelativeKey;
                        RegisterTemplates();
                    }
                    _model = templateModel;
                    var xmlParsedDescription = XElement.Parse(_model.Description);
                    _model.Description = null;
                    GetValue(xmlParsedDescription);
                }
                return _models;
            }
            catch (Exception e)
            {
                Write(e.Message);
                throw;
            }
        }

        private void RegisterTemplates()
        {
            var files = new List<string>();
            var folderPath = $@"{_generateMode.TemplatesPath}\Partials";
            Handlebars.RegisterHelper("relativeKey", (writer, context, arguments) =>
            {
                if (_currentRelativeKey != null)
                    writer.WriteSafeString(_currentRelativeKey);
            });
            _compiledTemplates.Clear();
            if (!Directory.Exists(folderPath))
                throw new FileNotFoundException();
            files.AddRange(Directory.GetFiles(folderPath, "*.txt"));
            foreach (var filePath in files)
            {
                var fileText = File.ReadAllText(filePath);
                _compiledTemplates.Add(Path.GetFileNameWithoutExtension(filePath), Handlebars.Compile(fileText));
            }
        }


        private void GetValue(XElement targetElement)
        {
            foreach (var xElement in targetElement.Elements().ToList())
            {
                if (xElement.Name.LocalName == "list")
                {
                    _model.ListDescription = GetListTag(xElement);
                    xElement.Remove();
                }
                else if (xElement.HasElements)
                    GetValue(xElement);
                switch (xElement.Name.LocalName)
                {
                    case "summary":
                        if (_model.Description == null)
                            _model.Description = xElement.Value.Trim();
                        else _model.Description += xElement.Value.Trim();
                        xElement.Remove();
                        break;
                    case "returns":
                        _model.ReturnType.Description = xElement.Value;
                        xElement.Remove();
                        break;
                    case "para":
                        xElement.ReplaceWith(_compiledTemplates["para"](new { value = xElement.Value }));
                        break;
                    case "c":
                        xElement.ReplaceWith(_compiledTemplates["c"](new { value = xElement.Value }));
                        break;
                    case "code":
                        xElement.ReplaceWith(_compiledTemplates["code"](new { value = xElement.Value }));
                        break;
                    case "example":
                        _model.Example = xElement.Value;
                        xElement.Remove();
                        break;
                    case "exception":
                        var xmlName = xElement.Attribute("cref")?.Value;
                        var exceptionItem =
                            _models.FirstOrDefault(model => model.XmlName == xmlName);
                        if (_model.Exceptions == null)
                            _model.Exceptions = new List<MarkdownTemplateModel>();
                        if (exceptionItem != null)
                            _model.Exceptions.Add(exceptionItem);
                        else
                        {
                            _model.Exceptions.Add(new MarkdownTemplateModel
                            {
                                Name = GetNormalizedName(xmlName),
                                Description = xElement.Value
                            });
                        }
                        xElement.Remove();
                        break;
                    case "param":
                        if (_model.ArgsTypes != null)
                            _model.ArgsTypes
                                    .First(model => model.ParamName == xElement.Attribute("name")?.Value)
                                    .Description = xElement.Value;
                        xElement.Remove();
                        break;
                    case "paramref":
                        xElement.ReplaceWith(_compiledTemplates["paramref"](
                            _model.ArgsTypes.First(model => model.ParamName == xElement.Attribute("name")?.Value)));
                        break;
                    case "remarks":
                        _model.Remarks = _compiledTemplates["remarks"](new { value = xElement.Value });
                        xElement.Remove();
                        break;
                    case "see":
                        var seeItemName = xElement.Attribute("cref")?.Value;
                        var seeItem =
                            _models.FirstOrDefault(model => model.XmlName == seeItemName);
                        xElement.ReplaceWith(seeItem != null
                            ? _compiledTemplates["link"](seeItem)
                            : _compiledTemplates["link"](new MarkdownTemplateModel
                            {
                                Name = GetNormalizedName(seeItemName)
                            }));
                        break;
                    case "seealso":
                        var seealsoItem =
                            _models.FirstOrDefault(model => model.XmlName == xElement.Attribute("cref")?.Value);
                        if (seealsoItem != null)
                        {
                            if (_model.Seealso == null)
                                _model.Seealso = new List<MarkdownTemplateModel>();
                            _model.Seealso.Add(seealsoItem);
                        }
                        xElement.Remove();
                        break;
                    case "typeparam":
                        _model.Generics
                            .First(model => model.Name == xElement.Attribute("name")?.Value)
                            .Description = xElement.Value;
                        break;
                    case "typeparamref":
                        xElement.ReplaceWith(_compiledTemplates["typeparamref"](
                            _model.ArgsTypes.First(model => model.Name == xElement.Attribute("name")?.Value)));
                        break;
                    case "value":
                        _model.Value = xElement.Value;
                        xElement.Remove();
                        break;
                }
            }
        }

        private string GetListTag(XElement xElement)
        {
            var list = new XmlListTagModel();
            if (xElement.Attribute("type")?.Value == "table")
            {
                list.IsTable = true;
                var listheader = xElement?.Element("listheader");
                if (listheader?.Elements() != null)
                    foreach (var headerElement in listheader?.Elements())
                    {
                        if (list.HeaderList == null)
                            list.HeaderList = new List<string>();
                        list.HeaderList.Add(headerElement.Value);
                    }
                listheader?.Remove();
            }
            else if (xElement.Attribute("type")?.Value == "bullet")
                list.IsBullet = true;
            else list.IsNumber = true;
            var elements = xElement.Elements()?.ToList();
            if (elements == null) return null;
            list.ItemsList = new List<XmlListItem>();
            foreach (var itemElement in elements)
            {
                list.ItemsList.Add(new XmlListItem
                {
                    Items = itemElement
                        .Elements()
                        .ToList()
                        .ForEachReturn(GetValue)
                        .Select(element => element.Value)
                        .ToList(),
                });
            }

            return _compiledTemplates["listTag"](list);
        }

        private string GetNormalizedName(string name)
        {
            var splittedName = name.Split(':');
            return splittedName.Last();
        }

    }
}