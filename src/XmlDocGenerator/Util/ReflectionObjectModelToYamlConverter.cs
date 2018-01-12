using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes;
using XmlDocGenerator.Util.TypeItems;
using XmlDocGenerator.YamlManager.YamlModels;

namespace XmlDocGenerator.Util
{
    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
    public class ReflectionObjectModelToYamlConverter
    {

        #region FieldsAndCtors

        private readonly ObjectTypeItems _objectTypeItems;

        public ReflectionObjectModelToYamlConverter(ObjectTypeItems objectTypeItems)
        {
            _objectTypeItems = objectTypeItems;
        }


        #endregion


        #region InitTypes

        private TBaseType InitBaseAssemblyNamepaceTypeDTO<TBaseType>(BaseAssemblyNamespaceType type)
            where TBaseType : BaseTypeNamespaceModel, new() => new TBaseType()
            {
                Namespace = type.Namespace,
                Description = type.Description,
                IsTargetType = type.IsTargetType,
                Module = type.Module.ToString(),
                Name = type.Name,
                FullName = type.FullName,
                TypeForm = type.Form,
                Version = type.Version,
                XmlName = type.XmlName
            };

        private TAssemblyType InitBaseAssemblyTypeDto<TAssemblyType>(BaseAssemblyType type)
            where TAssemblyType : BaseTypeModel, new()
        {
            var item = InitBaseAssemblyNamepaceTypeDTO<TAssemblyType>(type);
            item.Attributes = type.Attributes.Select(attribute => attribute.ToString()).ToList();
            return item;
        }

        private TLocalType InitLocalTypeDto<TLocalType>(LocalAssemblyType type)
            where TLocalType : LocalAssemblyTypeModel, new()
        {
            var item = InitBaseAssemblyTypeDto<TLocalType>(type);
            item.DeclaredType = type.Member.DeclaringType?.Name;
            item.TypeForm = type.Form;
            item.ShortName = type.ShortName;
            return item;
        }

        private TLocalType InitMethodConstructorBaseDto<TLocalType>(MethodBaseType type)
            where TLocalType : MethodConstructorBaseModel, new()
        {
            var item = InitLocalTypeDto<TLocalType>(type);
            item.Generics = GetGenericsDictonary(type);
            item.ArgsTypes = type.ArgsTypes.ToList();
            item.IsGeneric = type.IsGeneric;

            return item;
        }

        private TLocalType InitFieldPropertyBase<TLocalType>(FieldPropertyBase type)
            where TLocalType : FieldPropertyBaseModel, new()
        {
            var item = InitLocalTypeDto<TLocalType>(type);
            item.FieldType = type.FieldType.ToString();
            return item;
        }

        #endregion

        #region GetDictonaries

        private Dictionary<string, List<string>> GetGenericsDictonary(Dictionary<Type, ICollection<Type>> dictionary)
        {
            return dictionary?
                .ToDictionary(pair => pair.Key.ToString(),
                    pair => pair.Value.Select(type1 => type1.FullName).ToList());
        }

        private Dictionary<string, List<string>> GetGenericsDictonary(AssemblyType baseType)
        {
            return GetGenericsDictonary(baseType.GenericsParamsConstraints);
        }

        private Dictionary<string, List<string>> GetGenericsDictonary(MethodBaseType baseType)
        {
            return GetGenericsDictonary(baseType.GenericsParamsConstraints);
        }


        #endregion

        #region GetModels

        public StringTypeItems GetStringTypesHash() =>
            new StringTypeItems()
            {
                FilePath = _objectTypeItems.FilePath,
                AssemblyName = _objectTypeItems.AssemblyName,
                Types = GetTypes(),
                Constructors = GetConstructors(),
                Methods = GetMethods(),
                Events = GetEvents(),
                Fields = GetFields(),
                Properties = GetProperties(),
                Namespaces = GetNamespaces(),
                Attributes = GetAttributes(),
                Delegates = GetDelegates()
            };

        private ICollection<AssemblyTypeModel> GetDelegates() =>
            _objectTypeItems.Types.Values
                .Where(type => type.BaseType?.Name == "MulticastDelegate")
                .Select(type => InitBaseAssemblyTypeDto<AssemblyTypeModel>(type))
                .ToList();


        private ICollection<AssemblyTypeModel> GetAttributes() =>
            _objectTypeItems.Types.Values
                .Where(type => type.Form == "Attribute")
                .Select(type => InitBaseAssemblyTypeDto<AssemblyTypeModel>(type))
                .ToList();

