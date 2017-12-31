using Famoser.Bookmarked.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Famoser.Bookmarked.Tests.UWP
{
    [TestClass]
    public class EncryptionTests
    {
        [TestMethod]
        public void TestEncrypt()
        {
            var password = "password";
            var payload = "hi mom";

            var encryptionService = new EncryptionService();
            var encryptedStuff = encryptionService.Encrypt(payload, password);

            var decrypted = encryptionService.Decrypt(encryptedStuff, password);
            Assert.IsTrue(decrypted == payload);
        }
    }
}
