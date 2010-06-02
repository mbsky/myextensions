using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace System.Collections
{
    public static class IEnumerableExtensions
    {
        public static string Concat(this IEnumerable items, string separator)
        {
            return Concat(items, separator, "{0}");
        }

        public static string Concat(this IEnumerable items, string separator, string template)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in items)
            {
                builder.Append(separator);
                builder.Append(string.Format(template, obj2.ToString()));
            }
            return builder.ToString().RightOf(separator);
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> target)
        {
            Random r = new Random();

            return target.OrderBy(x => (r.Next()));
        }

        public static IEnumerable<List<T>> SplitByLength<T>(this IEnumerable<T> target, int length)
        {
            List<List<T>> res = new List<List<T>>();
            int skip = 0;

            int cnt = target.Count();

            while (skip < cnt)
            {
                var lst = target.Skip(skip).Take(length).ToList();
                res.Add(lst);
                skip += length;
            }

            return res;
        }
    }
}
