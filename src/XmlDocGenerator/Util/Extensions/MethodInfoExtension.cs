using System.Reflection;

namespace XmlDocGenerator.Util.Extensions
{
    public static class MethodInfoExtesion
    {
        public static bool IsOverride(this MethodInfo methodInfo)
        {
            return (methodInfo.GetBaseDefinition() != methodInfo);
        }
    }
}