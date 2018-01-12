using System.Reflection;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo
{
    public class LocalAssemblyType : BaseAssemblyType
    {
        public bool IsVirtual { get; protected set; }

        

        protected LocalAssemblyType(MemberInfo member, Assembly assembly) : base(member, assembly)
        {
            Form = member.MemberType.ToString();
            Name = member.Name;
            FullName = $"{member.DeclaringType?.FullName}.{member.Name}";
            ShortName = member.Name;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}