        private ICollection<NamespaceModel> GetNamespaces()
        {
            var result = new List<NamespaceModel>();
            
            foreach (var @namespace in _objectTypeItems.Namespaces.Values)
            {
                var item = InitBaseAssemblyNamepaceTypeDTO<NamespaceModel>(@namespace);
                var types = GetTypes().Concat(GetAttributes().Concat(GetDelegates()))
                    .Where(model => model.Namespace == @namespace.Name)
                    .ToList();
                item.Classes =
                    types
                        .Where(type => type.TypeForm == "Class")
                        .Select(type => type.FullName)
                        .ToList();
                item.Structs =
                    types
                        .Where(type => type.TypeForm == "Struct")
                        .Select(type => type.FullName)
                        .ToList();
                item.Enums =
                    types
                        .Where(type => type.TypeForm == "Enum")
                        .Select(type => type.FullName)
                        .ToList();
                item.Interfaces =
                    types
                        .Where(type => type.TypeForm == "Interface")
                        .Select(type => type.FullName)
                        .ToList();
                item.Delegates =
                    types
                        .Where(type => type.TypeForm == "Delegate")
                        .Select(type => type.FullName)
                        .ToList();
                item.Attributes =
                    types
                        .Where(type => type.TypeForm == "Attribute")
                        .Select(type => type.FullName)
                        .ToList();
                result.Add(item);
            }
            return result;
        }

        private ICollection<AssemblyTypeModel> GetTypes()
        {
            var result = new List<AssemblyTypeModel>();
            foreach (var type in _objectTypeItems.Types.Values)
            {
                var item = InitBaseAssemblyTypeDto<AssemblyTypeModel>(type);
                if (type.Form == "Attribute") continue;
                if (type.Form == "Delegate") continue;
                item.MemberType = type.MemberType.Name;
                item.TypeForm = type.Form;
                item.BaseType = type.BaseType?.FullName;
                item.IsGeneric = type.IsGeneric;
                item.IsNested = type.IsNested;
                item.Constructors = type.Constructors.Select(constructor => constructor.FullName).ToList();
                item.Methods = type.Methods.Select(method => method.FullName).ToList();
                item.Fields = type.Fields.Select(field => field.FullName).ToList();
                item.Events = type.Events.Select(@event => @event.FullName).ToList();
                item.Properties = type.Properties.Select(property => property.FullName).ToList();
                item.NestedTypes = type.NestedTypes.Select(assemblyType => assemblyType.FullName).ToList();
                item.BaseInterfaces = type.BaseInterfaces.Select(assemblyType => assemblyType.FullName).ToList();
                item.SubTypes = type.SubTypes.Select(assemblyType => assemblyType.FullName).ToList();
                item.Generics = GetGenericsDictonary(type);
                result.Add(item);
            }
            return result;
        }

        private ICollection<MethodModel> GetMethods()
        {
            var result = new List<MethodModel>();
            foreach (var method in _objectTypeItems.Methods.Values)
            {
                var item = InitMethodConstructorBaseDto<MethodModel>(method);
                item.ReturnType = method.ReturnTypeString;
                item.IsVirtual = method.IsVirtual;
                result.Add(item);
            }
            return result;
        }

        private ICollection<PropertyModel> GetProperties()
        {
            var result = new List<PropertyModel>();
            foreach (var property in _objectTypeItems.Properties.Values)
            {
                var item = InitFieldPropertyBase<PropertyModel>(property);
                item.IsGetter = property.IsGetter;
                item.IsSetter = property.IsSetter;
                item.IsVirtual = property.IsVirtual;
                result.Add(item);
            }
            return result;
        }

        private ICollection<EventModel> GetEvents() =>
            _objectTypeItems.Events.Values.Select(@event =>
            InitLocalTypeDto<EventModel>(@event))
            .ToList();

        private ICollection<FieldModel> GetFields()
        {
            var result = new List<FieldModel>();
            foreach (var field in _objectTypeItems.Fields.Values)
            {
                var item = InitFieldPropertyBase<FieldModel>(field);
                item.ConstValue = field.ConstValue;
                item.IsEnumElement = field.IsEnumElement;
                result.Add(item);
            }
            return result;
        }

        private ICollection<ConstructorModel> GetConstructors()
        {
            return _objectTypeItems.Constructors.Values.Select(constructor =>
            InitMethodConstructorBaseDto<ConstructorModel>(constructor))
            .ToList();
        }




        #endregion



    }
}