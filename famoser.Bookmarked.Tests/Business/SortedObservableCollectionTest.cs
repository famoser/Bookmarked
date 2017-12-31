using System.Collections.Generic;
using System.Linq;
using System.Text;
using Famoser.Bookmarked.Business.List;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace famoser.Bookmarked.Tests.Business
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
