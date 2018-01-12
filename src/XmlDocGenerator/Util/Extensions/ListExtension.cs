using System;
using System.Collections.Generic;

namespace XmlDocGenerator.Util.Extensions
{
    public static class ListExtension
    {
        public static List<T> ForEachReturn<T>(this List<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
            return list;
        }
    }
}
