using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Famoser.Bookmarked.Business.Services.Interfaces;

namespace Famoser.Bookmarked.Presentation.Webpage.Platform
{
    /// <summary>
    /// we dont care about progress in our simple viewer
    /// </summary>
    public class SimpleProgressService : ISimpleProgressService
    {
        public void InitializeProgressBar(int total)
        {
            
        }

        public void IncrementProgress()
        {
            
        }

        public void HideProgress()
        {

        }
    }
}