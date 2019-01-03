using System;
using System.Collections.ObjectModel;

namespace Famoser.Bookmarked.Business.Extensions
{
    public static class ObservableCollection
    {
        public static bool AddUniqueSorted<TValue>(this ObservableCollection<TValue> target, TValue value) where TValue : IComparable
        {
            if (target.Contains(value))
                return false;

            //you can find a O(log n) solution here with binary search
            //currently performance not important because average size is <10
            for (var index = 0; index < target.Count; index++)
            {
                var source1 = target[index];
                var compare = source1.CompareTo(value);
                if (compare <= 0)
                {
                    target.Insert(index, value);
                    return true;
                }
            }
            target.Add(value);
            return true;
        }
    }
}
