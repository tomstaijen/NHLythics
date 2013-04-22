using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHLythics.Extensions
{
    public static class CollectionExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            if (dict.TryGetValue(key, out value))
                return value;
            return defaultValue;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static PairResult<T1, T2> Pair<T1, T2>(this ICollection<T1> firsts, ICollection<T2> seconds,
                                          Func<T1, T2, bool> matchPredicate) where T1 : class where T2 : class 
        {
            var noSecond = new List<T1>();
            var pairs = new List<Tuple<T1, T2>>();

            foreach (var f in firsts)
            {
                var second = seconds.SingleOrDefault(s => matchPredicate(f, s));
                if (second == null)
                {
                    noSecond.Add(f);
                }
                else
                {
                    pairs.Add(new Tuple<T1, T2>(f,second));
                }
            }

            var noFirst = seconds.Where(s => !pairs.Select(p => p.Item2).Contains(s)).ToList();

            return new PairResult<T1, T2>() {Matches = pairs, NoFirst = noFirst, NoSecond = noSecond};
        }
    }

    public class PairResult<TFirst,TSecond>
    {
        public ICollection<Tuple<TFirst, TSecond>> Matches { get; set; }
        public ICollection<TFirst> NoSecond { get; set; }
        public ICollection<TSecond> NoFirst { get; set; } 
    }
}
