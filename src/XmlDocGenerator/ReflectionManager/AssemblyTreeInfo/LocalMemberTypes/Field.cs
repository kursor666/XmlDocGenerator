using System;
using System.Reflection;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.Extensions;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes
{
    public class Field : FieldPropertyBase
    {
        public FieldConstValue ConstValue { get; set; }

        public bool IsEnumElement { get; set; } = false;

        public Field(FieldInfo member, Assembly assembly) : base(member, assembly)
        {
            Form = "Field";
            FieldType = member.FieldType;
            if (member.DeclaringType != null && member.DeclaringType.IsEnum)
                IsTargetType = false;
            if (!member.IsLiteral || member.IsInitOnly) return;
            ConstValue = new FieldConstValue();
            if (member.DeclaringType.IsEnum)
                IsEnumElement = true;
            ConstValue.Value = member.DeclaringType.IsEnum ? member.Name : member.GetRawConstantValue().ToString();
            var constValue = member.GetRawConstantValue().ToString();
            if (!constValue.IsNumeric()) return;
            ConstValue.Base10Value = constValue;
            if (member.DeclaringType?.GetCustomAttribute(typeof(FlagsAttribute)) != null)
            ConstValue.Base16Value = BitConverter.ToString(BitConverter.GetBytes(double.Parse(constValue)))
                .Replace("-", "");
        }


    }
}