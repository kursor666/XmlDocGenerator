using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XmlDocGenerator.Util.Extensions
{
    public static class MemberInfoExtention
    {
        public static string GetMemberElementName(this MemberInfo member)
        {
            char prefixCode;
            var memberName = (member is Type)
                ? ((Type)member).FullName
                : (member.DeclaringType?.FullName + "." + member.Name);

            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    memberName = memberName?.Replace(".ctor", "#ctor");
                    goto case MemberTypes.Method;
                case MemberTypes.Method:
                    prefixCode = 'M';

                    var paramTypesList = String.Join(
                        ",",
                        ((MethodBase)member).GetParameters()
                        .Select(x => x.ParameterType.FullName
                        ).ToArray()
                    );
                    if (!String.IsNullOrEmpty(paramTypesList)) memberName += "(" + paramTypesList + ")";
                    break;

                case MemberTypes.Event:
                    prefixCode = 'E';
                    break;

                case MemberTypes.Field:
                    prefixCode = 'F';
                    break;

                case MemberTypes.NestedType:
                    memberName = memberName?.Replace('+', '.');
                    goto case MemberTypes.TypeInfo;
                case MemberTypes.TypeInfo:
                    prefixCode = 'T';
                    break;

                case MemberTypes.Property:
                    prefixCode = 'P';
                    break;

                default:
                    throw new ArgumentException("Unknown member type", nameof(member));
            }

            return $"{prefixCode}:{memberName}";
        }

        public static string GetXmlDescription(this MemberInfo member)
        {
            try
            {
                var targetDocumentPath = member.Module.Assembly.CodeBase.Replace("dll", "xml").Replace("exe", "xml");
                var targetDocument = XDocument.Load(targetDocumentPath);
                var documentElementChildNodes =
                    targetDocument.Root?.Elements()
                        .FirstOrDefault(element => element.Name == "members");
                return (documentElementChildNodes?.Elements())?.Where(element =>
                        element.Name == "member" && element.Attribute("name")?.Value == member.GetMemberElementName())
                    .Select(element => element.ToString()).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }




    }
}