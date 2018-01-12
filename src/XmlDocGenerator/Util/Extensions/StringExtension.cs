using System;
using System.Collections.Generic;
using System.Linq;

namespace XmlDocGenerator.Util.Extensions
{
    public static class StringExtension
    {
        public static string SubstringFromIndex(this string str, int startIndex, int endIndex)
        {
            var length = endIndex - startIndex + 1;
            var result = str.Substring(startIndex, length).Trim();
            return result;
        }

        public static string Substring(this string str, string startValue, string endValue)
        {
            var startIndex = str.IndexOf(startValue, StringComparison.Ordinal);
            var endIndex = str.IndexOf(endValue, StringComparison.Ordinal);
            var result = str.SubstringFromIndex(startIndex, endIndex);
            result = result.Remove(0, startValue.Length - 1).Trim(new[] {'<', '>'});
            return result.Trim();
        }


        public static string ToStringFromArray(this char[] arr)
        {
            return ToStringFromList(arr.ToList());
        }

        public static string ToStringFromList(this List<char> arr)
        {
            return arr.Aggregate("", (current, item) => current + item);
        }

        public static bool IsNumeric(this string s)
        {
            return s.All(c => char.IsDigit(c) || c == '.');
        }

        public static double ToNumeric(this string s)
        {
            return double.Parse(s);
        }
    }
}