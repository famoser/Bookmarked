using System;
using System.Threading.Tasks;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.Command
{
    internal class MyLoadingRelayCommand<T> : LoadingRelayCommand<T>
    {
        public MyLoadingRelayCommand(Action<T> execute, Func<T, bool> canExecute = null, bool disableWhileExecuting = true) : base(execute, canExecute, disableWhileExecuting)
        {
        }

        public MyLoadingRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, bool disableWhileExecuting = true) : base(execute, canExecute, disableWhileExecuting)
        {
        }
    }
}
