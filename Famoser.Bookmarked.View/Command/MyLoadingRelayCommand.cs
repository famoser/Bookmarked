using System;
using System.Threading.Tasks;
using Famoser.FrameworkEssentials.View.Commands;

namespace Famoser.Bookmarked.View.Command
{
    internal class MyLoadingRelayCommand : LoadingRelayCommand
    {
        public MyLoadingRelayCommand(Action execute, Func<bool> canExecute = null, bool disableWhileExecuting = true) : base(execute, canExecute, disableWhileExecuting)
        {
        }

        public MyLoadingRelayCommand(Func<Task> execute, Func<bool> canExecute = null, bool disableWhileExecuting = true) : base(execute, canExecute, disableWhileExecuting)
        {
        }
    }
}
