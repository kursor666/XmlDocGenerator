using System.Reflection;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes
{
    public class Constructor:MethodBaseType
    {
        public Constructor(ConstructorInfo member, Assembly assembly) : base(member, assembly)
        {
            Form = "Constructor";
            Name = $"{member.DeclaringType?.Name}{CreateMethodParamName(member)}";
            ShortName = member.DeclaringType?.Name;
            FullName = $"{member.DeclaringType?.FullName?.Replace('+','.')}.{Name}";
        }
    }
}