using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.Bookmarked.Business.Models;
using Famoser.Bookmarked.Business.Models.Base;

namespace Famoser.Bookmarked.Business.List.Comparators
{
    class BaseModelComparator<T> : Comparer<T>
        where T : BaseModel
    {
        public override int Compare(T x, T y)
        {
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }
}
