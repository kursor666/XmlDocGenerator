using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HandlebarsDotNet;
using XmlDocGenerator.MarkdownManager.MarkdownModels;
using XmlDocGenerator.ProjectManager;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.Extensions;
using XmlDocGenerator.Util.TypeItems;
using XmlDocGenerator.XmlManager;
using XmlDocGenerator.YamlManager.YamlModels;

namespace XmlDocGenerator.MarkdownManager
{
    /// <summary>
    /// Данный класс отвечает за создание объектной модели из временного хранения в yaml формате <see cref="XmlDocGenerator.YamlManager.YamlModels"/>.
    /// </summary>
    public class MarkdownController : BaseController
    {
        #region Fields

        private StringTypeItems _typeItems;

        private readonly Dictionary<string, string> _typeValue;

        private readonly Dictionary<BaseTypeNamespaceModel, MarkdownTemplateModel> _models;

        private readonly GenerateMode _generateMode;

        #endregion

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MarkdownController"/>.
        /// </summary>
        /// <param name="generateMode">Набор опций режима генерации документации, передает в данный класс путь к шаблонам, путь для сохранения документации.</param>
        public MarkdownController(GenerateMode generateMode)
        {
            _generateMode = generateMode;
            _models = new Dictionary<BaseTypeNamespaceModel, MarkdownTemplateModel>();
            _typeValue = new Dictionary<string, string>
            {
                {"Class", "Класс"},
                {"Interface", "Интерфейс"},
                {"Struct", "Структура"},
                {"Enum", "Перечисление"},
                {"Constructor", "Конструктор"},
                {"Event", "Событие"},
                {"Field", "Поле"},
                {"NestedType", "Вложенный тип"},
                {"Property", "Свойство"},
                {"Method", "Метод"},
                {"Delegate","Делегат" },
                {"Namespace","Пространство имен" },
                {"Attribute", "Аттрибут"}
            };
        }

        /// <summary>
        /// Метод передает закрытому полю набор данных из временного хранилища.
        /// </summary>
        /// <param name="typeItems">Набор данных из временного хранилища, создаваемый в режиме сканирования.</param>
        public void SetTypeItems(StringTypeItems typeItems) => _typeItems = typeItems;

        /// <summary>
        /// В данном методе происходит создание объектной модели, создание на ее основе документации и ее сохранение.
        /// </summary>
        public void CreateAndSave()
        {
            var template = new MarkdownTemplate(_generateMode);
            var xml = new XmlController(_generateMode);
            template.CreateTemplate();
            var elements = xml
                .GetElements(CreateRelationsFromTypesHash().ToList());
            CreateFileNumbers(elements);
            var namespaces = new NamepacesIndexPageModel { NamespacesList = new List<MarkdownTemplateModel>() };
            foreach (var model in elements/*.OrderBy(model => model.RelativeKey)*/.ToList())
            {
                if (!model.IsTargetType) continue;
                if (model.RelativeKey != template.CurrentRelativeKey)
                {
                    Write(template.CurrentRelativeKey);
                    template.CurrentRelativeKey = model.RelativeKey;
                    template.CreateTemplate();
                    Write(template.CurrentRelativeKey);
                }
                model.MdKey = _generateMode.MdKey;
                Save(template.CreateMdText(model), $"{_generateMode.OutPath}{model.FilePath}");
                Write(model.FullName + $" {model.RelativeKey}");
                if (model.TypeForm == "Пространство имен")
                    namespaces.NamespacesList.Add(model);
            }
            Save(template.CreateMdIndexPage(namespaces), $"{_generateMode.OutPath}Index.md");
        }

        private void CreateFileNumbers(List<MarkdownTemplateModel> models)
        {
            foreach (var markdownTemplateModel in models)
            {
                if (models.FirstOrDefault(model =>
                        model.FilePath != null
                        && model.FilePath == markdownTemplateModel.FilePath
                        && model != markdownTemplateModel) == null) continue;
                markdownTemplateModel.FileNumber++;
                CreateFileNumbers(models);
            }
        }

