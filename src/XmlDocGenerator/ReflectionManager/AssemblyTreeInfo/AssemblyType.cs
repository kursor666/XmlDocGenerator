using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo
{
    public class AssemblyType : BaseAssemblyType
    {
        public Type MemberType { get; set; }

        public ICollection<AssemblyType> SubTypes { get; set; }

        public AssemblyType BaseType { get; set; }

        public ICollection<AssemblyType> BaseInterfaces { get; set; }

        public ICollection<Method> Methods { get; set; }

        public ICollection<Property> Properties { get; set; }

        public ICollection<Field> Fields { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<Constructor> Constructors { get; set; }

        public ICollection<AssemblyType> NestedTypes { get; set; }

        public bool IsGeneric { get; }

        public Dictionary<Type, ICollection<Type>> GenericsParamsConstraints { get; }

        public bool IsNested { get; set; }


        public AssemblyType(Type type, Assembly assembly) : base(type, assembly)
        {
            if (type == null) return;
            MemberType = type;
            Form = type.IsClass
                ? "Class"
                : type.IsInterface
                    ? "Interface"
                    : type.IsEnum
                        ? "Enum"
                            : "Struct";

            if (type.GetInterfaces().FirstOrDefault(@interface =>
                    @interface.Name == "_Attribute") != null)
                Form = "Attribute";
            if (type.BaseType?.Name == "MulticastDelegate")
                Form = "Delegate";

            IsNested = type.IsNested;
            if (IsNested)
                FullName = type.FullName?.Replace('+', '.');

            IsGeneric = type.IsGenericType;
            if (!IsGeneric) return;
            GenericsParamsConstraints = new Dictionary<Type, ICollection<Type>>();
            foreach (var genericType in type.GetGenericArguments())
            {
                GenericsParamsConstraints.Add(genericType, genericType.GetGenericParameterConstraints());
            }
            
        }

        public override string ToString()
        {
            return $"{MemberType.FullName}";
        }
    }
}