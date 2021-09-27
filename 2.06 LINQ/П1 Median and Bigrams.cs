using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        public static double Median(this IEnumerable<double> items)
        {
            var itemsList = items.ToList();
            var count = itemsList.Count;
            if (count == 0) throw new InvalidOperationException();
            
            itemsList.Sort();
            return count % 2 == 0 ? (itemsList[count / 2 - 1] + itemsList[count / 2]) / 2 
                : itemsList[count / 2];
        }
        
        public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
        {
            var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext()) yield break;
			
            while (true)
            {
                var prev = enumerator.Current;
                if (!enumerator.MoveNext()) yield break;
                yield return Tuple.Create(prev, enumerator.Current);
            }
        }
    }
}