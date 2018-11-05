using System;
using System.Collections.Generic;

namespace ClpQrColoring.Extensions
{
    public static class StringExtension
    {
        public static string ToConcatenatedString(
            this IEnumerable<string> strings, string separator)
        {
            return String.Join(separator, strings);
        }
    }
}