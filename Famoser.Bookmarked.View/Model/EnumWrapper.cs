using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.Bookmarked.View.Model
{
    public class ValueWrapper<T>
    {
        public ValueWrapper(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