        private IEnumerable<MarkdownTemplateModel> CreateRelationsFromTypesHash()
        {
            #region BaseInit
            var initModels = new List<MarkdownTemplateModel>();

            MarkdownTemplateModel Init(BaseTypeNamespaceModel item)
            {
                var init = new MarkdownTemplateModel
                {
                    Name = item.Name,
                    TypeForm = item.TypeForm,
                    FullName = item.FullName
                };
                if (item.IsTargetType)
                    init.FilePath = $"{item.Namespace.Replace(".", "/")}/{item.Name}.md";
                return init;
            }

            foreach (var type in _typeItems.Types)
                _models.Add(type, Init(type));
            foreach (var type in _typeItems.Constructors)
                _models.Add(type, Init(type));
            foreach (var type in _typeItems.Events)
                _models.Add(type, Init(type));
            foreach (var type in _typeItems.Fields)
                _models.Add(type, Init(type));
            foreach (var type in _typeItems.Methods)
                _models.Add(type, Init(type));
            foreach (var type in _typeItems.Properties)
                _models.Add(type, Init(type));
            foreach (var type in _typeItems.Namespaces)
                _models.Add(type, Init(type));
            foreach (var type in _typeItems.Attributes)
                _models.Add(type, Init(type));
            foreach (var type in _typeItems.Delegates)
                _models.Add(type, Init(type));

            #endregion

            foreach (var markdownTemplateModel in _models)
            {
                switch (markdownTemplateModel.Value.TypeForm)
                {
                    case "Class":
                        initModels.Add(InitTypeTemplateModel((AssemblyTypeModel)markdownTemplateModel.Key));
                        break;
                    case "Interface":
                        initModels.Add(InitTypeTemplateModel((AssemblyTypeModel)markdownTemplateModel.Key));
                        break;
                    case "Struct":
                        initModels.Add(InitTypeTemplateModel((AssemblyTypeModel)markdownTemplateModel.Key));
                        break;
                    case "Enum":
                        initModels.Add(InitTypeTemplateModel((AssemblyTypeModel)markdownTemplateModel.Key));
                        break;
                    case "Constructor":
                        initModels.Add(InitConstructorTemplateModel((ConstructorModel)markdownTemplateModel.Key));
                        break;
                    case "Event":
                        initModels.Add(InitEventTemplateModel((EventModel)markdownTemplateModel.Key));
                        break;
                    case "Field":
                        initModels.Add(InitFieldTemplateModel((FieldModel)markdownTemplateModel.Key));
                        break;
                    case "NestedType":
                        initModels.Add(InitTypeTemplateModel((AssemblyTypeModel)markdownTemplateModel.Key));
                        break;
                    case "Property":
                        initModels.Add(InitPropertyTemplateModel((PropertyModel)markdownTemplateModel.Key));
                        break;
                    case "Method":
                        initModels.Add(InitMethodTemplateModel((MethodModel)markdownTemplateModel.Key));
                        break;
                    case "Delegate":
                        initModels.Add(InitTypeTemplateModel((AssemblyTypeModel)markdownTemplateModel.Key));
                        break;
                    case "Namespace":
                        initModels.Add(InitNamespaceTemplateModel((NamespaceModel)markdownTemplateModel.Key));
                        break;
                    case "Attribute":
                        initModels.Add(InitTypeTemplateModel((AssemblyTypeModel)markdownTemplateModel.Key));
                        break;
                    default:
                        throw new ArgumentException("Неизвестный тип");
                }
            }

            foreach (var templateModel in initModels)
            {
                if (templateModel.Fields == null) continue;
                templateModel.EnumElements = templateModel.Fields.Where(model => model.IsEnumElement).ToList();
                templateModel.Fields = templateModel.Fields.Except(templateModel.EnumElements).ToList();
                templateModel.ConstsList = templateModel.Fields.Where(model => model.ConstValue != null).ToList();
                templateModel.Fields = templateModel.Fields.Except(templateModel.ConstsList).ToList();
            }

            CreateInheritanceHierarchy();

            return initModels;
        }

