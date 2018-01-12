using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XmlDocGenerator.ProjectManager;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.Extensions;
using XmlDocGenerator.Util.TypeItems;

namespace XmlDocGenerator.ReflectionManager
{
    /// <summary>
    /// Класс отвечающий за сборку данных через отражение со сборок (.dll, .exe).
    /// </summary>
    public class ReflectionController : BaseController
    {
        #region Fields

        private ObjectTypeItems _objectTypeItems;

        private const BindingFlags BindingFlags =
            System.Reflection.BindingFlags.Static
            | System.Reflection.BindingFlags.NonPublic
            | System.Reflection.BindingFlags.Public
            | System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.SuppressChangeType;

        private Assembly _assembly;

        private ICollection<Type> _dllTypes;

        private readonly string _dllPath;

        #endregion

        /// <summary>
        /// Конструктор класса, передает в приватное поле путь к сборке через <paramref name="scanMode"/>.
        /// </summary>
        /// <param name="scanMode">Набор опций для режима сканирования, в <see cref="ReflectionController"/> используется для передачи пути к сборке.</param>
        public ReflectionController(ScanMode scanMode)
        {
            _dllPath = scanMode.DllPath;
        }

        /// <summary>
        /// Метод возвращающий объектную модель собранную данным классом.
        /// </summary>
        /// <returns>Объектная модель публичных членов сборки.</returns>
        public ObjectTypeItems GetObjectTypeItems()
        {
            LoadAssembly();
            return _objectTypeItems;
        }

        #region Methods

        private void LoadAssembly()
        {
            try
            {
                _assembly = Assembly.LoadFrom(_dllPath);
                var types = GetTypesFromAssembly().ToList();
                types = CreateAsseblyTreeRelations(types);
                _objectTypeItems = GetTypesInfo(types);
                _objectTypeItems.FilePath = _assembly.EscapedCodeBase;
                _objectTypeItems.AssemblyName = _assembly.GetName().Name;
                _objectTypeItems.Namespaces = GetNamespaces();
                _objectTypeItems.Attributes = GetAttributes();
            }
            // BAD: нужен отлов разных исключений по типу: ошибка чтения файла, неправильный формат, другое..
            catch (Exception e)
            {
                Write(e.Message);
            }
        }

        private IDictionary<string, AssemblyType> GetAttributes()
        {
            var atrList = new List<AssemblyType>();
            foreach (var type in _objectTypeItems.Types.Values)
            {
                if (type.BaseInterfaces
                        .FirstOrDefault(
                            assemblyType => assemblyType.Name == "_Attribute") != null)
                {
                    type.Form = "Attribute";
                    atrList.Add(type);
                }
            }
            return atrList.ToDictionary(type => type.Name);
        }

        private List<AssemblyType> CreateAsseblyTreeRelations(List<AssemblyType> types)
        {
            foreach (var type in types)
            {
                type.SubTypes = FindSubClasses(type.MemberType);
                type.BaseInterfaces = FindBaseInterfaces(type.MemberType);
                type.BaseType = FindBaseType(type.MemberType);
                type.Methods = GetMethods(type.MemberType);
                type.Fields = GetFields(type.MemberType);
                type.Properties = GetProperties(type.MemberType);
                type.Constructors = GetConstructors(type.MemberType);
                type.Events = GetEvents(type.MemberType);
                type.NestedTypes = GetNestedTypes(type.MemberType);
            }
            return types;
        }



        private ObjectTypeItems GetTypesInfo(ICollection<AssemblyType> types)
        {
            var objectTypeItems = new ObjectTypeItems();
            foreach (var type in types)
            {
                if (type.Methods != null)
                    foreach (var method in type.Methods)
                        if (!objectTypeItems.Methods.ContainsKey(method.ToString()))
                            objectTypeItems.Methods.Add(method.ToString(), method);
                if (type.Properties != null)
                    foreach (var property in type.Properties)
                        if (!objectTypeItems.Properties.ContainsKey(property.ToString()))
                            objectTypeItems.Properties.Add(property.ToString(), property);
                if (type.Events != null)
                    foreach (var @event in type.Events)
                        if (!objectTypeItems.Events.ContainsKey(@event.ToString()))
                            objectTypeItems.Events.Add(@event.ToString(), @event);
                if (type.Constructors != null)
                    foreach (var constructor in type.Constructors)
                        if (!objectTypeItems.Constructors.ContainsKey(constructor.ToString()))
                            objectTypeItems.Constructors.Add(constructor.ToString(), constructor);
                if (type.Fields != null)
                    foreach (var field in type.Fields)
                        if (!objectTypeItems.Fields.ContainsKey(field.ToString()))
                            objectTypeItems.Fields.Add(field.ToString(), field);
            }
            // BAD: это таки не хеш ))
            objectTypeItems.Types = types.ToDictionary(type => type.ToString());

            return objectTypeItems;
        }

        



        #endregion

        #region FindhierarchyOfTypes

        private ICollection<AssemblyType> GetTypesFromAssembly()
        {
            try
            {
                _dllTypes = _assembly.GetTypes();
                return _dllTypes
                    .Where(type => !type.IsNotPublic && !type.IsNestedPrivate)
                    .Select(type => new AssemblyType(type, _assembly))
                    .ToList();
            }
            catch (Exception e)
            {
                Write(e.Message);
                throw;
            }
        }

