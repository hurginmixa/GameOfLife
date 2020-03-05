using System.Collections.Generic;
using System.Text;

namespace GameOfLifeAppl
{
    internal static class Extensions
    {
        public static string Combine<T>(this IEnumerable<T> list, string delimiter)
        {
            StringBuilder builder = new StringBuilder();

            foreach (T value in list)
            {
                if (builder.Length > 0)
                {
                    builder.Append(delimiter);
                }

                builder.Append(value.ToString());
            }

            return builder.ToString();
        }
    }
}
