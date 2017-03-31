using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace famoser.Bookmarked.Tests
{
    [TestClass]
    public class CSharpTests
    {
        [TestMethod]
        public void TestDictionary()
        {
            var dic = new Dictionary<string, int> { ["hi mom"] = 2 };
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestDictionarySerialize()
        {
            var dic1 = new Dictionary<string, int> { ["hi mom"] = 2 };
            var json = JsonConvert.SerializeObject(dic1);
            var dic2 = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            Assert.IsTrue(dic1.Count == dic2.Count && !dic1.Except(dic2).Any());
        }
        [TestMethod]
        public void TestEncoding()
        {
            var bytes = new byte[256];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)i;
            }

            var fullCodePageEncoding = Encoding.GetEncoding("iso-8859-1");
            var utf8Encoding = Encoding.UTF8;

            var newBytes = Encoding.Convert(fullCodePageEncoding, utf8Encoding, bytes);
            var text = utf8Encoding.GetString(bytes, 0, bytes.Length);

            Assert.IsTrue(newBytes.Length > bytes.Length);

            var rawbytes = utf8Encoding.GetBytes(text);
            var bytes2 = Encoding.Convert(utf8Encoding, fullCodePageEncoding, rawbytes);

            Assert.IsTrue(bytes2.Length == 256);

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], bytes2[i]);
            }
        }
    }
}
