using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.Bookmarked.Business.Services.Interfaces
{
    public interface IViewService
    {
        void CheckBeginInvokeOnUi(Action action);
    }
}
