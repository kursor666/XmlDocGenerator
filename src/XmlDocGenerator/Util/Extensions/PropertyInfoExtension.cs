using System.Reflection;

namespace XmlDocGenerator.Util.Extensions
{
    public static class PropertyInfoExtension
    {
        public static bool IsVirtual(this PropertyInfo prop)
        {
            var get = prop.GetMethod != null && prop.GetMethod.IsVirtual;
            var set = prop.SetMethod != null && prop.SetMethod.IsVirtual;
            return get && set;
        }

    }
}