        private void CreateInheritanceHierarchy()
        {
            void CreateInheritanceHierarchy(MarkdownTemplateModel model, MarkdownTemplateModel baseModel)
            {
                if (baseModel == null) return;
                model.InheritanceHierarchy.Add(baseModel);
                var baseClass = baseModel.BaseClass;
                CreateInheritanceHierarchy(model, baseClass);
            }

            foreach (var modelsValue in _models.Values)
            {
                if (modelsValue.TypeForm != "Класс") continue;
                if (modelsValue.InheritanceHierarchy == null)
                {
                    modelsValue.InheritanceHierarchy =
                        new List<MarkdownTemplateModel> { modelsValue };
                    CreateInheritanceHierarchy(modelsValue, modelsValue.BaseClass);
                }
            }
            foreach (var modelsValue in _models.Values)
                modelsValue.InheritanceHierarchy?.Reverse();
        }



        #region Inits

        private MarkdownTemplateModel InitBaseTemplateModel(BaseTypeNamespaceModel item)
        {
            var type = _models[item];
            type.Namespace = _models.Values.FirstOrDefault(model => model.Name == item.Namespace);
            type.Name = item.Name;
            type.FullName = item.FullName;
            type.Description = item.Description;
            type.TypeForm = _typeValue[item.TypeForm];
            type.Module = item.Module;
            type.Version = item.Version;
            type.IsTargetType = item.IsTargetType;
            type.XmlName = item.XmlName;
            return type;
        }

        private MarkdownTemplateModel InitNamespaceTemplateModel(NamespaceModel item)
        {
            var @namespace = InitBaseTemplateModel(item);
            @namespace.Classes =
                _models
                    .Values
                    .Where(model => item.Classes.FirstOrDefault(s => model.FullName == s) != null)
                    .ToList();
            @namespace.Interfaces =
                _models
                    .Values
                    .Where(model => item.Interfaces.FirstOrDefault(s => model.FullName == s) != null)
                    .ToList();
            @namespace.Structs =
                _models
                    .Values
                    .Where(model => item.Structs.FirstOrDefault(s => model.FullName == s) != null)
                    .ToList();
            @namespace.Enums =
                _models
                    .Values
                    .Where(model => item.Enums.FirstOrDefault(s => model.FullName == s) != null)
                    .ToList();
            @namespace.Delegates = _models
                .Values
                .Where(model => item.Delegates.FirstOrDefault(s => model.FullName == s) != null)
                .ToList();
            @namespace.Attributes = _models
                .Values
                .Where(model => item.Attributes.FirstOrDefault(s => model.FullName == s) != null)
                .ToList();
            @namespace.FilePath = $"{@namespace.Name.Replace('.', '/')}/{@namespace.Name}.md";
            return @namespace;
        }

