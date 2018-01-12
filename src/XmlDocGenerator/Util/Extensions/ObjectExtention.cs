using System;

namespace XmlDocGenerator.Util.Extensions
{
    public static class ObjectExtention
    {
        public static T Action<T>(this object obj, Action<T> action)
        {
            action((T)obj);
            return (T)obj;
        }
    }
}