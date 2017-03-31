using System;
using System.Collections.ObjectModel;

namespace Famoser.Bookmarked.Business.Extensions
{
    public static class ObservableCollection
    {
        public static void AddUniqueSorted<TValue>(this ObservableCollection<TValue> source, TValue value) where TValue : IComparable
        {
            if (source.Contains(value))
                return;
            for (var index = 0; index < source.Count; index++)
            {
                var source1 = source[index];
                if (source1.CompareTo(value) < 0)
                {
                    source.Insert(index, value);
                    return;
                }
            }
            source.Add(value);
        }
    }
}