        private MarkdownTemplateModel InitTypeTemplateModel(AssemblyTypeModel item)
        {
            var type = InitBaseTemplateModel(item);
            type.IsGeneric = item.IsGeneric;
            type.BaseClass = _models.Values.FirstOrDefault(model => model.FullName == item.BaseType) ??
                             (item.BaseType == null ? null : new MarkdownTemplateModel() { Name = item.BaseType });

            if (item.Attributes != null)
                type.Attributes = _models
                    .Values
                    .Where(model => item.Attributes?.FirstOrDefault(s => model.FullName == s) != null)
                    .Concat(item
                        .Attributes
                        .Except(_models.Values.Select(model => model.FullName))
                        .Select(s => new MarkdownTemplateModel { Name = s }))
                    .ToList();

            if (item.BaseInterfaces != null)
                type.BaseInterfaces = _models
                    .Values
                    .Where(model => item.BaseInterfaces?.FirstOrDefault(s => model.FullName == s) != null)
                    .Concat(item
                        .BaseInterfaces
                        .Except(_models.Values.Select(model => model.FullName))
                        .Select(s => new MarkdownTemplateModel { Name = s }))
                    .ToList();
            if (item.Methods != null)
                type.Methods =
                    item
                        .Methods
                        .Where(s => _models.Values.FirstOrDefault(model => model.FullName == s) != null)
                        .Select(s => _models.Values.First(model => model.FullName == s))
                        .ToList();
            if (item.Properties != null)
                type.Properties =
                item
                    .Properties
                    .Where(s => _models.Values.FirstOrDefault(model => model.FullName == s) != null)
                    .Select(s => _models.Values.First(model => model.FullName == s))
                    .ToList();
            if (item.Fields != null)
                type.Fields =
                item
                    .Fields
                    .Where(s => _models.Values.FirstOrDefault(model => model.FullName == s) != null)
                    .Select(s => _models.Values.First(model => model.FullName == s))
                    .ToList();
            if (item.Events != null)
                type.Events =
                item
                    .Events
                    .Where(s => _models.Values.FirstOrDefault(model => model.FullName == s) != null)
                    .Select(s => _models.Values.First(model => model.FullName == s))
                    .ToList();
            if (item.Constructors != null)
                type.Constructors =
                item
                    .Constructors
                    .Where(s => _models.Values.FirstOrDefault(model => model.FullName == s) != null)
                    .Select(s => _models.Values.First(model => model.FullName == s))
                    .ToList();
            if (item.NestedTypes != null)
                type.NestedTypes =
                item
                    .NestedTypes
                    .Where(s => _models.Values.FirstOrDefault(model => model.FullName == s) != null)
                    .Select(s => _models.Values.First(model => model.FullName == s))
                    .ToList();
            if (!type.IsGeneric) return type;
            {
                type.Generics = new List<MdGenericsModel>();
                foreach (var itemGeneric in item.Generics)
                {
                    type.Generics.Add(new MdGenericsModel
                    {
                        Name = itemGeneric
                            .Key,
                        TypesList = itemGeneric
                            .Value
                            .Where(s => _models.Values.FirstOrDefault(model => model.FullName == s) != null)
                            .Select(s => _models.Values.First(model => model.FullName == s).CloneCast)
                            .Concat(
                                itemGeneric
                                    .Value.Except(_models.Values.Select(model => model.FullName))
                                    .Select(s => new MarkdownTemplateModel() { Name = s }))
                            .ToList()
                    });
                }
            }
            return type;
        }

        private MarkdownTemplateModel InitLocalTemplateModel(LocalAssemblyTypeModel item)
        {
            var localType = InitBaseTemplateModel(item);
            if (localType.IsTargetType)
                localType.FilePath = $"{item.Namespace.Replace(".", "/")}/{item.DeclaredType}/{item.ShortName}.md"
                    .Replace('<', '[').Replace('>', ']');
            var findedAtr = item
                .Attributes
                .Select(attribute => _models.Values.FirstOrDefault(model => model.FullName == attribute))
                .Where(model => model != null)
                .ToList();

            var findOutAtr = item
                .Attributes
                .Except(findedAtr.Select(model => model.FullName))
                .Select(str => new MarkdownTemplateModel() { Name = str })
                .ToList();

            if (findedAtr.Count != 0)
                localType.Attributes = findedAtr;

            if (findOutAtr.Count == 0) return localType;
            if (localType.Attributes == null)
                localType.Attributes = new List<MarkdownTemplateModel>();
            localType.Attributes.AddRange(findOutAtr);

            return localType;
        }

        private MarkdownTemplateModel InitConstructorTemplateModel(MethodConstructorBaseModel item)
        {
            var baseModel = InitLocalTemplateModel(item);
            baseModel.IsGeneric = item.IsGeneric;
            var argsDict = item.ArgsTypes.ToDictionary(s => s.Split(' ').Last(), s => s.Split(' ').First());
            baseModel.ArgsTypes =
                argsDict
                    .Where(pair => _models.Values.FirstOrDefault(model => model.FullName == pair.Value) != null)
                    .Select(pair =>
                        _models.Values.First(model => model.FullName == pair.Value).CloneCast
                            .Action<MarkdownTemplateModel>(model => model.ParamName = pair.Key))
                    .Concat(
                        argsDict.Values
                            .Except(_models.Values.Select(model => model.FullName))
                            .Select(s => GetParam(s).Action<MarkdownTemplateModel>(model =>
                            {
                                model.ParamName = argsDict.First(pair => pair.Value == s).Key;
                            })))
                    .ToList();

            var splittedName = $"{baseModel.Name.Split('(')[0]}(";
            if (splittedName.Contains(' ')) splittedName = splittedName.Split(' ').Last();
            if (baseModel.HasArgsTypes)
            {
                foreach (var argsType in baseModel.ArgsTypes)
                {
                    splittedName += $"{argsType.Name}, ";
                }
                splittedName = splittedName.Remove(splittedName.LastIndexOf(','));
            }
            splittedName += ")";

            baseModel.Name = splittedName;
            if (baseModel.IsTargetType)
                baseModel.FilePath = $"{item.Namespace.Replace(".", "/")}/{item.DeclaredType}/{item.ShortName}.md"
                    .Replace('<', '[').Replace('>', ']');

            if (!baseModel.IsGeneric) return baseModel;
            {
                baseModel.Generics = new List<MdGenericsModel>();
                foreach (var itemGeneric in item.Generics)
                {
                    baseModel.Generics.Add(new MdGenericsModel
                    {
                        Name = itemGeneric
                        .Key,
                        TypesList = itemGeneric
                        .Value
                        .Where(s => _models.Values.FirstOrDefault(model => model.FullName == s) != null)
                        .Select(s => _models.Values.First(model => model.FullName == s).CloneCast)
                        .Concat(
                            itemGeneric
                            .Value.Except(_models.Values.Select(model => model.FullName))
                            .Select(s => new MarkdownTemplateModel() { Name = s }))
                            .ToList()
                    });
                }
            }
            return baseModel;
        }

