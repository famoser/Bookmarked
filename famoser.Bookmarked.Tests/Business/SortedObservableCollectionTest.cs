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
            var list = new SortedObservableCollection<string>(Comparer<string>.Default);
            list.Add("a");
            list.Add("b");
            list.Add("c");

            Assert.IsTrue(list[0] == "a");
            Assert.IsTrue(list[1] == "b");
            Assert.IsTrue(list[2] == "c");
        }

        [TestMethod]
        public void TestReverseAdd()
        {
            var list = new SortedObservableCollection<string>(Comparer<string>.Default);
            list.Add("c");
            list.Add("b");
            list.Add("a");

            Assert.IsTrue(list[0] == "a");
            Assert.IsTrue(list[1] == "b");
            Assert.IsTrue(list[2] == "c");
        }

        [TestMethod]
        public void TestComplexAdd()
        {
            var list = new SortedObservableCollection<string>(Comparer<string>.Default);
            list.Add("b");
            list.Add("c");
            list.Add("a");

            Assert.IsTrue(list[0] == "a");
            Assert.IsTrue(list[1] == "b");
            Assert.IsTrue(list[2] == "c");
        }
    }
}
