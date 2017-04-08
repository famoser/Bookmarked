using System;

namespace Famoser.Bookmarked.Presentation.Universal.Entity
{
    internal class NavigationParameter
    {
        public string Name { get; set; }
        public Type EditFrameType { get; set; }
        public Type ViewFrameType { get; set; }
        public Type ViewModelType { get; set; }
    }
}
