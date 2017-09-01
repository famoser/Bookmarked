using System;
using System.Collections.ObjectModel;

namespace Famoser.Bookmarked.Business.Extensions
{
    public static class ObservableCollection
    {
        public static void AddUniqueSorted<TValue>(this ObservableCollection<TValue> target, TValue value) where TValue : IComparable
        {
            if (target.Contains(value))
                return;
            for (var index = 0; index < target.Count; index++)
            {
                var source1 = target[index];
                if (source1.CompareTo(value) < 0)
                {
                    target.Insert(index, value);
                    return;
                }
            }
            target.Add(value);
        }
    }
}
