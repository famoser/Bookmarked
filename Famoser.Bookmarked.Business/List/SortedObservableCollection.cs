using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Famoser.Bookmarked.Business.List
{
    public class SortedObservableCollection<T> : ObservableCollection<T>
    {
        private readonly Comparer<T> _comparator;
        public SortedObservableCollection(Comparer<T> comparator)
        {
            _comparator = comparator;
        }

        public new void Add(T item)
        {
            using (var enumerator = GetEnumerator())
            {
                int i = 0;
                while (enumerator.MoveNext())
                {
                    if (_comparator.Compare(enumerator.Current, item) == 1)
                    {
                        Insert(i, item);
                        return;
                    }
                    i++;
                }
                base.Add(item);
            }
        }
    }
}
