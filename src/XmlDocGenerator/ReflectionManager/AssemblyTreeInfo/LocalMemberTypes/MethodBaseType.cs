using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XmlDocGenerator.Util;
using XmlDocGenerator.Util.Extensions;

namespace XmlDocGenerator.ReflectionManager.AssemblyTreeInfo.LocalMemberTypes
{
    public class MethodBaseType : LocalAssemblyType
    {

        public List<string> ArgsTypes { get; set; }

        public bool IsGeneric { get; }

        public Dictionary<Type, ICollection<Type>> GenericsParamsConstraints { get; set; }

        public MethodBaseType(MethodBase member, Assembly assembly) : base(member, assembly)
        {
            IsVirtual = member.IsVirtual;
            ArgsTypes = member.GetParameters().Select(info =>
                $"{ReturnNameParse(info.ParameterType.FullName ?? info.ParameterType.Name)} {info.Name}").ToList();
            IsGeneric = member.IsGenericMethod;
            if (!IsGeneric) return;
            GenericsParamsConstraints = new Dictionary<Type, ICollection<Type>>();
            foreach (var type in member.GetGenericArguments())
            {
                GenericsParamsConstraints.Add(type, type.GetGenericParameterConstraints());
            }
        }

        protected string CreateMethodParamName(MethodBase member)
        {
            var result = "(";
            var @params = member.GetParameters();
            if (@params.Any())
            {
                foreach (var param in @params)
                    result += $"{ReturnNameParse(param.ParameterType.FullName?.Replace('+', '.'))}, ";
                result = result.Replace("  ", ", ");
                var i = result.LastIndexOf(',');
                var charArray = result.ToCharArray().ToList();
                charArray.RemoveAt(i);
                result = charArray.ToStringFromList();
            }
            result += ")";
            return result;
        }

        protected string ReturnNameParse(string fullName)
        {
            var catchName = fullName;
            try
            {
                if (!fullName.Contains("[[")) return fullName;
                if (fullName.Contains(','))
                    fullName = fullName.Remove(fullName.IndexOf(',')).Replace("[[", "<");
                for (int i = 0; i < fullName.Count(c => c == '<'); i++)
                    fullName += ">";
                string resultName = "";
                foreach (var s in fullName.Split('<'))
                {
                    if (s.Contains('.')) resultName += s.Substring(s.LastIndexOf('.') + 1);
                    if (s.Contains('`')) resultName = resultName.Remove(resultName.LastIndexOf('`'));
                    if (!s.Contains(">")) resultName += "<";
                }
                return resultName;
            }
            catch (Exception)
            {
                return catchName;
            }
        }
    }
}