        private ICollection<AssemblyType> FindSubClasses(Type baseType) =>
            _dllTypes
                .Where(subType => baseType.IsAssignableFrom(subType) && baseType != subType)
                .Select(type => new AssemblyType(type, _assembly))
                .ToList();

        private ICollection<AssemblyType> FindBaseInterfaces(Type subType) =>
            subType
                .GetInterfaces()
                .Select(type => new AssemblyType(type, _assembly))
                .ToList();

        private AssemblyType FindBaseType(Type subType) =>
            subType.BaseType == null
                ? null
                : new AssemblyType(subType.BaseType, _assembly) { IsTargetType = false };

        #endregion

        private Dictionary<string, AssemblyNamespace> GetNamespaces()
        {
            var namespaces = new List<AssemblyNamespace>();
            foreach (var type in _objectTypeItems.Types.Values)
            {
                if (!namespaces.Exists(ns => ns.Namespace == type.Namespace))
                    namespaces
                        .Add(new AssemblyNamespace(_assembly)
                        {
                            Name = type.Namespace,
                            Namespace = type.Namespace,
                            FullName = type.Namespace,
                            IsTargetType = type.IsTargetType,
                            Module = type.Module
                        });
                switch (type.Form)
                {
                    case "Class":
                        namespaces.First(ns => ns.Namespace == type.Namespace).Classes.Add(type);
                        break;
                    case "Struct":
                        namespaces.First(ns => ns.Namespace == type.Namespace).Structs.Add(type);
                        break;
                    case "Enum":
                        namespaces.First(ns => ns.Namespace == type.Namespace).Enums.Add(type);
                        break;
                    case "Interface":
                        namespaces.First(ns => ns.Namespace == type.Namespace).Interfaces.Add(type);
                        break;
                    case "Delegate":
                        namespaces.First(ns => ns.Namespace == type.Namespace).Delegates.Add(type);
                        break;
                    case "Attribute":
                        namespaces.First(ns => ns.Namespace == type.Namespace).Attributes.Add(type);
                        break;
                }
            }
            foreach (var @namespace in namespaces)
            {
                @namespace.Description =
                    _dllTypes
                        .FirstOrDefault(type =>
                            type.Namespace == @namespace.Namespace &&
                            type.Name == "NamespaceGroupDoc")?
                        .GetXmlDescription();

                if (@namespace.Description != null)
                    @namespace.IsGroupDoc = true;
                else
                    @namespace.Description =
                        _dllTypes
                            .FirstOrDefault(type =>
                                type.Namespace == @namespace.Namespace &&
                                type.Name == "NamespaceDoc")?
                            .GetXmlDescription();
                @namespace.XmlName = $"N:{@namespace.Name}";
            }
            foreach (var @namespace in namespaces.Where(@namespace => @namespace.IsGroupDoc))
            {
                foreach (var assemblyNamespace in namespaces)
                {
                    if (assemblyNamespace.Namespace.Contains(@namespace.Namespace) &&
                        String.IsNullOrEmpty(assemblyNamespace.Description))
                        assemblyNamespace.Description = @namespace.Description;
                }
            }
            return namespaces.ToDictionary(ns => ns.Namespace);
        }

        #region GetLocalTypes

        private ICollection<Method> GetMethods(Type type) =>
            type
                .GetMembers(BindingFlags)
                .Where(info => info.MemberType == MemberTypes.Method)
                .Cast<MethodInfo>()
                .Where(info => !info.IsPrivate && !info.Name.Contains("g__"))
                .Except(GetProperties(type).ToList().Select(property => property.PropertyInfo.GetMethod))
                .Except(GetProperties(type).ToList().Select(property => property.PropertyInfo.SetMethod))
                .Except(GetEvents(type).ToList().Select(@event => @event.EventInfo.AddMethod))
                .Except(GetEvents(type).ToList().Select(@event => @event.EventInfo.RemoveMethod))
                .Select(info => new Method(info, _assembly))
                .ToList();

        private ICollection<Property> GetProperties(Type type) =>
            type
                .GetMembers(BindingFlags)
                .Where(info => info.MemberType == MemberTypes.Property)
                .Cast<PropertyInfo>()
                .Select(info => new Property(info, _assembly))
                .ToList();

        private ICollection<Field> GetFields(Type type) =>
            type
                .GetMembers(BindingFlags)
                .Where(info => info.MemberType == MemberTypes.Field)
                .Cast<FieldInfo>()
                .Where(info => !info.IsPrivate)
                .Where(info => info.Name!="value__")
                .Select(info => new Field(info, _assembly))
                .ToList();

        private ICollection<Constructor> GetConstructors(Type type) =>
            type
                .GetMembers(BindingFlags)
                .Where(info => info.MemberType == MemberTypes.Constructor)
                .Cast<ConstructorInfo>()
                .Where(info => !info.IsPrivate)
                .Select(info => new Constructor(info, _assembly))
                .ToList();

        private ICollection<Event> GetEvents(Type type) =>
            type
                .GetMembers(BindingFlags)
                .Where(info => info.MemberType == MemberTypes.Event)
                .Cast<EventInfo>()
                .Select(info => new Event(info, _assembly))
                .ToList();

        private ICollection<AssemblyType> GetNestedTypes(Type type) =>
            type
                .GetMembers(BindingFlags)
                .Where(info => info.MemberType == MemberTypes.NestedType)
                .Cast<Type>()
                .Where(info => !info.IsNestedPrivate)
                .Select(nestedType => new AssemblyType(nestedType, _assembly))
                .ToList();

        #endregion


    }
}