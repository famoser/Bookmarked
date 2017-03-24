using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace famoser.Bookmarked.Tests
{
    [TestClass]
    public class CSharpTests
    {
        [TestMethod]
        public void TestDictionary()
        {
            var dic = new Dictionary<string, int>();
            dic["hi mom"] = 2;
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestDictionarySerialize()
        {
            var dic = new Dictionary<string, int>();
            dic["hi mom"] = 2;
            //todo: jsonconvert dictionary
            Assert.IsTrue(true);
        }
    }
}
