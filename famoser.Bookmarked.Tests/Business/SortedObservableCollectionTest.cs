using System.Collections.Generic;
using Famoser.Bookmarked.Business.List;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.Bookmarked.Tests.Business
{
    [TestClass]
    public class SortedObservableCollectionTest
    {
        [TestMethod]
        public void TestSimpleAdd()
        {
            var list = new SortedObservableCollection<string>(Comparer<string>.Default) {"a", "b", "c"};

            Assert.IsTrue(list[0] == "a");
            Assert.IsTrue(list[1] == "b");
            Assert.IsTrue(list[2] == "c");
        }

        [TestMethod]
        public void TestReverseAdd()
        {
            var list = new SortedObservableCollection<string>(Comparer<string>.Default) {"c", "b", "a"};

            Assert.IsTrue(list[0] == "a");
            Assert.IsTrue(list[1] == "b");
            Assert.IsTrue(list[2] == "c");
        }

        [TestMethod]
        public void TestComplexAdd()
        {
            var list = new SortedObservableCollection<string>(Comparer<string>.Default) {"b", "c", "a"};

            Assert.IsTrue(list[0] == "a");
            Assert.IsTrue(list[1] == "b");
            Assert.IsTrue(list[2] == "c");
        }
    }
}