        private MarkdownTemplateModel InitMethodTemplateModel(MethodModel item)
        {
            var method = InitConstructorTemplateModel(item);
            method.ReturnType = _models
                                    .Values
                                    .FirstOrDefault(model => model.FullName == item.ReturnType)?.CloneCast ??
                                GetParam(item.ReturnType);
            return method;
        }

        private MarkdownTemplateModel InitFieldPropertyTemplateModel(FieldPropertyBaseModel item)
        {
            var field = InitLocalTemplateModel(item);
            field.FieldType = _models
                                  .Values
                                  .FirstOrDefault(model => model.FullName == item.FieldType) ??
                              new MarkdownTemplateModel() { Name = item.FieldType };
            return field;
        }

        private MarkdownTemplateModel InitFieldTemplateModel(FieldModel item)
        {
            var field = InitFieldPropertyTemplateModel(item);
            field.ConstValue = item.ConstValue;
            field.IsEnumElement = item.IsEnumElement;
            return field;
        }

        private MarkdownTemplateModel InitPropertyTemplateModel(PropertyModel item)
        {
            var property = InitFieldPropertyTemplateModel(item);
            return property;
        }

        private MarkdownTemplateModel InitEventTemplateModel(EventModel item)
        {
            var @event = InitLocalTemplateModel(item);
            return @event;
        }

        #endregion

        private MarkdownTemplateModel GetParam(string fullName)
        {
            string LocalSub(string name)
            {
                var startIndex = name.LastIndexOf("<", StringComparison.Ordinal) + 1;
                var endIndex = name.IndexOf(">", StringComparison.Ordinal) - 1;
                name = name.SubstringFromIndex(startIndex, endIndex);
                return name;
            }

            if (!fullName.Contains("<")) return new MarkdownTemplateModel { Name = fullName };
            var model = new MarkdownTemplateModel();
            var localName = LocalSub(fullName);
            var tempModel =
                _models.Values.FirstOrDefault(templateModel => templateModel.Name == localName);

            var compiledTemplate = Handlebars.Compile(File.ReadAllText($@"{_generateMode.TemplatesPath}\Partials\link.txt"));
            model.Name = tempModel != null
                ? fullName.Replace(localName, compiledTemplate(tempModel))
                : fullName;
            return model;


        }

        private void Save(string text, string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
            using (var fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var array = System.Text.Encoding.UTF8.GetBytes(text);
                fstream.Write(array, 0, array.Length);
            }
        }

        /// <summary>
        /// Данный метод очищает данные документации, перед созданием новой, если эти данные есть.
        /// </summary>
        /// <param name="repeat"></param>
        public void DeleteCash(bool repeat = true)
        {
            if (!repeat) return;
            if (!Directory.Exists(_generateMode.OutPath)) return;
            var dirInfo = new DirectoryInfo(_generateMode.OutPath);
            try
            {
                dirInfo.Delete(true);
            }
            catch (IOException)
            {
                DeleteCash(false);
            }
        }
    }
}