using System;
using System.Reflection;
using System.Reflection.Emit;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes
{
    public class FieldPropertyBase:LocalAssemblyType
    {
        public Type FieldType { get; set; }

        public FieldPropertyBase(MemberInfo member, Assembly assembly) : base(member, assembly)
        {
            
        }
    }
}
