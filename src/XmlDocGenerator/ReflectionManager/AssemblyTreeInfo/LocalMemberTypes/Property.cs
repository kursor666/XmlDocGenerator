using System;
using System.Reflection;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.Extensions;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes
{
    public class Property:FieldPropertyBase
    {
        public bool IsSetter { get; }

        public bool IsGetter { get; }

        public PropertyInfo PropertyInfo { get; }

        public Property(PropertyInfo member, Assembly assembly) : base(member, assembly)
        {
            Form = "Property";
            PropertyInfo = member;
            FieldType = member.PropertyType;
            IsGetter = member.CanRead;
            IsSetter = member.CanWrite;
            IsVirtual = member.IsVirtual();
            Name += " {" + (IsGetter ? " get; " : " ") + (IsSetter ? "set; }":"}");
            FullName = $"{member.DeclaringType?.FullName?.Replace('+', '.')}.{Name}";
        }
    }
}