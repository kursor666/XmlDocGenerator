using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.Extensions;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo
{
    public class BaseAssemblyType : BaseAssemblyNamespaceType
    {
        public Assembly Assembly { get; set; }

        public MemberInfo Member { get; set; }

        public IEnumerable<Attribute> Attributes { get; set; }

        public string ShortName { get; set; }

        protected BaseAssemblyType(MemberInfo member, Assembly assembly) : base(assembly)
        {
            Name = member.Name;
            Module = member.Module;
            Namespace = member.DeclaringType?.Namespace ?? (member as Type)?.Namespace;
            FullName = $"{Namespace}.{Name}";
            if (assembly.Modules.Contains(Module))
                IsTargetType = true;
            Assembly = assembly;
            Attributes = member.GetCustomAttributes();
            Member = member;
            if (!IsTargetType) return;
            Description = member.GetXmlDescription();
            XmlName = member.GetMemberElementName();
        }

    }
}