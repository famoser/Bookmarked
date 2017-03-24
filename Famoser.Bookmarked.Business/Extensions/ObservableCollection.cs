using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.Bookmarked.Business.Extensions
{
    public static class ObservableCollection
    {
        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector, bool isAZ)
        {
            if (isAZ)
            {
                List<TSource> sortedList = source.OrderBy(keySelector).ToList();
                source.Clear();
                foreach (var sortedItem in sortedList)
                {
                    source.Add(sortedItem);
                }
            }
            else
            {
                List<TSource> sortedList = source.OrderByDescending(keySelector).ToList();
                source.Clear();
                foreach (var sortedItem in sortedList)
                {
                    source.Add(sortedItem);
                }
            }
        }
    }
}
