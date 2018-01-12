using System.Reflection;
using XmlDocGenerator.Util.Extensions;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes
{
    public class Method : MethodBaseType
    {
        public ParameterInfo ReturnType { get; }

        public string ReturnTypeString { get; }

        public Method(MethodInfo member, Assembly assembly) : base(member, assembly)
        {
            Form = "Method";
            var declaringType = member.ReflectedType;
            if (declaringType != null && (member.IsOverride() && declaringType.IsImport))
                IsTargetType = true;
            ReturnType = member.ReturnParameter;
            ReturnTypeString = ReturnNameParse(ReturnType?.ParameterType.FullName ?? ReturnType?.ParameterType.Name);
            Name = ReturnTypeString + $" {Name}" + CreateMethodParamName(member);
            FullName = ReturnType?.ParameterType.FullName?.Replace('+', '.') + $" {FullName?.Replace('+', '.')}" +
                       CreateMethodParamName(member);
        }



    }
}